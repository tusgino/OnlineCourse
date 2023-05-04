using BLL;
using Common.Req.Chapter;
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
        [HttpGet("AddChapter")]
        public IActionResult AddChapter(ChapterReq chapterReq) {
            var res = _chapterSvc.AddChapter(chapterReq);
            return Ok(res);
        }

        /*[HttpPatch("Update")]
        public IActionResult UpdateChapter(Guid idChapter, JsonPatchDocument patchDoc)
        {
            var res = _chapterSvc.Update(chapterRsp )
        }*/
    }
}
