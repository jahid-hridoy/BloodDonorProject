using System.Linq.Expressions;
using BloodDonorProject.Models;

namespace BloodDonorProject.Services.Interfaces;

public interface IBloodDonorService
{
    Task<BloodDonor?> GetByIdAsync(int id);
    Task<List<BloodDonor>> GetAllAsync(string bloodGroup, string address, bool? eligible);
    void Add(BloodDonor bloodDonor);
    void Update(BloodDonor bloodDonor);
    void Delete(BloodDonor bloodDonor);
}