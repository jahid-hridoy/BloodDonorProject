using BloodDonorProject.Data.Interfaces;
using BloodDonorProject.Repositories.Interfaces;
using BloodDonorProject.Services.Implementations;
using BloodDonorProject.Services.Interfaces;
using Microsoft.Build.Framework;

namespace BloodDonorProject.Data.Implementations
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly BloodDonorDbContext _context;
        public IBloodDonorRepository bloodDonorRepository { get; }
        public IDonationRepository donationRepository { get; }
        public UnitOfWork(BloodDonorDbContext context, IBloodDonorRepository _bloodDonorRepository, IDonationRepository _donationRepository)
        {
            _context = context;
            bloodDonorRepository = _bloodDonorRepository;
            donationRepository = _donationRepository;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
