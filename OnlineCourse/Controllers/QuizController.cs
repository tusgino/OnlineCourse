using BLL;
using Common.Req.Quiz;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private QuizSvc _quizSvc = new QuizSvc();

        [HttpGet("Add-Quiz")]
        public IActionResult AddQuiz([FromBody] QuizReq quizReq)
        {
            var res = _quizSvc.AddQuiz(quizReq);
            if(res.Success)
                return Ok(res);
            else
            {
                return BadRequest(res.Message);
            }

        }
    }
}
