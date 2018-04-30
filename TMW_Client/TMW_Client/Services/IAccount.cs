using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TMW_Client.Services
{
    public interface IAccount<T>
    {
        Task<int> LoginAsync(T item);
        Task<bool> Register(T item);
    }
}
