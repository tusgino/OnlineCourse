﻿using Common.BLL;
using Common.Req.Chapter;
using Common.Rsp;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ChapterSvc : GenericSvc<ChapterRep, Chapter>
    {
        private readonly ChapterRep _chapterRep = new ChapterRep();

        public SingleRsp AddChapter(ChapterReq chapterReq)
        {
            var rsp = new SingleRsp();

            if(_chapterRep.AddChapter(new Chapter
            {
                IdChapter = Guid.NewGuid(),
                IdCourse = chapterReq.IdCourse,
                Index= chapterReq.Index,
                Name= chapterReq.Name,
            }))
            {
                rsp.SetError("Can not add this chapter");
            }

            return rsp;
        }

        public SingleRsp DeleteChapter(Guid idChapter)
        {
            var rsp = new SingleRsp();
            if (!_chapterRep.DeleteChapter(idChapter))
            {
                rsp.SetError("Can not update this chapter");
            }
            return rsp; 
        }

        public SingleRsp UpdateChapter(Guid idChapter, JsonPatchDocument patchDoc)
        {
            var rsp = new SingleRsp();

            if(!_chapterRep.UpdateChapter(idChapter, patchDoc))
            {
                rsp.SetError("Can not update this chapter");
            }

            return rsp;
        }
    }
}
