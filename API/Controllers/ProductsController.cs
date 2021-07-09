
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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
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
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts()
        {
            var spec = new ProductsWithProductTypeAndBrandSpecification();
            var data = await _productRepository.ListAsync(spec);
            //return Ok(data);
            //return data.Select(pro => new ProductDto
            //{
            //    Id = pro.Id,
            //    Name = pro.Name,
            //    Description = pro.Description,
            //    PictureUrl = pro.PictureUrl,
            //    Price = pro.Price,
            //    ProductBrand = pro.ProductBrand != null ? pro.ProductBrand.Name : string.Empty,
            //    ProductType = pro.ProductType != null ? pro.ProductType.Name : string.Empty
            //}).ToList();
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductsWithProductTypeAndBrandSpecification(id);
            //return await _productRepository.GetEntityWithSpec(spec);
            var product = await _productRepository.GetEntityWithSpec(spec);
            //return new ProductDto
            //{
            //    Id=product.Id,
            //    Name=product.Name,
            //    Description=product.Description,
            //    PictureUrl=product.PictureUrl,
            //    Price=product.Price,
            //    ProductBrand=product.ProductBrand!=null? product.ProductBrand.Name :string.Empty,
            //    ProductType=product.ProductType!=null? product.ProductType.Name : string.Empty
            //};
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
