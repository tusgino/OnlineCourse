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
        public IActionResult GetAllCourse(int Page, int Limit, string? Title_like)
        {
            var coursesPaginationReq = new CoursesPaginationReq
            {
               Page= Page, Limit=Limit, Title_like=Title_like,
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

        [HttpGet("Get-course-by-IdCourse")]
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
        }


    }
}
