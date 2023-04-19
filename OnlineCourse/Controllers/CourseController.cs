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



    }
}
