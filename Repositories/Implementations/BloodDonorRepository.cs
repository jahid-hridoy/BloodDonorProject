using System.Linq.Expressions;
using BloodDonorProject.Data;
using BloodDonorProject.Models;
using BloodDonorProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonorProject.Repositories.Implementations;

public class BloodDonorRepository: IBloodDonorRepository
{
    private readonly BloodDonorDbContext _context;
    
    public BloodDonorRepository(BloodDonorDbContext context)
    {
        _context = context;
    }
    public async Task<BloodDonor?> GetByIdAsync(int id)
    {
        return await _context.BloodDonors.FindAsync(id);
    }
    public IQueryable<BloodDonor> GetAllAsync()
    {
        return _context.BloodDonors;
    }
    public async Task<IEnumerable<BloodDonor>> FindAllAsync(Expression<Func<BloodDonor, bool>> predicate)
    {
        return await _context.BloodDonors.Where(predicate).ToListAsync();
    }

    public void Add(BloodDonor bloodDonor)
    {
        _context.BloodDonors.Add(bloodDonor);
    }
    public void Update(BloodDonor bloodDonor)
    {
        _context.BloodDonors.Update(bloodDonor);
    }

    public void Delete(BloodDonor bloodDonor)
    {
        _context.BloodDonors.Remove(bloodDonor);
    }
}