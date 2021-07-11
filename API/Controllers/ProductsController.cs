
using API.Infrastructure.DataContext;
using API.Core.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Core.Interfaces;
using API.Core.Specifications;
using API.Dtos;
using System.Linq;
using AutoMapper;
using API.Helpers;

namespace API.Controllers
{
    
    public class ProductsController : BaseApiController
    {
        //private readonly StoreContext _context;
        //private readonly IProductRepository _productRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductBrand> productBrandRepository,
            IGenericRepository<ProductType> productTypeRepository,IMapper mapper)
        {
            _productRepository = productRepository;
            _productBrandRepository = productBrandRepository;
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
            //_context = context;
        }


        [HttpGet] //http verbs -> get post delete put
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery]ProductSpecParams productSpecParams)
        {
            var spec = new ProductsWithProductTypeAndBrandSpecification(productSpecParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productSpecParams);

            var totalItems = await _productRepository.CountAsync(spec);

            var products = await _productRepository.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);
            
            return Ok(new Pagination<ProductDto>(productSpecParams.PageIndex,productSpecParams.PageSize,totalItems,data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductsWithProductTypeAndBrandSpecification(id);
            //return await _productRepository.GetEntityWithSpec(spec);
            var product = await _productRepository.GetEntityWithSpec(spec);
            
            return _mapper.Map<Product, ProductDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepository.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepository.ListAllAsync());
        }

    }
}
