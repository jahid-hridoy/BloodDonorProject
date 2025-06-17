using BloodDonorProject.Models;
using Microsoft.EntityFrameworkCore;
namespace BloodDonorProject.Data;

public class BloodDonorDbContext: DbContext
{
    public BloodDonorDbContext(DbContextOptions<BloodDonorDbContext> options) : base(options)
    {
    }
    public DbSet<Models.BloodDonor> BloodDonors { get; set; }
    public DbSet<Donation> Donations { get; set; }
}