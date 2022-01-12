using DogsAppAPI.DB.Models;
using DogsAppAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly DogsDbContext _context;

        public Repository(DogsDbContext dogsDBContext)
        {
            _context = dogsDBContext ?? throw new ArgumentNullException(nameof(dogsDBContext));
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<bool> CreateAsync(T item)
        {
            _context.Set<T>().Add(item);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }
    }
}
