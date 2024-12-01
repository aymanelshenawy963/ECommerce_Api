using E_Commerce507Api.Models;
using E_Commerce507Api.Repository.IRepository;
using E_Commerce507Api.Utilty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce507Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles=SD.AdminRole)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var categories = categoryRepository.GetAll();
            return Ok(categories);
        }
        [HttpPost("Create")]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Add(category);
                categoryRepository.Commit();
                return Ok();
            }
            
            return BadRequest(category);
        }

        [HttpGet("Details")]
        public IActionResult Details(int categoryId)
        {
            var category = categoryRepository.GetOne(expression:e=>e.Id == categoryId);
            if (category != null)
            {
                return Ok(category);
            }

            return NotFound();
        }

        [HttpPut("Edit")]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Edit(category);
                categoryRepository.Commit();
                return Created($"{Request.Scheme}://{Request.Host}/api/Category/Details?categoryId={category.Id}",category);
            }

            return BadRequest(category);
        }

        [HttpDelete]
        public IActionResult Delete(int categoryId)
        {
            var category = categoryRepository.GetOne(expression: e => e.Id == categoryId);
            if (category != null)
            {
                categoryRepository.Delete(category);
                categoryRepository.Commit();
                return Ok(category);
            }

            return NotFound();
        }
    }
}
