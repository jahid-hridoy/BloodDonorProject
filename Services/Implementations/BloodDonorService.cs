using BloodDonorProject.Models;
using BloodDonorProject.Repositories.Interfaces;
using BloodDonorProject.Services.Interfaces;

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

    public Task<IEnumerable<BloodDonor>> GetAllAsync(string bloodGroup, string address, bool? eligible)
    {
        return await _bloodDonorRepository.GetAllAsync(bloodGroup, address, eligible);
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