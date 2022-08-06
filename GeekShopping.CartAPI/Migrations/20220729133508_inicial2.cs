using Microsoft.EntityFrameworkCore.Migrations;

namespace GeekShopping.CartAPI.Migrations
{
    public partial class inicial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "coupun_code",
                table: "cart_header",
                newName: "coupon_code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "coupon_code",
                table: "cart_header",
                newName: "coupun_code");
        }
    }
}
