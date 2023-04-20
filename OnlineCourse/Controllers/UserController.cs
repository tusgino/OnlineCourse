using BLL;
using Common.Req.User;
using Microsoft.AspNetCore.Authorization;
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


        [HttpGet("get-user")]
        [Authorize]
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
        [HttpGet("Get-all-users-by-filtering")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsersByFiltering(string? _title_like, DateTime? _start_date_create, DateTime? _end_date_create, bool? _is_student, bool? _is_expert, bool? _is_admin, bool? _status_active, bool? _status_banned, int page)
        {
            UserFilteringReq userFilteringReq = new UserFilteringReq
            {
                text = _title_like,
                start_date_create = _start_date_create,
                end_date_create = _end_date_create,
                is_student = _is_student,
                is_expert = _is_expert,
                is_admin = _is_admin,
                status_active = _status_active,
                status_banned = _status_banned,
            };

            var rsp = _userSvc.GetAllUsersByFiltering(userFilteringReq, page);

            if (rsp.Success)
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
            if (rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest(rsp.Message);
            }
        }

        [HttpGet("Get-All-Expert")]
        public IActionResult GetAllExpert()
        {
            var rsp = _userSvc.GetAllExpert();
            if (rsp.Success)
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
