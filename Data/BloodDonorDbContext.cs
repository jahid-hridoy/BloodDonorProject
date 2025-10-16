using BloodDonorProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace BloodDonorProject.Data;

public class BloodDonorDbContext: IdentityDbContext<IdentityUser>
{
    public BloodDonorDbContext(DbContextOptions<BloodDonorDbContext> options) : base(options)
    {
    }
    public DbSet<Models.BloodDonor> BloodDonors { get; set; }
    public DbSet<Donation> Donations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BloodDonor>()
            .HasMany(d => d.Donations)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BloodDonor>()
            .HasData(
            new BloodDonor
            {
                Id = -1,
                FullName = "John Doe",
                ContactNumber = "123-456-7890",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@gmail.com",
                BloodGroup = BloodGroup.A_Positive,
                Weight = 70,
                Address = "123 Main St, City, Country",
                ProfilePicture = null,
                LastDonationDate = new DateTime(2023, 1, 1)
            }
        );
    }
}