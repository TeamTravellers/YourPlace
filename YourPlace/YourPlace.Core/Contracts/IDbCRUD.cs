using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Core.Contracts
{
    public interface IDbCRUD<T,K> where T : class
    {
        Task CreateAsync(T item);
        Task<T> ReadAsync(K key, bool useNavigationalProperties = false, bool isReadOnly = true);
        Task<List<T>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true);
        Task UpdateAsync(T item);
        Task DeleteAsync(K key);
    }
}
