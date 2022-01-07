using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsAppAPI.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetFirstOrDefault(Func<T, bool> predicate);
        Task<bool> Create(T item);
    }
}
