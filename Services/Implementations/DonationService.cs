using BloodDonorProject.Data;
using BloodDonorProject.Data.Interfaces;
using BloodDonorProject.Models;
using BloodDonorProject.Services.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BloodDonorProject.Services.Implementations
{
    public class DonationService: IDonationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DonationService(IUnitOfWork unitOfWork) { 
            _unitOfWork = unitOfWork;
        }
        // Implement methods here

        public async Task<IEnumerable<Donation>> GetAllDonationsAsync()
        {
            return await _unitOfWork.donationRepository.GetAllAsync().ToListAsync();
        }
        public async Task<Donation?> GetDonationByIdAsync(int id)
        {
            return await _unitOfWork.donationRepository.GetByIdAsync(id);
        }
        public async Task CreateDonation(Donation donation)
        {
            _unitOfWork.donationRepository.Add(donation);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateDonation(int id, Donation donation)
        {
            var existingDonation = await _unitOfWork.donationRepository.GetByIdAsync(id);
            if (existingDonation != null)
            {
                existingDonation.DonationDate = donation.DonationDate;
                existingDonation.BloodDonorId = donation.BloodDonorId;
                _unitOfWork.donationRepository.Update(existingDonation);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteDonation(int id)
        {
            var donation = await _unitOfWork.donationRepository.GetByIdAsync(id);
            if (donation != null)
            {
                _unitOfWork.donationRepository.Delete(donation);
                await _unitOfWork.SaveAsync();
            }
        }   
    }
}
