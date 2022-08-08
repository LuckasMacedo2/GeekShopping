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
        private ICartRepository _repository;
        private IRabbitMQMessageSender _rabbitMQMessageSender;

        public CartController(ICartRepository repository, IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); ;
            _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));
        }

        [HttpGet("find-cart/{userId}")]
        public async Task<ActionResult<CartVO>> FindById(string userId)
        {
            var product = await _repository.FindCartByUserId(userId);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart(CartVO cartVO)
        {
            var product = await _repository.SaveOrUpdateCart(cartVO);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartVO>> UpdateCart(CartVO cartVO)
        {
            var product = await _repository.SaveOrUpdateCart(cartVO);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(int id)
        {
            var status = await _repository.RemoveForCart(id);
            if (!status) return BadRequest();
            return Ok(status);
        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartVO>> ApplyCoupon(CartVO cartVO)
        {
            var status = await _repository.ApplyCoupon(cartVO.CartHeader.UserId, cartVO.CartHeader.CouponCode);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult<CartVO>> RemoveCoupon(string userId)
        {
            var status = await _repository.RemoveCoupon(userId);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CartVO>> Checkout(CheckoutHeaderVO checkoutHeaderVO)
        {
            if (checkoutHeaderVO?.UserId == null) return BadRequest();
            var cart = await _repository.FindCartByUserId(checkoutHeaderVO.UserId);
            if (cart == null) return NotFound();
            checkoutHeaderVO.CartDetails = cart.CartDetails;
            checkoutHeaderVO.DateTime = DateTime.Now;

            // RabbitMQ logic comes here
            _rabbitMQMessageSender.SendMessage(checkoutHeaderVO, "checkoutqueue");

            return Ok(checkoutHeaderVO);
        }
    }
}
