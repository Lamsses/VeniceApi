using AutoMapper;
using EFDataAccessLibrary.Dto;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using VeniceApi.Interfaces;
using VeniceApi.Repository;

namespace VeniceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
        {
            var product = await _categoryRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(product));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var product = await _categoryRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDtoModfiy>> Post(CategoryDtoModfiy categoryDto)
        {
            var category = await _categoryRepository.Add(_mapper.Map<Category>(categoryDto));
            return CreatedAtAction("Get", new { id = category.Id }, _mapper.Map<CategoryDtoModfiy>(category));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDtoModfiy>> Put(int id, [FromBody] CategoryDtoModfiy categoryDto)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            _mapper.Map(categoryDto, category);
            await _categoryRepository.Update(category);

            return Ok(categoryDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            await _categoryRepository.Delete(id);
            return Ok();
        }




    }
}
