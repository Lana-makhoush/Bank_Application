using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_Application.Migrations
{
    /// <inheritdoc />
    public partial class Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountFeatures");

            migrationBuilder.CreateTable(
                name: "AccountTypeFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountTypeId = table.Column<int>(type: "int", nullable: true),
                    FeatureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypeFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountTypeFeatures_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "AccountTypeId");
                    table.ForeignKey(
                        name: "FK_AccountTypeFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "FeatureId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountTypeFeatures_AccountTypeId",
                table: "AccountTypeFeatures",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTypeFeatures_FeatureId",
                table: "AccountTypeFeatures",
                column: "FeatureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTypeFeatures");

            migrationBuilder.CreateTable(
                name: "AccountFeatures",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountFeatures", x => new { x.AccountId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_AccountFeatures_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "FeatureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountFeatures_FeatureId",
                table: "AccountFeatures",
                column: "FeatureId");
        }
    }
}
