using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repositry.Data
{
    public static class StoreDbContextSeed
    {

        public static async Task SeedAsync(StoreDbContext _context)
        {
            //Brand
            if (_context.ProductBrands.Count()==0) 
            { 
            var BrandData = File.ReadAllText(path: "../Talabat.Repositry/Data/DataSeed/brands.json");
            var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);

            if (Brands?.Count() > 0)
            {
                foreach (var Brand in Brands)
                {
                    _context.Set<ProductBrand>().Add(Brand);

                }

                await _context.SaveChangesAsync();
            }
        }

            //Category
            if (_context.ProductCategories.Count() == 0)
            {
                var CategoryData = File.ReadAllText(path: "../Talabat.Repositry/Data/DataSeed/categories.json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);

                if (Categories?.Count() > 0)
                {
                    foreach (var Category in Categories)
                    {
                        _context.Set<ProductCategory>().Add(Category);

                    }

                    await _context.SaveChangesAsync();
                }
            }
            //product
            if (_context.Products.Count() == 0)
            {
                var productData = File.ReadAllText(path: "../Talabat.Repositry/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products?.Count() > 0)
                {
                    foreach (var product in products)
                    {
                        _context.Set<Product>().Add(product);

                    }

                    await _context.SaveChangesAsync();
                }
            }
        }

    }
}
