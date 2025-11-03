using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10174327_GiftOfTheGiversWebApp.Migrations
{
    /// <inheritdoc />
    public partial class FixDisasterTableSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fix the typo in the primary key column name
            migrationBuilder.RenameColumn(
                name: "DISTATER_ID",
                table: "Disaster",
                newName: "DISASTER_ID");

            // Add missing DisasterName column
            migrationBuilder.AddColumn<string>(
                name: "DisasterName",
                table: "Disaster",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            // Add missing Description column
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Disaster",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            // Update existing foreign key references to use the correct column name
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsAllocation_Disaster_DISASTER_ID",
                table: "GoodsAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_MoneyAllocation_Disaster_DISASTER_ID",
                table: "MoneyAllocation");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsAllocation_Disaster_DISASTER_ID",
                table: "GoodsAllocation",
                column: "DISASTER_ID",
                principalTable: "Disaster",
                principalColumn: "DISASTER_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoneyAllocation_Disaster_DISASTER_ID",
                table: "MoneyAllocation",
                column: "DISASTER_ID",
                principalTable: "Disaster",
                principalColumn: "DISASTER_ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the foreign key updates
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsAllocation_Disaster_DISASTER_ID",
                table: "GoodsAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_MoneyAllocation_Disaster_DISASTER_ID",
                table: "MoneyAllocation");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsAllocation_Disaster_DISASTER_ID",
                table: "GoodsAllocation",
                column: "DISASTER_ID",
                principalTable: "Disaster",
                principalColumn: "DISTATER_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoneyAllocation_Disaster_DISASTER_ID",
                table: "MoneyAllocation",
                column: "DISASTER_ID",
                principalTable: "Disaster",
                principalColumn: "DISTATER_ID",
                onDelete: ReferentialAction.Restrict);

            // Remove the added columns
            migrationBuilder.DropColumn(
                name: "DisasterName",
                table: "Disaster");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Disaster");

            // Revert the column name back to the typo
            migrationBuilder.RenameColumn(
                name: "DISASTER_ID",
                table: "Disaster",
                newName: "DISTATER_ID");
        }
    }
}
