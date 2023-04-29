using BLL;
using Common.Req.Lesson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/private/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private LessonSvc _lessonSvc = new LessonSvc();
        [HttpGet("{idLesson}")]
        [Authorize]
        public IActionResult GetLessonByID(Guid idLesson)
        {
            var rsp = _lessonSvc.GetLessonByID(idLesson);
            if (rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("change-status")]
        [Authorize]
        public IActionResult ChangeStatus([FromBody] ChangeStatusReq changeStatusReq)
        {
            var rsp = _lessonSvc.ChangeStatus(changeStatusReq.IdUser, changeStatusReq.IdLesson);
            if (rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
