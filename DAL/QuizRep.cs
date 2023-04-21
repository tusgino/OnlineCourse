using Common.DAL;
using DAL.Models;
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
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                if(context.Lessons.SingleOrDefault(l => l.IdLesson == quiz.IdLesson) == null) {
                    return false;
                }else
                {
                    context.Quizzes.Add(quiz);
                    context.SaveChanges();
                    return true;
                }
            }
        }
    }
}
