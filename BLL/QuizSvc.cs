using Common.BLL;
using Common.Req.Quiz;
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
    public class QuizSvc : GenericSvc<QuizRep, Quiz>
    {
        private QuizRep _quizRep = new QuizRep();

        public SingleRsp AddQuiz(QuizReq quizReq)
        {
            var rsp = new SingleRsp();
            if (_quizRep.AddQuiz(new Quiz
            {
                IdLesson = quizReq.IdLesson,
                Answer = quizReq.Answer,
                IdQuiz = Guid.NewGuid(),
                Option1 = quizReq.Option1,
                Option2 = quizReq.Option2,
                Option3 = quizReq.Option3,
                Option4 = quizReq.Option4,
                Question = quizReq.Question,
            }))
            {
                rsp.SetError("Not found ");
            }
            return rsp;
        }
    }
}
