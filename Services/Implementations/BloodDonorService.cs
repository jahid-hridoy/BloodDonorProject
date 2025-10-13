using BloodDonorProject.Data.Interfaces;
using BloodDonorProject.Models;
using BloodDonorProject.Repositories.Interfaces;
using BloodDonorProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            query = query.Where(d => d.BloodGroup.ToString() == bloodGroup);
        }

        if (!string.IsNullOrEmpty(address))
        {
            query = query.Where(d => d.Address != null && d.Address.ToString() == address);
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
    public void Add(BloodDonor bloodDonor)
    {
        _unitOfWork.bloodDonorRepository.Add(bloodDonor);
        _unitOfWork.SaveAsync();
    }

    public void Update(BloodDonor bloodDonor)
    {
        _unitOfWork.bloodDonorRepository.Update(bloodDonor);
        _unitOfWork.SaveAsync();
    }

    public void Delete(BloodDonor bloodDonor)
    {
        _unitOfWork.bloodDonorRepository.Delete(bloodDonor);
        _unitOfWork.SaveAsync();
    }   
    
}