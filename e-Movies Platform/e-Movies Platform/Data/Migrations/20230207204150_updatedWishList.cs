using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Movies_Platform.Data.Migrations
{
    public partial class updatedWishList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WishList",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishList_UserId",
                table: "WishList",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WishList_AspNetUsers_UserId",
                table: "WishList",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishList_AspNetUsers_UserId",
                table: "WishList");

            migrationBuilder.DropIndex(
                name: "IX_WishList_UserId",
                table: "WishList");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WishList");
        }
    }
}
