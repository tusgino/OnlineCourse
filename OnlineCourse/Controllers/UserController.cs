using BLL;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPatch("{ID_User}")]
        public IActionResult UpdateUser(Guid ID_User, [FromBody] JsonPatchDocument patchDoc)
        {
            var rsp = _userSvc.UpdateUser(ID_User, patchDoc);
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
