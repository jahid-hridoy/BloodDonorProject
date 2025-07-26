using BloodDonorProject.Models;
using BloodDonorProject.Repositories.Interfaces;
using BloodDonorProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonorProject.Services.Implementations;

public class BloodDonorService: IBloodDonorService
{
    private readonly IBloodDonorRepository _bloodDonorRepository;

    public BloodDonorService(IBloodDonorRepository bloodDonorRepository)
    {
        _bloodDonorRepository = bloodDonorRepository;
    }
    
    public async Task<BloodDonor?> GetByIdAsync(int id)
    {
        return await _bloodDonorRepository.GetByIdAsync(id);
    }

    public async Task<List<BloodDonorListViewModel>> GetAllAsync(string bloodGroup, string address, bool? eligible)
    {
        IQueryable<BloodDonor> query = _bloodDonorRepository.GetAllAsync(); // assuming this returns Task<IQueryable<...>>

        if (!string.IsNullOrEmpty(bloodGroup))
        {
            query = query.Where(d => d.BloodGroup.ToString() == bloodGroup);
        }

        if (!string.IsNullOrEmpty(address))
        {
            query = query.Where(d => d.Address != null && d.Address.ToString() == address);
        }

        var donors = await query.Select(d => new BloodDonorListViewModel
        {
            Id = d.Id,
            FullName = d.FullName,
            ContactNumber = d.ContactNumber,
            Age = DateTime.Now.Year - d.DateOfBirth.Year,
            Email = d.Email,
            BloodGroup = d.BloodGroup.ToString(),
            Address = d.Address,
            ProfilePicture = d.ProfilePicture,
            LastDonationDate = d.LastDonationDate.HasValue
                ? $"{(DateTime.Today - d.LastDonationDate.Value).Days} days ago"
                : "Never",
            IsEligibleForDonation = (d.Weight >= 50 && d.Weight <= 150) &&
                                    (d.LastDonationDate == null || (DateTime.Now - d.LastDonationDate.Value).TotalDays >= 90)
        }).ToListAsync();

        if (eligible.HasValue)
        {
            donors = donors.Where(d => d.IsEligibleForDonation == eligible.Value).ToList();
        }

        return donors;
    }


    public void Add(BloodDonor bloodDonor)
    {
        throw new NotImplementedException();
    }

    public void Update(BloodDonor bloodDonor)
    {
        throw new NotImplementedException();
    }

    public void Delete(BloodDonor bloodDonor)
    {
        throw new NotImplementedException();
    }   
    
}