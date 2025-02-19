using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystemAPIs.Migrations
{
    /// <inheritdoc />
    public partial class CompositePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountsTransactions",
                table: "AccountsTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AccountsTransactions_AccountId",
                table: "AccountsTransactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountsTransactions",
                table: "AccountsTransactions",
                columns: new[] { "AccountId", "TransactionId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountsTransactions",
                table: "AccountsTransactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountsTransactions",
                table: "AccountsTransactions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsTransactions_AccountId",
                table: "AccountsTransactions",
                column: "AccountId");
        }
    }
}
