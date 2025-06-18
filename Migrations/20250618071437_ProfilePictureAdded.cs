using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodDonorProject.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePictureAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "BloodDonors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "BloodDonors");
        }
    }
}
