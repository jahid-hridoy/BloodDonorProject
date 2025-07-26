using System.Linq.Expressions;
using BloodDonorProject.Models;
namespace BloodDonorProject.Repositories.Interfaces;

public interface IBloodDonorRepository
{
    Task<BloodDonor?> GetByIdAsync(int id);
    Task<IEnumerable<BloodDonor>> GetAllAsync();
    Task<IEnumerable<BloodDonor>> FindAllAsync(Expression<Func<BloodDonor, bool>> predicate);
    void Add(BloodDonor bloodDonor);
    void Update(BloodDonor bloodDonor);
    void Delete(BloodDonor bloodDonor);
}