using System.Linq.Expressions;
using BloodDonorProject.Models;

namespace BloodDonorProject.Services.Interfaces;

public interface IBloodDonorService
{
    Task<BloodDonor?> GetByIdAsync(int id);
    IEnumerable<BloodDonor> GetAllAsync(string bloodGroup, string address, bool? eligible);
    Task Add(BloodDonor bloodDonor);
    Task Update(BloodDonor bloodDonor);
    Task Delete(BloodDonor bloodDonor);
}