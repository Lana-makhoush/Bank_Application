using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_Application.Migrations
{
    /// <inheritdoc />
    public partial class SupportTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SupportTickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SupportTickets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
