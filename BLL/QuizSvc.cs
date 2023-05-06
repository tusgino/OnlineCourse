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
using Common.Req.Quiz;

namespace BLL
{
    public class QuizSvc : GenericSvc<QuizRep, Quiz>
    {
        private readonly QuizRep _quizRep = new QuizRep();

        public SingleRsp AddQuiz(QuizReq quizReq)
        {
            var rsp = new SingleRsp();

            if(!_quizRep.AddQuiz(new Quiz
            {
                Answer= quizReq.Answer,
                IdLesson= quizReq.IdLesson,
                IdQuiz= Guid.NewGuid(),
                 Image= quizReq.Image,
                 Option1 = quizReq.Option1,
                 Option2 = quizReq.Option2,
                 Option3 = quizReq.Option3,
                 Option4 = quizReq.Option4,
                 Question= quizReq.Question,
            }))
            {
                rsp.SetError("Can not add this quiz");
            }

            return rsp;
        }

        public SingleRsp DeleteQuiz(Guid idQuiz)
        {
            var rsp = new SingleRsp();

            if (!_quizRep.DeleteQuiz(idQuiz))
            {
                rsp.SetError("Can not delete this quiz");
            }

            return rsp;
        }

        public SingleRsp GetQuizzesByIdLesson(Guid idLesson)
        {
            var rsp = new SingleRsp();

            rsp.Data = _quizRep.GetQuizzesByIdLesson(idLesson);

            return rsp;
        }

        public SingleRsp UpdateQuiz(Guid idQuiz, JsonPatchDocument patchDoc)
        {
            var rsp = new SingleRsp();

            if(!_quizRep.UpdateQuiz(idQuiz, patchDoc))
            {
                rsp.SetError("Can not update this quiz");
            }

            return rsp;
        }
    }
}
