using BloodDonorProject.Data;
using BloodDonorProject.Models;
using BloodDonorProject.Repositories.Interfaces;

namespace BloodDonorProject.Repositories.Implementations
{
    public class DonationRepository: Repository<Donation>, IDonationRepository
    {
        public DonationRepository(BloodDonorDbContext context): base(context)
        {

        }

    }
}
