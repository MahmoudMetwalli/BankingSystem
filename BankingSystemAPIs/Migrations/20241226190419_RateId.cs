using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystemAPIs.Migrations
{
    /// <inheritdoc />
    public partial class RateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rates",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Currency",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Accounts");

            migrationBuilder.AddColumn<Guid>(
                name: "RateId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Rates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RateId",
                table: "Accounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rates",
                table: "Rates",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RateId",
                table: "Transactions",
                column: "RateId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_Currency",
                table: "Rates",
                column: "Currency",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RateId",
                table: "Accounts",
                column: "RateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Rates_RateId",
                table: "Accounts",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Rates_RateId",
                table: "Transactions",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Rates_RateId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Rates_RateId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RateId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rates",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Rates_Currency",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_RateId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "RateId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "RateId",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Transactions",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Accounts",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rates",
                table: "Rates",
                column: "Currency");

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
    }
}
