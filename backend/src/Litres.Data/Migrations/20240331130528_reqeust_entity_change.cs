using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Litres.Data.Migrations
{
    /// <inheritdoc />
    public partial class reqeust_entity_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UpdatedBookId",
                table: "Request",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UpdatedBookId",
                table: "Request",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "40cec47f-57a4-462a-893b-d1a63f6fcd6d");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "6f4e7aea-53c7-4641-ba5f-8c8f19722066");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "995f15d6-6156-44f3-b3a1-fb3c8d3d570b");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "9885f582-8c0e-4076-9581-d64502420e73");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConcurrencyStamp",
                value: "50564fd9-8933-4757-881f-839d41db2efd");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConcurrencyStamp",
                value: "35eddc2e-c9ca-4640-ad1f-9175a2cb93d9");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7L,
                column: "ConcurrencyStamp",
                value: "f47e3699-9c6f-463e-b8b2-c3cecbbc69ea");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8L,
                column: "ConcurrencyStamp",
                value: "79302e5d-c9b0-4dee-9b7a-5e486f638a23");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9L,
                column: "ConcurrencyStamp",
                value: "1ca0e690-1fd2-42bb-bfa6-a3d86807ff18");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10L,
                column: "ConcurrencyStamp",
                value: "ea884586-2655-495b-9d04-48d0744e52c4");
        }
    }
}
