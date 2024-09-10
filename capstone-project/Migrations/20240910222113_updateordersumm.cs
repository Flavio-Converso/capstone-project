using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace capstone_project.Migrations
{
    /// <inheritdoc />
    public partial class updateordersumm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GamesOrdered",
                table: "OrderSummaries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "OrderSummaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "OrderSummaries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GamesOrdered",
                table: "OrderSummaries");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "OrderSummaries");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "OrderSummaries");
        }
    }
}
