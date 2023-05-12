using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intution_API.Migrations
{
    public partial class elevenMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserCartId",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserCart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCart", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserCartId",
                table: "Carts",
                column: "UserCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_UserCart_UserCartId",
                table: "Carts",
                column: "UserCartId",
                principalTable: "UserCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_UserCart_UserCartId",
                table: "Carts");

            migrationBuilder.DropTable(
                name: "UserCart");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserCartId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "UserCartId",
                table: "Carts");
        }
    }
}
