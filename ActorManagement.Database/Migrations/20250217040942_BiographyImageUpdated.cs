using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActorManagement.Database.Migrations
{
    /// <inheritdoc />
    public partial class BiographyImageUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "BiographyImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "BiographyImages",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
