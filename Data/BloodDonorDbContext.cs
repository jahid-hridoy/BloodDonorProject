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
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Models.BloodDonor>()
    //         .HasMany(d => d.Donations)
    //         .WithOne()
    //         .OnDelete(DeleteBehavior.Cascade);
    //
    //     base.OnModelCreating(modelBuilder);
    // }
}