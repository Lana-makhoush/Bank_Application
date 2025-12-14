using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_Application.Migrations
{
    /// <inheritdoc />
    public partial class SubAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubAccountId",
                table: "ScheduledTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTransactions_SubAccountId",
                table: "ScheduledTransactions",
                column: "SubAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledTransactions_SubAccounts_SubAccountId",
                table: "ScheduledTransactions",
                column: "SubAccountId",
                principalTable: "SubAccounts",
                principalColumn: "SubAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledTransactions_SubAccounts_SubAccountId",
                table: "ScheduledTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledTransactions_SubAccountId",
                table: "ScheduledTransactions");

            migrationBuilder.DropColumn(
                name: "SubAccountId",
                table: "ScheduledTransactions");
        }
    }
}
