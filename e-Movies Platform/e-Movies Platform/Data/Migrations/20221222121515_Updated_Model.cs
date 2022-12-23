using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Movies_Platform.Data.Migrations
{
    public partial class Updated_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CastCrew_CastCrewRole_Roleid",
                table: "CastCrew");

            migrationBuilder.AlterColumn<int>(
                name: "Roleid",
                table: "CastCrew",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CastCrew_CastCrewRole_Roleid",
                table: "CastCrew",
                column: "Roleid",
                principalTable: "CastCrewRole",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CastCrew_CastCrewRole_Roleid",
                table: "CastCrew");

            migrationBuilder.AlterColumn<int>(
                name: "Roleid",
                table: "CastCrew",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CastCrew_CastCrewRole_Roleid",
                table: "CastCrew",
                column: "Roleid",
                principalTable: "CastCrewRole",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
