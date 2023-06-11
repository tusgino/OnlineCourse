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

            var quiz = new Quiz
            {
                IdQuiz = Guid.NewGuid(),
                IdLesson = quizReq.IdLesson,
                Question = quizReq.Question,
                Image = quizReq.Image,
                Option1 = quizReq.Option1,
                Option2 = quizReq.Option2,
                Option3 = quizReq.Option3,
                Option4 = quizReq.Option4,
                Answer = quizReq.Answer,
            };  

            if(_quizRep.AddQuiz(quiz))
            {
                rsp.Data = quiz;
            }
            else
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
