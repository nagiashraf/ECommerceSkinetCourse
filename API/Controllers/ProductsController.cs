using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);

            return Ok(_mapper.Map<List<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);
            if (product is null)
            {
                return NotFound("Product not found");
            }
            
            return _mapper.Map<ProductToReturnDto>(product);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetTypes()
        {
            return Ok(await _unitOfWork.Repository<ProductType>().GetAllAsync());
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetBrands()
        {
            return Ok(await _unitOfWork.Repository<ProductBrand>().GetAllAsync());
        }
    }
}