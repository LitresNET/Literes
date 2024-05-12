using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Litres.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Order");

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiverId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Pending = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "acf15a31-1400-42a0-b97a-5f8ca9c74184");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "078129ac-d9fc-40a5-bd61-b664c27a0c99");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "4a1afbcd-e683-4203-8a84-f8df8a1194d6");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "c9d8636c-334b-457d-976b-53308e98cd42");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConcurrencyStamp",
                value: "95dbe57f-e1e5-469b-9d23-f5c2dfb07dc2");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConcurrencyStamp",
                value: "8546ac87-9ed8-4366-8cde-20bd109c1b94");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7L,
                column: "ConcurrencyStamp",
                value: "ae8a3e4e-c5e2-4f90-9334-129d2cfc501a");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8L,
                column: "ConcurrencyStamp",
                value: "321eccc9-4ccb-4d55-984e-ea1fb1517bb8");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9L,
                column: "ConcurrencyStamp",
                value: "45bae856-8a2f-4950-b8cc-5b28b03a79d4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10L,
                column: "ConcurrencyStamp",
                value: "a98e802e-1fcf-4c35-b13c-dc795ca20dc0");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ReceiverId",
                table: "Notification",
                column: "ReceiverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 1L,
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 2L,
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 3L,
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 4L,
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 5L,
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 6L,
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 7L,
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 8L,
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 9L,
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 10L,
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "87034fa0-bc5f-457e-a338-55b65e5cb1bc");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "32fe2254-0bbf-40b5-84f8-17928869c27c");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "46e3ddce-359c-4894-a3f8-a1839c13f599");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "321fcdab-16f0-47dc-9771-fcd817a47b0e");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ConcurrencyStamp",
                value: "541c6617-021e-4c39-9a5e-bb72de79619d");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ConcurrencyStamp",
                value: "bd7d803c-c544-4827-9d1f-354d24a9b6ea");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7L,
                column: "ConcurrencyStamp",
                value: "bd347b42-966b-40c9-be36-71d910522762");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8L,
                column: "ConcurrencyStamp",
                value: "3bf05880-a5c8-4e2d-82e8-1ee3d94a423a");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9L,
                column: "ConcurrencyStamp",
                value: "5633b7fe-1523-4d8c-a2ca-ad3f6ef62ea1");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10L,
                column: "ConcurrencyStamp",
                value: "3b3e79d8-787a-4e6f-838d-0a3dfef0400a");
        }
    }
}
