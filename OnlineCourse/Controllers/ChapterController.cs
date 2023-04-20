using BLL;
using Common.Req.Chapter;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private ChapterSvc _chapterSvc = new ChapterSvc();

        [HttpPost("Add-Chapter")]
        public IActionResult AddChapter([FromBody]ChapterReq chapterReq)
        {
            var rsp = _chapterSvc.AddChapter(chapterReq);
            if (rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest(rsp.Message);
            }

        }

        [HttpDelete("Remove-Chapter-By-Id")]
        public IActionResult RemoveChapter([FromBody]Guid idChapter)
        {
            var rsp = _chapterSvc.RemoveChapter(idChapter);
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
