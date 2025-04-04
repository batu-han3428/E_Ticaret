using E_Ticaret.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticaret.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetNestedCategoriesAsync();
            return Ok(categories);
        }
    }
}
