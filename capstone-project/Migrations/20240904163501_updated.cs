using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace capstone_project.Migrations
{
    /// <inheritdoc />
    public partial class updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewLikes_Reviews_ReviewId1",
                table: "ReviewLikes");

            migrationBuilder.DropIndex(
                name: "IX_ReviewLikes_ReviewId1",
                table: "ReviewLikes");

            migrationBuilder.DropColumn(
                name: "ReviewId1",
                table: "ReviewLikes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewId1",
                table: "ReviewLikes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewLikes_ReviewId1",
                table: "ReviewLikes",
                column: "ReviewId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewLikes_Reviews_ReviewId1",
                table: "ReviewLikes",
                column: "ReviewId1",
                principalTable: "Reviews",
                principalColumn: "ReviewId");
        }
    }
}
