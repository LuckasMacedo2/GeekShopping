using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSernder;
using GeekShopping.CartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private ICartRepository _cartRepository;
        private ICouponRepository _couponRepository;
        private IRabbitMQMessageSender _rabbitMQMessageSender;

        public CartController(ICartRepository cartRepository, IRabbitMQMessageSender rabbitMQMessageSender, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository)); ;
            _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));
            _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository)); ;
        }

        [HttpGet("find-cart/{userId}")]
        public async Task<ActionResult<CartVO>> FindById(string userId)
        {
            var product = await _cartRepository.FindCartByUserId(userId);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart(CartVO cartVO)
        {
            var product = await _cartRepository.SaveOrUpdateCart(cartVO);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartVO>> UpdateCart(CartVO cartVO)
        {
            var product = await _cartRepository.SaveOrUpdateCart(cartVO);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(int id)
        {
            var status = await _cartRepository.RemoveForCart(id);
            if (!status) return BadRequest();
            return Ok(status);
        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartVO>> ApplyCoupon(CartVO cartVO)
        {
            var status = await _cartRepository.ApplyCoupon(cartVO.CartHeader.UserId, cartVO.CartHeader.CouponCode);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult<CartVO>> RemoveCoupon(string userId)
        {
            var status = await _cartRepository.RemoveCoupon(userId);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CartVO>> Checkout(CheckoutHeaderVO checkoutHeaderVO)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", "");

            if (checkoutHeaderVO?.UserId == null) return BadRequest();
            var cart = await _cartRepository.FindCartByUserId(checkoutHeaderVO.UserId);
            if (cart == null) return NotFound();

            if(!string.IsNullOrEmpty(checkoutHeaderVO.CouponCode))
            {
                CouponVO coupon = await _couponRepository.GetCouponByCouponCode(checkoutHeaderVO.CouponCode, token);
                if(checkoutHeaderVO.DiscountTotal != coupon.DiscountAmount)
                {
                    return StatusCode(412); // Pré-condição falhou, mudança entre a primeira requisição e a requisição final
                }
            }

            checkoutHeaderVO.CartDetails = cart.CartDetails;
            checkoutHeaderVO.DateTime = DateTime.Now;

            // RabbitMQ logic comes here
            _rabbitMQMessageSender.SendMessage(checkoutHeaderVO, "checkoutqueue");

            // Limpando o carrinho
            await _cartRepository.ClearCart(checkoutHeaderVO.UserId);

            return Ok(checkoutHeaderVO);
        }
    }
}
