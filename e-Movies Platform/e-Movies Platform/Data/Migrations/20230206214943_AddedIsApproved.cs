using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Movies_Platform.Data.Migrations
{
    public partial class AddedIsApproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Isapproved",
                table: "WishList",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isapproved",
                table: "WishList");
        }
    }
}
