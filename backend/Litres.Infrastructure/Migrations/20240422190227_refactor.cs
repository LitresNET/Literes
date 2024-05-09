using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Litres.Data.Migrations
{
    /// <inheritdoc />
    public partial class refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityRole<long>User_Roles_RolesId",
                table: "IdentityRole<long>User");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityRole<long>User_User_UserId",
                table: "IdentityRole<long>User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "c7b63788-f748-4507-ba8b-05ac700bdb4d");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "d31a4e8e-990d-4382-9ec4-7d262e006262");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "70635919-aa4e-4cc2-8a0f-dfd6156fc103");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "a6222273-72eb-434b-adaa-147128249fb1");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConcurrencyStamp",
                value: "9c06bece-ae9a-4a0b-ac1c-b0ac981eb364");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConcurrencyStamp",
                value: "9cebfa28-62ba-4f83-8671-d153bdf7e487");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7L,
                column: "ConcurrencyStamp",
                value: "8e009071-f20b-4d28-b6cc-69f489cc3104");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8L,
                column: "ConcurrencyStamp",
                value: "0d60074f-ef0f-45dc-b0e8-135f8328afe6");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9L,
                column: "ConcurrencyStamp",
                value: "8cc6f404-ee90-48e0-995c-d13889eed55d");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10L,
                column: "ConcurrencyStamp",
                value: "44dfe17f-07d8-486a-b686-e1ca08d50ac5");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRole<long>User_Roles_RolesId",
                table: "IdentityRole<long>User",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRole<long>User_User_UserId",
                table: "IdentityRole<long>User",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityRole<long>User_Roles_RolesId",
                table: "IdentityRole<long>User");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityRole<long>User_User_UserId",
                table: "IdentityRole<long>User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "d2db7cd4-c3b0-4828-b6bf-5124b04ceb2a");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "a9d03285-9af2-4515-8ceb-75d562f8c92e");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "dfbd480d-8782-4ca8-a9cd-d029869210aa");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "6d6447ba-7b46-4425-8e4e-b93afbf3d7a5");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConcurrencyStamp",
                value: "f3a3b169-9ed2-497d-95b5-a13bfa0d8948");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConcurrencyStamp",
                value: "5846da29-1b11-4c64-8b9a-f37a1ad92c30");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7L,
                column: "ConcurrencyStamp",
                value: "723d9a43-847f-4cd2-ab00-8ae1ec2fb021");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8L,
                column: "ConcurrencyStamp",
                value: "e983df07-71d0-44aa-a3e0-339318393b12");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9L,
                column: "ConcurrencyStamp",
                value: "0dc332c6-803d-4174-bd24-1ea1b07320d1");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10L,
                column: "ConcurrencyStamp",
                value: "49071091-e0d5-4cb3-aa36-3d897e2dea40");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRole<long>User_Roles_RolesId",
                table: "IdentityRole<long>User",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRole<long>User_User_UserId",
                table: "IdentityRole<long>User",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
