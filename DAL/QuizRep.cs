using Common.DAL;
using Common.Rsp.DTO;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class QuizRep : GenericRep<WebsiteKhoaHocOnline_V4Context, Quiz>
    {
        public bool AddQuiz(Quiz quiz)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                context.Quizzes.Add(quiz);
                context.SaveChanges();
                return true;
            }
        }

        public bool DeleteQuiz(Guid idQuiz)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var quiz = context.Quizzes.SingleOrDefault(q => q.IdQuiz== idQuiz);

                if (quiz != null)
                {
                    context.Quizzes.Remove(quiz);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public List<Quiz> GetQuizzesByIdLesson(Guid idLesson)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.Quizzes.Where(q => q.IdLesson == idLesson).ToList();
            }
        }

        public bool UpdateQuiz(Guid idQuiz, JsonPatchDocument patchDoc)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var quiz = context.Quizzes.SingleOrDefault(q => q.IdQuiz== idQuiz);
                if (quiz != null)
                {
                    patchDoc.ApplyTo(quiz);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
