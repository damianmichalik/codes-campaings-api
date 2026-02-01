using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodesCampaigns.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TopUpField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "TopUps",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActiveFrom",
                table: "TopUps",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActiveTo",
                table: "TopUps",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TopUps",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TopUps",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerCode",
                table: "TopUps",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UsedAt",
                table: "TopUps",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WalletExpirationDate",
                table: "TopUps",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveFrom",
                table: "TopUps");

            migrationBuilder.DropColumn(
                name: "ActiveTo",
                table: "TopUps");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TopUps");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "TopUps");

            migrationBuilder.DropColumn(
                name: "PartnerCode",
                table: "TopUps");

            migrationBuilder.DropColumn(
                name: "UsedAt",
                table: "TopUps");

            migrationBuilder.DropColumn(
                name: "WalletExpirationDate",
                table: "TopUps");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "TopUps",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);
        }
    }
}
