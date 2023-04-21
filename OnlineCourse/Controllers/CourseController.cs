using BLL;
using Common.Req.Course;
using Common.Req.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/public/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseSvc _courseSvc = new CourseSvc();

        [HttpGet("Get-all-course-for")]
        public IActionResult GetAllCourse( int _page, int _limit, string? title_like)
        {
             var coursesPaginationReq = new CoursesPaginationReq
            {
               Page= _page, Limit=_limit, Title_like=title_like,
            };
            var res = _courseSvc.GetAllCourse(coursesPaginationReq);
            return Ok(res);
        }

        [HttpGet("Get-all-course-by-Expert"), Authorize(Roles = "Expert")]
        public IActionResult GetAllCourseByExpert(Guid id) {
            var res = _courseSvc.GetAllCourseByExpert(id);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        /*[HttpGet("Get-course-by-IdCourse")]
        public IActionResult GetCourseByIDCourse(Guid id)
        {
            var res = _courseSvc.GetCourseByID(id);

            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }*/

        [HttpGet("Get-course-by-IdCourse-for-Student")]
        public IActionResult GetCourseByIDCourse(Guid id)
        {
            var res = _courseSvc.GetACourse(id); // res.Data: CourseDTO 
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-all-courses-by-filtering")]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetALLCoursesByFiltering(string? _title_like, string? _category_name, DateTime? _start_upload_day, DateTime? _end_upload_day, bool? _status_active, bool? _status_store, int page)
        {
            CoursesFilteringReq coursesFilteringReq = new CoursesFilteringReq
            {
                text = _title_like,
                category_name = _category_name,
                start_day = _start_upload_day,
                end_day = _end_upload_day,
                status_active = _status_active,
                status_store = _status_store,
            };
            CoursesPaginationReq coursesPaginationReq = new CoursesPaginationReq
            {
                Page = page,
                Limit = 10,
                Title_like = _title_like,
            };


            var res = _courseSvc.GetAllCoursesByFiltering(coursesFilteringReq, coursesPaginationReq);

            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("get-all-courses-by-categoryid")]
        public IActionResult GetAllCoursesByCategoryID(Guid _category_id)
        {
            var res = _courseSvc.GetCoursesByCategoryID(_category_id);
            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpPatch("{ID_Course}")]
        public IActionResult UpdateCourse(Guid ID_Course, [FromBody] JsonPatchDocument patchDoc)
        {
            var res = _courseSvc.UpdateCourse(ID_Course, patchDoc);
            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-all-courses-for-analytics")]
        public IActionResult GetAllCoursesForAnalytics (string? _title_like, int? _start_reg_user, int? _end_reg_user, int? _start_rate, int? _end_rate, int page)
        {
            CourseAnalyticsReq courseAnalyticsReq = new CourseAnalyticsReq
            {
                title_like = _title_like,
                start_reg_user = _start_reg_user,
                end_reg_user = _end_reg_user,
                start_rate = _start_rate,
                end_rate = _end_rate,
            };
            CoursesPaginationReq coursesPaginationReq = new CoursesPaginationReq
            {
                Page = page,
                Limit = 10,
                Title_like = _title_like,
            };

            var res = _courseSvc.GetAllCourseForAnalytics(courseAnalyticsReq, coursesPaginationReq);
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
