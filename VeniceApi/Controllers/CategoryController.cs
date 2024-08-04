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
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper, IRepositoryManager repositoryManager)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
        {
            var product = await _repositoryManager.Category.GetAll();
            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(product));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var product = await _repositoryManager.Category.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDtoModfiy>> Post(CategoryDtoModfiy categoryDto)
        {
            var category = await _repositoryManager.Category.Add(_mapper.Map<Category>(categoryDto));
            _repositoryManager.Save();
            return CreatedAtAction("Get", new { id = category.Id }, _mapper.Map<CategoryDtoModfiy>(category));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDtoModfiy>> Put(int id, [FromBody] CategoryDtoModfiy categoryDto)
        {
            var category = await _repositoryManager.Category.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            _mapper.Map(categoryDto, category);
            await _repositoryManager.Category.Update(category);
            _repositoryManager.Save();

            return Ok(categoryDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _repositoryManager.Category.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            await _repositoryManager.Category.Delete(id);
            _repositoryManager.Save();
            return Ok();
        }




    }
}
