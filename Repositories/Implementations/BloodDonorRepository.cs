using System.Linq.Expressions;
using BloodDonorProject.Data;
using BloodDonorProject.Models;
using BloodDonorProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonorProject.Repositories.Implementations;

public class BloodDonorRepository: Repository<BloodDonor>, IBloodDonorRepository
{
    public BloodDonorRepository(BloodDonorDbContext context): base(context)
    {
        
    }
}