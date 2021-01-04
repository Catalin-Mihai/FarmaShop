using Microsoft.EntityFrameworkCore.Migrations;

namespace FarmaShop.Data.Migrations
{
    public partial class FarmaShopV13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ApplicationUserInfos_UserInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserInfoId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ApplicationUserInfos",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserInfos_UserId",
                table: "ApplicationUserInfos",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserInfos_AspNetUsers_UserId",
                table: "ApplicationUserInfos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserInfos_AspNetUsers_UserId",
                table: "ApplicationUserInfos");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserInfos_UserId",
                table: "ApplicationUserInfos");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ApplicationUserInfos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserInfoId",
                table: "AspNetUsers",
                column: "UserInfoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ApplicationUserInfos_UserInfoId",
                table: "AspNetUsers",
                column: "UserInfoId",
                principalTable: "ApplicationUserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
