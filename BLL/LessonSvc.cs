﻿using Common.BLL;
using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Rsp;

namespace BLL
{
    public class LessonSvc : GenericSvc<LessonRep, Lesson>
    {
        private LessonRep _lessonRep = new LessonRep();

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
