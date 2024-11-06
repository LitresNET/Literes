using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Litres.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChatPrepare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatSessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    From = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "4ade7f5e-d1c1-4ac7-bde8-435185c9cf64");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "2d0f3b66-d8f1-4c6d-9103-ad9e3c4e4cbb");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "41ffc5d3-03e4-4e09-af92-88891fd8e57d");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "8f1d2890-643c-494c-8dd2-2e4de6383f94");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConcurrencyStamp",
                value: "e20a7164-9492-42a8-8750-aac4e6eabacc");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConcurrencyStamp",
                value: "b4c138f0-6174-4d46-b057-6c95956ebd0c");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7L,
                column: "ConcurrencyStamp",
                value: "a627b09f-25fe-4142-b308-e774a173434d");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8L,
                column: "ConcurrencyStamp",
                value: "b52de047-3139-40b6-84b4-3f033eefb182");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9L,
                column: "ConcurrencyStamp",
                value: "1725017d-4d2b-48b3-aef9-af90fda1096f");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10L,
                column: "ConcurrencyStamp",
                value: "d1193002-7612-46df-97d5-e1a420b1e58e");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "d3a8b4ea-cd5b-4b07-901e-d74c3602f6a8");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "4eb1e8d8-2d0f-4fb0-b1fb-e7592e0a6bf9");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "3bd4efd2-2b88-492b-a184-bbb9aa78144f");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "90830569-6dea-4f2b-952f-72b178581a86");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConcurrencyStamp",
                value: "272c987e-0209-4981-a22b-1fca149ab0f5");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConcurrencyStamp",
                value: "e7b55d96-1604-4797-8221-c216d5cbfc8a");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7L,
                column: "ConcurrencyStamp",
                value: "598a2c34-9e72-489b-a828-16260027eb55");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8L,
                column: "ConcurrencyStamp",
                value: "979a7efe-c82f-43a9-b3ca-b42185f2a2b2");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9L,
                column: "ConcurrencyStamp",
                value: "313989b4-01d4-417b-8241-220a90de3aa4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10L,
                column: "ConcurrencyStamp",
                value: "579a51ff-4b81-48eb-8178-e244ff9bcedc");
        }
    }
}
