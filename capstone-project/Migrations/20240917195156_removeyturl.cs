using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace capstone_project.Migrations
{
    /// <inheritdoc />
    public partial class removeyturl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YouTubeUrl",
                table: "Games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YouTubeUrl",
                table: "Games",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
