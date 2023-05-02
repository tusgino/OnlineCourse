using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/private/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategorySvc _categorySvc = new CategorySvc();
        [HttpPost("Add-category")]
        [Authorize(Roles = "Admin")]
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
        [HttpGet("Get-all-categories")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllCategories (string? _title_like, int page)
        {
            var res = _categorySvc.GetAllCategories(_title_like, page);
            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpPatch("Update-category-by-{ID_Category}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCategory(Guid ID_Category, [FromBody] JsonPatchDocument patchDoc)
        {
            var res = _categorySvc.UpdateCategory(ID_Category, patchDoc);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpDelete("Delete-categories")]
        [Authorize(Roles = "Admin")]
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
