using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Services.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IBaseService : IDisposable
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}
