using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategorySvc _categorySvc = new CategorySvc();
        [HttpPost("add-category")]
        public IActionResult AddCategory(string _category_name)
        {
            var res = _categorySvc.AddCategory(_category_name);
            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("get-all-categories")]
        public IActionResult GetAllCategories ()
        {
            var res = _categorySvc.GetAllCategories();
            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpDelete("delete-categories")]
        public IActionResult DeleteCategories(List<Guid> categoryIds)
        {
            var res = _categorySvc.DeleteCategories(categoryIds);

            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
    }
}
