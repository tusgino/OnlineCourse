﻿using BLL;
using Common.Req.Lesson;
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

        [HttpPost("Upload-Lesson")]
        public IActionResult AddLesson([FromBody] LessonReq lessonReq)
        {
            var rsp = _lessonSvc.AddLesson(lessonReq);
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
