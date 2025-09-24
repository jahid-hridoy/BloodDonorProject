using BloodDonorProject.Data;
using BloodDonorProject.Models;
using BloodDonorProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BloodDonorProject.Repositories.Implementations
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly BloodDonorDbContext _context;
        private readonly DbSet<T> dbSet;

        public Repository(BloodDonorDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }
        public IQueryable<T> GetAllAsync()
        {
            return dbSet;
        }
        public void Add(T donor)
        {
            dbSet.AddAsync(donor);
        }
        public void Update(T donor)
        {
            dbSet.Update(donor);
        }

        public void Delete(T donor)
        {
            dbSet.Remove(donor);
        }
    }
}
