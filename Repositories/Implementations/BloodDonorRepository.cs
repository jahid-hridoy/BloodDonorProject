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
    public async Task<IEnumerable<BloodDonor>> GetAllAsync()
    {
        IQueryable<BloodDonor> query = _context.BloodDonors;
        if (!string.IsNullOrEmpty(bloodGroup))
        {
            query = query.Where(d => d.BloodGroup.ToString() == bloodGroup);
        }

        if (!string.IsNullOrEmpty(address))
        {
            query = query.Where(d => d.Address != null && d.Address.ToString() == address);
        }
        var donors = query.Select(d => new BloodDonorListViewModel
        {
            Id = d.Id,
            FullName = d.FullName,
            ContactNumber = d.ContactNumber,
            Age = DateTime.Now.Year - d.DateOfBirth.Year,
            Email = d.Email,
            BloodGroup = d.BloodGroup.ToString(),
            Address = d.Address,
            ProfilePicture = d.ProfilePicture,
            LastDonationDate = d.LastDonationDate.HasValue? $"{(DateTime.Today - d.LastDonationDate.Value).Days} days ago": "Never",
            IsEligibleForDonation = (d.Weight >= 50 && d.Weight <= 150) && (d.LastDonationDate == null || (DateTime.Now - d.LastDonationDate.Value).TotalDays >= 90)
        }).ToList();
        // var donors = query.ToList();
        if (eligible.HasValue)
        {
            donors = donors.Where(d => d.IsEligibleForDonation == eligible).ToList();
        }
        return View(donors);
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