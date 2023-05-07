using Common.BLL;
using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Rsp;
using Microsoft.AspNetCore.JsonPatch;
using Common.Req.Lesson;

namespace BLL
{
    public class LessonSvc : GenericSvc<LessonRep, Lesson>
    {
        private LessonRep _lessonRep = new LessonRep();

        public SingleRsp AddLesson(LessonReq lessonReq)
        {
            var rsp = new SingleRsp();

            if(!_lessonRep.AddLesson(new Lesson
            {
                Description= lessonReq.Description,
                Duration= lessonReq.Duration,
                IdChapter= lessonReq.IdChapter,
                IdLesson= Guid.NewGuid(),
                Index= lessonReq.Index,
                Title= lessonReq.Title,
                Video= lessonReq.Video,
            }))
            {
                rsp.SetError("Can not add this lesson");
            }

            return rsp;
        }

        public SingleRsp ChangeStatus(Guid idUser, Guid idLesson)
        {
            var rsp = new SingleRsp();

            var study = new Study
            {
                IdLesson = idLesson,
                IdUser = idUser,
                Status = 1,
            };

            if(!_lessonRep.ChangeStatus(new Study
            {
                IdLesson = idLesson,
                IdUser = idUser,
                Status = 1,
            }))
            {
                rsp.SetError("Can not change status");
            }

            return rsp;
        }

        public SingleRsp DeleteLesson(Guid idLesson)
        {
            var rsp = new SingleRsp();

            if (!_lessonRep.DeleteLesson(idLesson))
            {
                rsp.SetError("Can not update this Lesson");
            }

            return rsp;
        }

        public SingleRsp GetLessonByID(Guid idLesson)
        {
            var rsp = new SingleRsp();
            if((rsp.Data = _lessonRep.GetLessonByID(idLesson)) == null)
            {
                rsp.SetError($"Not found lesson which has {idLesson}");
            }

            return rsp;
        }

        public SingleRsp UpdateLesson(Guid idLesson, JsonPatchDocument patchDoc)
        {
            var rsp = new SingleRsp();

            if(!_lessonRep.UpdateLesson(idLesson, patchDoc))
            {
                rsp.SetError("Can not update this Lesson");
            }

            return rsp;
        }


    }
}
