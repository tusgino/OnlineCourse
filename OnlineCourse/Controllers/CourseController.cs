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
        public IActionResult GetAllCourse([FromQuery] CoursesPaginationReq coursesPaginationReq)
        {
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

        [Authorize]
        [HttpGet("Get-course-by-IdCourse-for-Student")]
        public IActionResult GetCourseByIDCourse(Guid idCourse, Guid idUser)
        {
            var res = _courseSvc.GetACourse(idCourse, idUser); // res.Data: CourseDTO 

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
        public IActionResult GetALLCoursesByFiltering([FromQuery] CoursesFilteringReq coursesFilteringReq)
        {
            var res = _courseSvc.GetAllCoursesByFiltering(coursesFilteringReq);

            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpGet("Get-all-courses-by-categoryid{_category_id}")]
        public IActionResult GetAllCoursesByCategoryID(Guid _category_id)
        {
            var res = _courseSvc.GetCoursesByCategoryID(_category_id);

            if (res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpPatch("Update-course-by-{ID_Course}")]
        public IActionResult UpdateCourse(Guid ID_Course, [FromBody] JsonPatchDocument patchDoc)
        {
            var res = _courseSvc.UpdateCourse(ID_Course, patchDoc);

            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpGet("Get-all-courses-for-analytics")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllCoursesForAnalytics([FromQuery] CourseAnalyticsReq courseAnalyticsReq)
        {
            var res = _courseSvc.GetAllCourseForAnalytics(courseAnalyticsReq);

            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpPost("Add-course")]
        [Authorize(Roles = "Expert")]
        public IActionResult AddCourse([FromBody] CourseReq courseReq)
        {
            var res = _courseSvc.AddCourse(courseReq);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpPost("change-status/{idCourse}")]
        public IActionResult ChangeStatus(Guid idCourse)
        {
            var res = _courseSvc.ChangeStatus(idCourse);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpGet("Get-average-feepercent")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAverageFeePercent()
        {
            var res = _courseSvc.GetAverageFeePercent();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-best-courses")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBestCourses()
        {
            var res = _courseSvc.GetBestCourses();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            else return BadRequest(res.Message);
        }
        [HttpGet("Get-num-of-uploaded-course-by-month")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetNumOfUploadedCourseByMonth(int year)
        {
            var res = _courseSvc.GetNumOfUploadedCourseByMonth(year);
            if(res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Overview-course")]
        [Authorize(Roles = "Admin")]
        public IActionResult OverviewCourse()
        {
            var res = _courseSvc.OverviewCourse();
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
