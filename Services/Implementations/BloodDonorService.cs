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

    public async Task<List<BloodDonor>> GetAllAsync(string bloodGroup, string address, bool? eligible)
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

        if (eligible.HasValue)
        {
            query = query.Where(d => IsEligible(d) == eligible.Value);
        }

        return query.ToList();
    }

    public static bool IsEligible(BloodDonor donor)
    {
        return (donor.Weight >= 50 && donor.Weight <= 150) &&
               (donor.LastDonationDate == null || (DateTime.Now - donor.LastDonationDate.Value).TotalDays >= 90);
    }
    public void Add(BloodDonor bloodDonor)
    {
        _bloodDonorRepository.Add(bloodDonor);
    }

    public void Update(BloodDonor bloodDonor)
    {
        _bloodDonorRepository.Update(bloodDonor);
    }

    public void Delete(BloodDonor bloodDonor)
    {
        _bloodDonorRepository.Delete(bloodDonor);
    }   
    
}