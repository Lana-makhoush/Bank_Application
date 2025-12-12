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
                name: "SubAccountTypeId",
                table: "SubAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubAccounts_SubAccountTypeId",
                table: "SubAccounts",
                column: "SubAccountTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubAccounts_AccountTypes_SubAccountTypeId",
                table: "SubAccounts",
                column: "SubAccountTypeId",
                principalTable: "AccountTypes",
                principalColumn: "AccountTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubAccounts_AccountTypes_SubAccountTypeId",
                table: "SubAccounts");

            migrationBuilder.DropIndex(
                name: "IX_SubAccounts_SubAccountTypeId",
                table: "SubAccounts");

            migrationBuilder.DropColumn(
                name: "SubAccountTypeId",
                table: "SubAccounts");
        }
    }
}
