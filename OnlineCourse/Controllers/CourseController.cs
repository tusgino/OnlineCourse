using BLL;
using Common.Req.Course;
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
        public IActionResult GetAllCourse(int Page, int Limit, string Title_like = null )
        {
            var coursesPaginationReq = new CoursesPaginationReq
            {
               Page= Page, Limit=Limit, Title_like=Title_like
            };
            var res = _courseSvc.GetAllCourse(coursesPaginationReq);
            return Ok(res);
        }
    }
}
