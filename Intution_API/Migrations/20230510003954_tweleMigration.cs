using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intution_API.Migrations
{
    public partial class tweleMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_UserCart_UserCartId",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCart",
                table: "UserCart");

            migrationBuilder.RenameTable(
                name: "UserCart",
                newName: "UserCarts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCarts",
                table: "UserCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_UserCarts_UserCartId",
                table: "Carts",
                column: "UserCartId",
                principalTable: "UserCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_UserCarts_UserCartId",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCarts",
                table: "UserCarts");

            migrationBuilder.RenameTable(
                name: "UserCarts",
                newName: "UserCart");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCart",
                table: "UserCart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_UserCart_UserCartId",
                table: "Carts",
                column: "UserCartId",
                principalTable: "UserCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
