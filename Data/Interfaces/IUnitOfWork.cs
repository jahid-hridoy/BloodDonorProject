using BloodDonorProject.Repositories.Interfaces;

namespace BloodDonorProject.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IBloodDonorRepository bloodDonorRepository { get; }
        IDonationRepository donationRepository { get; }
        Task<int> SaveAsync();
    }
}
