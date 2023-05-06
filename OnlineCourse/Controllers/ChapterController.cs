using BLL;
using Common.Req.Chapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private ChapterSvc _chapterSvc = new ChapterSvc();
        [HttpPost("AddChapter")]
        public IActionResult AddChapter(ChapterReq chapterReq) {
            var res = _chapterSvc.AddChapter(chapterReq);
            return Ok(res);
        }

        [HttpPatch("Update/{idChapter}")]
        public IActionResult UpdateChapter(Guid idChapter, JsonPatchDocument patchDoc)
        {
            var res = _chapterSvc.UpdateChapter(idChapter, patchDoc);

            if(res.Success) 
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        /*        [HttpDelete("{idChapter}")]
                public IActionResult DeleteChapter(Guid idChapter)
                {
                    var res = _chapterSvc.DeleteChapter(idChapter);
                }*/

        [HttpGet("Get-chapters-by-IdCourse/{idCourse}")]
        [Authorize]
        public IActionResult GetChaptersByIDCourse(Guid idCourse)
        {
            var res = _chapterSvc.GetChaptersByIDCourse(idCourse);

            return Ok(res);
        }
    }
}
