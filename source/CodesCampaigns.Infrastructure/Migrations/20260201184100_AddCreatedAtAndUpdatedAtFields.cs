using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodesCampaigns.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtAndUpdatedAtFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TopUps",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Campaigns",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Campaigns",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TopUps");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Campaigns");
        }
    }
}
