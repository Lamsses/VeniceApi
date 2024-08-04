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

            [HttpGet]
            public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
            {
                var product = await  _repositoryManager.Product.GetAll();
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(product));
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<ProductDto>> Get(int id)
            {
                var product = await _repositoryManager.Product.GetById(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<ProductDto>(product));    
            }

            [HttpPost]
            public async Task<ActionResult<ProductDtoAdd>> Post(ProductDtoAdd productDto)
            {
                var product = await _repositoryManager.Product.Add(_mapper.Map<Product>(productDto));
                _repositoryManager.Save();
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
                _repositoryManager.Save();
                return Ok();
            }
    }
}
