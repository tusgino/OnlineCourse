using BLL;
using Common.Req.Quiz;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly QuizSvc _quizSvc = new QuizSvc();
        [HttpGet("get-quizzes-by-IdLesson")]
        public IActionResult GetQuizzesByIdLesson(Guid idLesson)
        {
            var rsp = _quizSvc.GetQuizzesByIdLesson(idLesson);
            if(rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest(rsp.Message);
            }
        }

        [HttpPatch("update-quiz")]
        public IActionResult UpdateQuiz(Guid idQuiz, JsonPatchDocument patchDoc)
        {
            var rsp = _quizSvc.UpdateQuiz(idQuiz, patchDoc);
            if (rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest(rsp.Message);
            }
        }

        [HttpPost("add-quiz")]
        public IActionResult AddQuiz(QuizReq quizReq)
        {
            var rsp = _quizSvc.AddQuiz(quizReq);
            if (rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest(rsp.Message);
            }
        }

        [HttpDelete("delete-quiz")]
        public IActionResult DeleteQuiz(Guid idQuiz) {
            var rsp = _quizSvc.DeleteQuiz(idQuiz);
            if (rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest(rsp.Message);
            }
        }

    }
}
