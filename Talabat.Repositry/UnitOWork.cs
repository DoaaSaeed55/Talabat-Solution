using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.RepostriesInterfaces;
using Talabat.Repositry.Data;
using Talabat.Repositry.Repositres;

namespace Talabat.Repositry
{
    public class UnitOWork : IUnitOfWork
    {
        private readonly StoreDbContext _storeDbContext;
        private Hashtable _repositries;
        public UnitOWork(StoreDbContext storeDbContext) 
        {
            _storeDbContext = storeDbContext;
            _repositries = new Hashtable();
        }
        public async Task<int> CompleteAsync()=>await _storeDbContext.SaveChangesAsync();


        public ValueTask DisposeAsync()=>_storeDbContext.DisposeAsync();

        //public IGenericRepositry<TEntity> Repositry<TEntity>() where TEntity : BaseEntity
        //{
        //    var type = typeof(TEntity).Name;

        //    if (!_repositries.ContainsKey(type)) // Corrected logic
        //    {
        //        var repository = new GenericRepositry<TEntity>(_storeDbContext);
        //        _repositries.Add(type, repository);
        //    }

        //    return (IGenericRepositry<TEntity>)_repositries[type]; // Ensure proper casting
        //}


        public IGenericRepositry<TEntity> Repositry<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (_repositries.ContainsKey(type))
            {
                var repositry = new GenericRepositry<TEntity>(_storeDbContext);
                _repositries.Add(type, repositry);
            }

            return _repositries[type] as IGenericRepositry<TEntity>;
        }
    }
}
