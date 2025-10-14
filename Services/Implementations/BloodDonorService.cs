using BloodDonorProject.Data.Interfaces;
using BloodDonorProject.Models;
using BloodDonorProject.Repositories.Interfaces;
using BloodDonorProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BloodDonorProject.Services.Implementations;

public class BloodDonorService: IBloodDonorService
{
    private readonly IUnitOfWork _unitOfWork;

    public BloodDonorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BloodDonor?> GetByIdAsync(int id)
    {
        return await _unitOfWork.bloodDonorRepository.GetByIdAsync(id);
    }

    public IEnumerable<BloodDonor> GetAllAsync(string bloodGroup, string address, bool? eligible)
    {
        var query = _unitOfWork.bloodDonorRepository.GetAllAsync();

        if (!string.IsNullOrEmpty(bloodGroup))
        {
            if (Enum.TryParse<BloodGroup>(bloodGroup, out var groupEnum))
            {
                query = query.Where(d => d.BloodGroup == groupEnum);
            }
        }

        if (!string.IsNullOrEmpty(address))
        {
            query = query.Where(d => d.Address != null && d.Address.Contains(address));
        }

        if (eligible.HasValue)
        {
            var thresholdDate = DateTime.Now.AddDays(-90);
            query = query.Where(donor => ((donor.Weight >= 50 && donor.Weight <= 150) &&
               ((donor.LastDonationDate == null) || (donor.LastDonationDate <= thresholdDate))) == eligible.Value);
        }

        return query.AsEnumerable();
    }

    public static bool IsEligible(BloodDonor donor)
    {
        return (donor.Weight >= 50 && donor.Weight <= 150) &&
               (donor.LastDonationDate == null || (DateTime.Now - donor.LastDonationDate.Value).TotalDays >= 90);
    }
    public async Task Add(BloodDonor bloodDonor)
    {
        _unitOfWork.bloodDonorRepository.Add(bloodDonor);
        await _unitOfWork.SaveAsync();
    }

    public async Task Update(BloodDonor bloodDonor)
    {
        _unitOfWork.bloodDonorRepository.Update(bloodDonor);
        await _unitOfWork.SaveAsync();
    }

    public async Task Delete(BloodDonor bloodDonor)
    {
        _unitOfWork.bloodDonorRepository.Delete(bloodDonor);
        await _unitOfWork.SaveAsync();
    }   
    
}