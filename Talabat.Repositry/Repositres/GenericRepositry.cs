using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.RepostriesInterfaces;
using Talabat.Core.Specifications;
using Talabat.Repositry.Data;
using Talabat.Repositry.Specifcations;

namespace Talabat.Repositry.Repositres
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : BaseEntity
    {
        private readonly StoreDbContext _context;

        public GenericRepositry(StoreDbContext context) 
        {
            _context = context;
        }

       
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>) await _context.Products.Include(p=>p.Brand).Include(p=>p.Category).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
           return await SpecifcationsEvaluator<T>.GetQuery(_context.Set<T>(),spec).ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {
                return await _context.Products.Where(p=>p.Id==id).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as T;
            }
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
        {

            return  SpecifcationsEvaluator<T>.GetQuery(_context.Set<T>(), spec).FirstOrDefaultAsync();

        }
        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecifcationsEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }


        public async Task AddAsync(T item)
        {
            
           await _context.Set<T>().AddAsync(item);
        }
        public void Update(T item)
        {
             _context.Set<T>().Update(item);
        }
        public void Delete(T item)
        {
             _context.Set<T>().Remove(item);
        }

       
    }
}
