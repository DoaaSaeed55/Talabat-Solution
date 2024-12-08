using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.RepostriesInterfaces;

namespace Talabat.Core
{
    public interface IUnitOfWork:IAsyncDisposable
    {


        IGenericRepositry<TEntity> Repositry<TEntity>() where TEntity:BaseEntity ;

        Task<int> CompleteAsync();
    }
}
