using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductsSpec
{
    public class ProductWithCountSpec:BaseSpecification<Product>
    {
        public ProductWithCountSpec(ProductSpecParams productSpec)
            : base(p =>
                 
                 (!productSpec.BrandId.HasValue || p.BrandId == productSpec.BrandId) &&
                 (!productSpec.CategoryId.HasValue || p.CategoryId == productSpec.CategoryId)
            )
        { 
        
        }
    }
}
