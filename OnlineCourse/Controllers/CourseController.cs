using BLL;
using Common.Req.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [Authorize]
        [HttpGet("Get-all-course-by-Id")] // id can Id of Expert or Student
        public IActionResult GetAllCourseByIdUser(Guid id) {
            var res = _courseSvc.GetAllCourseByIdUser(id);
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
        [Authorize(Roles = "Admin")]
        public IActionResult GetALLCoursesByFiltering(string? _title_like, string? _category_name, DateTime? _start_upload_day, DateTime? _end_upload_day, int _status_active, int _status_store, int page)
        {
            CoursesFilteringReq coursesFilteringReq = new CoursesFilteringReq
            {
                text = _title_like,
                category_name = _category_name,
                start_day = _start_upload_day,
                end_day = _end_upload_day,
                status_active = _status_active,
                status_store = _status_store
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


    }
}
