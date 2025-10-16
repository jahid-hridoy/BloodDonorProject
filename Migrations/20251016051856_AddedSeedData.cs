using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodDonorProject.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BloodDonors",
                columns: new[] { "Id", "Address", "BloodGroup", "ContactNumber", "DateOfBirth", "Email", "FullName", "LastDonationDate", "ProfilePicture", "Weight" },
                values: new object[] { -1, "123 Main St, City, Country", 0, "123-456-7890", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "john@gmail.com", "John Doe", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 70f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BloodDonors",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
