using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.RepostriesInterfaces;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductsSpec;

namespace Talabat.APIs.Controllers
{

    public class ProductController : BaseAPIController
    {
        private readonly IGenericRepositry<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepositry<ProductBrand> _brandRepo;
        private readonly IGenericRepositry<ProductCategory> _categoryRepo;

        public ProductController(IGenericRepositry<Product> productRepo,IMapper mapper,
            IGenericRepositry<ProductBrand> brandRepo,
            IGenericRepositry<ProductCategory> categoryRepo) 
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _brandRepo = brandRepo;
            _categoryRepo = categoryRepo;
        }
        [Authorize]

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts([FromQuery]ProductSpecParams productSpec) 
        {
            var spec = new ProductWithBrandAndCategorySpec(productSpec);
            var products=await _productRepo.GetAllWithSpecAsync(spec);
            var result=_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);

            var countSpec = new ProductWithCountSpec(productSpec);
            var count=_productRepo.GetCountAsync(countSpec);


            return Ok(new Pagination<ProductToReturnDTO>(productSpec.PageIndex,productSpec.PageSize,count:0,result));
        }

        [ProducesResponseType(typeof(ProductToReturnDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [HttpGet(template:"{id}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProductById(int id)
        {
            //var product = await _productRepo.GetAsync(id);
            var spec = new ProductWithBrandAndCategorySpec(id);
            var product = await _productRepo.GetWithSpecAsync(spec);
            if (product is null) 
                return NotFound(new ApiResponse(statusCode:404));

            var result = _mapper.Map<Product, ProductToReturnDTO>(product);

            return Ok(result);
        }

        [HttpGet(template: "brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandRepo.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet(template: "categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return Ok(categories);
        }

    }
}
