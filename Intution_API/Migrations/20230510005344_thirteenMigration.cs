using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intution_API.Migrations
{
    public partial class thirteenMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_UserCarts_UserCartId",
                table: "Carts");

            migrationBuilder.DropTable(
                name: "UserCarts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserCartId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "UserCartId",
                table: "Carts",
                newName: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Carts",
                newName: "UserCartId");

            migrationBuilder.CreateTable(
                name: "UserCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCarts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserCartId",
                table: "Carts",
                column: "UserCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_UserCarts_UserCartId",
                table: "Carts",
                column: "UserCartId",
                principalTable: "UserCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
