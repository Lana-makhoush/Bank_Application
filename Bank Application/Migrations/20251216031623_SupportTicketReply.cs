using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_Application.Migrations
{
    /// <inheritdoc />
    public partial class SupportTicketReply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportTicketReplies",
                columns: table => new
                {
                    ReplyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: true),
                    ReplyText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    RepliedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketReplies", x => x.ReplyId);
                    table.ForeignKey(
                        name: "FK_SupportTicketReplies_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_SupportTicketReplies_SupportTickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "SupportTickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketReplies_EmployeeId",
                table: "SupportTicketReplies",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketReplies_TicketId",
                table: "SupportTicketReplies",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportTicketReplies");
        }
    }
}
