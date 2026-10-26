using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiraajApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentInfoToHeroSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultCurrency",
                table: "Packages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OriginalPriceUsd",
                table: "Packages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceUsd",
                table: "Packages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountName",
                table: "HeroSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "HeroSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankIban",
                table: "HeroSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "HeroSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankSwiftCode",
                table: "HeroSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstapayLink",
                table: "HeroSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VodafoneCashNumber",
                table: "HeroSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppNumber",
                table: "HeroSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultCurrency",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "OriginalPriceUsd",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "PriceUsd",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "BankAccountName",
                table: "HeroSettings");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "HeroSettings");

            migrationBuilder.DropColumn(
                name: "BankIban",
                table: "HeroSettings");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "HeroSettings");

            migrationBuilder.DropColumn(
                name: "BankSwiftCode",
                table: "HeroSettings");

            migrationBuilder.DropColumn(
                name: "InstapayLink",
                table: "HeroSettings");

            migrationBuilder.DropColumn(
                name: "VodafoneCashNumber",
                table: "HeroSettings");

            migrationBuilder.DropColumn(
                name: "WhatsAppNumber",
                table: "HeroSettings");
        }
    }
}
