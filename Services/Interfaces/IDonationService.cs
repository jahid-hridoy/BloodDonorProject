using BloodDonorProject.Models;

namespace BloodDonorProject.Services.Interfaces
{
    public interface IDonationService
    {
        Task<IEnumerable<Donation>> GetAllDonationsAsync();
        Task<Donation?> GetDonationByIdAsync(int id);
        Task CreateDonation(Donation donation);
        Task UpdateDonation(int id, Donation donation);
        Task DeleteDonation(int id);
    }
}
