using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Litres.Data.Migrations
{
    /// <inheritdoc />
    public partial class remove_tph_for_publisher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publisher_User_Id",
                table: "Publisher");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Publisher",
                newName: "UserId");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "d2fc56d9-f2ac-4e23-8488-3d8864747063");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "eb185642-dc7b-417c-8509-73086cfdc199");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "cc3269ea-67ae-441d-abdc-f2b8f26bfa26");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "5f7acc0e-d575-490f-8df4-dbdc65b9dc15");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConcurrencyStamp",
                value: "99b20731-cb4e-45fb-a5e6-b3f803292859");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConcurrencyStamp",
                value: "f227be2b-cdbf-4c65-a18d-00f03284955c");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7L,
                column: "ConcurrencyStamp",
                value: "065f1f77-e454-41f6-890c-ec4286825da8");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8L,
                column: "ConcurrencyStamp",
                value: "207dcca2-0d55-4fd2-9d44-95654f1ce92c");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9L,
                column: "ConcurrencyStamp",
                value: "c5d06c87-d88c-43b3-8f9e-55cd47e70d92");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10L,
                column: "ConcurrencyStamp",
                value: "7dd98870-4cc7-49da-86fe-b0dd033456e0");

            migrationBuilder.AddForeignKey(
                name: "FK_Publisher_User_UserId",
                table: "Publisher",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publisher_User_UserId",
                table: "Publisher");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Publisher",
                newName: "Id");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "df67f7e3-d762-4701-9882-454ac24790c5");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "109f86d7-86ce-4a8b-882b-6eae5841c013");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "abdd6740-7587-451e-b17d-355ae89bba4b");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "d202281c-99b5-4fce-977e-6e9e184da7b1");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConcurrencyStamp",
                value: "da05117c-e0c8-45ec-b3ba-b2ab160cd497");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConcurrencyStamp",
                value: "95171555-cf81-4df3-a51d-25d89a866fdb");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7L,
                column: "ConcurrencyStamp",
                value: "f4fb27d4-2b82-4957-9433-6e4c0d0c3254");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8L,
                column: "ConcurrencyStamp",
                value: "1240cdfb-2cd3-4c8f-9abc-9785e8f8a9bb");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9L,
                column: "ConcurrencyStamp",
                value: "1a43c1b3-f904-4d92-9c27-1afe8352a669");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10L,
                column: "ConcurrencyStamp",
                value: "696057eb-67b1-4c3a-80ac-417ad4abc7a5");

            migrationBuilder.AddForeignKey(
                name: "FK_Publisher_User_Id",
                table: "Publisher",
                column: "Id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
