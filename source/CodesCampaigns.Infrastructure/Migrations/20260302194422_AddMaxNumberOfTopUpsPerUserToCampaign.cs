using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodesCampaigns.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxNumberOfTopUpsPerUserToCampaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxNumberOfTopUpsPerUser",
                table: "Campaigns",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxNumberOfTopUpsPerUser",
                table: "Campaigns");
        }
    }
}
