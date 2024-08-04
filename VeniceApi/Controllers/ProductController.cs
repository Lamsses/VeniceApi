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
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository , IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
            {
                var product = await  _productRepository.GetAll();
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(product));
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<ProductDto>> Get(int id)
            {
                var product = await _productRepository.GetById(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<ProductDto>(product));    
            }

            [HttpPost]
            public async Task<ActionResult<ProductDtoAdd>> Post(ProductDtoAdd productDto)
            {
                var product = await _productRepository.Add(_mapper.Map<Product>(productDto));
                return CreatedAtAction("Get", new { id = product.Id }, _mapper.Map<ProductDtoAdd>(product));
            }
            [HttpPut("{id}")]
            public async Task<ActionResult<ProductDtoUpdate>> Put(int id, [FromBody] ProductDtoUpdate productDto)
            {
                var product = await _productRepository.GetById(id);
                if (product == null)
                {
                    return NotFound();
                }

                _mapper.Map(productDto, product);
                await _productRepository.Update(product);

                return Ok(productDto);
            }
            [HttpDelete("{id}")]
            public async Task<ActionResult> Delete(int id)
            {
                var product = await _productRepository.GetById(id);
                if (product == null)
                {
                    return NotFound();
                }
                await _productRepository.Delete(id);
                return Ok();
            }
    }
}
