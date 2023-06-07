using BLL;
using Common.Req.Course;
using Common.Req.User;
using Common.Rsp.DTO;
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

            if (rsp.Success)
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
        public IActionResult GetAllUsersByFiltering([FromQuery] UserFilteringReq userFilteringReq)
        {
            var rsp = _userSvc.GetAllUsersByFiltering(userFilteringReq);

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
        [Authorize(Roles = "Admin")]
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

        [HttpGet("Get-all-students-for-analytics")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetStudentsForAnalytics([FromQuery] StudentAnalyticsReq studentAnalyticsReq)
        {
            var res = _userSvc.GetAllStudentForAnalytics(studentAnalyticsReq);

            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-all-experts-for-analytics")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllExpertsForAnalytics([FromQuery] ExpertAnalyticsReq expertAnalyticsReq)
        {
            var res = _userSvc.GetAllExpertsForAnalytics(expertAnalyticsReq);

            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-expert-revenue-by-id={IdExpert}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetExpertRevenueByID(Guid IdExpert)
        {
            var res = _userSvc.GetExpertRevenueByID(IdExpert);
            if(res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-all-users-by-type")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsersByType()
        {
            var res = _userSvc.GetAllUsersByType();

            if (res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-new-users")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetNewUsers()
        {
            var res = _userSvc.GetNewUsers();
            if(res.Success)
            {
                return Ok(res.Data);  
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-all-expert-requests")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllExpertRequests([FromQuery] ExpertRegisterReq expertRegisterReq)
        {
            var res = _userSvc.GetAllExpertRequests(expertRegisterReq);

            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-best-students")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBestStudents()
        {
            var res = _userSvc.GetBestStudents();

            if(res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-best-experts")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBestExperts()
        {
            var res = _userSvc.GetBestExperts();

            if (res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpPost("Send-mail")]
        public IActionResult SendMail(EmailDTO emailDTO)
        {
            var res = _userSvc.SendMail(emailDTO);

            if (res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
    }
}
