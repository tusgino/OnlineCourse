using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/private/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserSvc _userSvc = new UserSvc();

        //<param "id"> ID user </param>
        [HttpGet("get-user-by-id")]
        public IActionResult GetUserByID(Guid? id) {
            var rsp = _userSvc.GetUserByID(id);

            if(rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest(rsp.Message);
            }
        }
    }
}
