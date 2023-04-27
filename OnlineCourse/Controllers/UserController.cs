using BLL;
using Common.Req.Course;
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
        //[Authorize(Roles = "Admin")]
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

        [HttpGet("Get-all-students-for-analytics")]
        public IActionResult GetStudentsForAnalytics(string? _student_name_like, int? _start_purchase_course, int? _end_purchase_course, int? _start_finish_course, int? _end_finish_course, int page)
        {
            StudentAnalyticsReq studentAnalyticsReq = new StudentAnalyticsReq
            {
                student_name_like = _student_name_like,
                start_purchase_course = _start_purchase_course,
                end_purchase_course = _end_purchase_course,
                start_finish_course = _start_finish_course,
                end_finish_course = _end_finish_course,
            };

            CoursesPaginationReq coursesPaginationReq = new CoursesPaginationReq
            {
                Limit = 10,
                Page = page,
                Title_like = _student_name_like,
            };


            var res = _userSvc.GetAllStudentForAnalytics(studentAnalyticsReq, coursesPaginationReq);
            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-all-experts-for-analytics")]
        public IActionResult GetAllExpertsForAnalytics(string? _expert_name_like, int? _start_upload_course, int? _end_upload_course, long? _start_revenue, long? _end_revenue, int page)
        {
            ExpertAnalyticsReq expertAnalyticsReq = new ExpertAnalyticsReq
            {
                expert_name = _expert_name_like,
                start_upload_course = _start_upload_course,
                end_upload_course = _end_upload_course,
                start_revenue = _start_revenue,
                end_revenue = _end_revenue,
            };
            CoursesPaginationReq coursesPaginationReq = new CoursesPaginationReq
            {
                Page = page,
                Limit = 10,
                Title_like = ""
            };

            var res = _userSvc.GetAllExpertsForAnalytics(expertAnalyticsReq, coursesPaginationReq);
            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpGet("Get-system-revenue")]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetSystemRevenue ()
        {
            var res = _userSvc.GetSystemRevenue();
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
