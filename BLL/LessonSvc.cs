using Common.BLL;
using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Rsp;
using Common.Req.Lesson;

namespace BLL
{
    public class LessonSvc : GenericSvc<LessonRep, Lesson>
    {
        private LessonRep _lessonRep = new LessonRep();

        public SingleRsp AddLesson(LessonReq lessonReq)
        {
            var rsp = new SingleRsp();

            if (_lessonRep.AddLesson(new Lesson
            {
                Description = lessonReq.Description,
                Duration = lessonReq.Duration,
                IdChapter = lessonReq.IdChapter,
                IdLesson = Guid.NewGuid(),
                Index = lessonReq.Index,
                Title = lessonReq.Title,
                Video = lessonReq.Video
            }))
            {
                rsp.SetError("Can not add lesson because not exist Chapter");
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
    }
}
