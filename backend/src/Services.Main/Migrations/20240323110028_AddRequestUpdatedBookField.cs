using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestUpdatedBookField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestTypeId",
                table: "Request",
                newName: "UpdatedBookId");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Book",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Request_UpdatedBookId",
                table: "Request",
                column: "UpdatedBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Book_UpdatedBookId",
                table: "Request",
                column: "UpdatedBookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Book_UpdatedBookId",
                table: "Request");

            migrationBuilder.DropIndex(
                name: "IX_Request_UpdatedBookId",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "UpdatedBookId",
                table: "Request",
                newName: "RequestTypeId");
        }
    }
}
