using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystemAPIs.Migrations
{
    /// <inheritdoc />
    public partial class rate_fk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Currency",
                table: "Transactions",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Currency",
                table: "Accounts",
                column: "Currency");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Rates_Currency",
                table: "Accounts",
                column: "Currency",
                principalTable: "Rates",
                principalColumn: "Currency",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Rates_Currency",
                table: "Transactions",
                column: "Currency",
                principalTable: "Rates",
                principalColumn: "Currency",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Rates_Currency",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Rates_Currency",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_Currency",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Currency",
                table: "Accounts");
        }
    }
}
