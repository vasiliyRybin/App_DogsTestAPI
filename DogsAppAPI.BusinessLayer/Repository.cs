using DogsAppAPI.DB.Models;
using DogsAppAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<T> GetFirstOrDefault(Func<T, bool> predicate)
        {
            return await Task.Run(() => _context.Set<T>().FirstOrDefault(predicate));
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<bool> Create(T item)
        {
            _context.Set<T>().Add(item);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }
    }
}
