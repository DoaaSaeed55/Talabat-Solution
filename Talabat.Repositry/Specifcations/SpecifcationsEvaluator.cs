﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repositry.Specifcations
{
    public class SpecifcationsEvaluator<TEntity> where TEntity:BaseEntity   
    {

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecifications<TEntity> spec)
        {
            var query = inputQuery;
            if(spec.Criteira is not null)
            {
                query = query.Where(spec.Criteira);
            }
            if (spec.IsPaginationEnabled)
            {
                query=query.Skip(spec.Skip).Take(spec.Take);
            }

            if(spec.OrderBy is not null)
                query= query.OrderBy(spec.OrderBy);

            if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);

            query =spec.Includes.Aggregate(query, (currentQuery, includeExpresion) => currentQuery.Include(includeExpresion));


            return query;
        }




    }
}
