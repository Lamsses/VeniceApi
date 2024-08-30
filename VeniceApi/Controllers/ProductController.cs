using AutoMapper;
using EFDataAccessLibrary.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using VeniceApi.Interfaces;

namespace VeniceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ProductController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public static int GenerateRandomId()
        {
            Random random = new Random();
            return random.Next(100000, 1000000); // Generates a number between 100000 and 999999
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(int page = 1, int pageSize = 10)
        {
            var products = await _repositoryManager.Product.GetAll();
            var visibleProducts = products.Where(p => p.IsVisible); // Filter products with visible set to true
            var totalProducts = visibleProducts.Count();
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var paginatedProducts = visibleProducts.Skip((page - 1) * pageSize).Take(pageSize);

            var result = new
            {
                TotalPages = totalPages,
                Products = paginatedProducts
            };

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDtoUpdate>> Get(int id)
        {

            var product = await _repositoryManager.Product.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ProductDtoUpdate>(product));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDtoAdd>> Post(ProductDtoAdd productDto)
        {
            var productToDto = _mapper.Map<Product>(productDto);
            var product = await _repositoryManager.Product.Add(productToDto);
            
            product.RandomId = GenerateRandomId();
             await _repositoryManager.Save();
            return CreatedAtAction("Get", new { id = product.Id }, _mapper.Map<ProductDtoAdd>(product));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDtoUpdate>> Put(int id, [FromBody] ProductDtoUpdate productDto)
        {
            var product = await _repositoryManager.Product.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            _mapper.Map(productDto, product);
            await _repositoryManager.Product.Update(product);
            _repositoryManager.Save();

            return Ok(productDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _repositoryManager.Product.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            await _repositoryManager.Product.Delete(id);
            await _repositoryManager.Save();
            return Ok();
        }
    }
}
