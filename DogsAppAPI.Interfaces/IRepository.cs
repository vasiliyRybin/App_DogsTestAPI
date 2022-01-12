using System.Linq;
using System.Threading.Tasks;

namespace DogsAppAPI.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> Get();
        Task<bool> CreateAsync(T item);
    }
}
