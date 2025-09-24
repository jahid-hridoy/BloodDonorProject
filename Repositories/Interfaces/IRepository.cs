using BloodDonorProject.Models;
using System.Linq.Expressions;

namespace BloodDonorProject.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAllAsync();
        void Add(T donor);
        void Update(T donor);
        void Delete(T donor);
    }
}
