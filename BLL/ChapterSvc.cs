using Common.BLL;
using Common.Req.Chapter;
using Common.Rsp;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ChapterSvc : GenericSvc<ChapterRep, Chapter>
    {
        private ChapterRep _chapterRep = new ChapterRep();

        public SingleRsp AddChapter(ChapterReq chapterReq)
        {
            var rsp = new SingleRsp();
            Chapter chapter = new Chapter
            {
                IdChapter = Guid.NewGuid(),
                IdCourse = chapterReq.IdCourse,
                Index = chapterReq.Index,
                Name = chapterReq.Name,
            };

            if (!_chapterRep.AddChapter(chapter))
            {
                rsp.SetError($"{chapter.IdCourse} can not found");
            }
            return rsp;
        }

        public SingleRsp RemoveChapter(Guid idChapter)
        {
            var rsp = new SingleRsp();
            if(!_chapterRep.RemoveChapter(idChapter))
            {
                rsp.SetError($"Can not remove chapter which has id = {idChapter}");
            }
            return rsp;
        }
    }
}
