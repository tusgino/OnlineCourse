using Common.DAL;
using Common.Rsp.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LessonRep : GenericRep<WebsiteKhoaHocOnline_V4Context, Lesson>
    {
        public bool AddLesson(Lesson lesson)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                if(context.Chapters.SingleOrDefault(c => c.IdChapter == lesson.IdChapter) != null)
                {
                    context.Lessons.Add(lesson);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
                

            }
        }

        public LessonDTO GetLessonByID(Guid idLesson)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var lesson = context.Lessons.SingleOrDefault(l => l.IdLesson== idLesson);
                List<QuizDTO> quizDTOs= new List<QuizDTO>();
                foreach(var quiz in context.Quizzes.Where(q => q.IdLessonNavigation.IdLesson == idLesson).ToList())
                {
                    quizDTOs.Add(new QuizDTO
                    {
                        IdLesson = quiz.IdLesson,
                        Answer = quiz.Answer,
                        IdQuiz = quiz.IdQuiz,
                        Option1 = quiz.Option1,
                        Option2 = quiz.Option2,
                        Option3 = quiz.Option3,
                        Option4 = quiz.Option4,
                        Question = quiz.Question,
                    });
                }
                return new LessonDTO
                {
                    Title = lesson.Title,
                    Desc = lesson.Description,
                    IdLesson = lesson.IdLesson,
                    Index = lesson.Index,
                    Quizzes = quizDTOs,
                    Video = lesson.Video,
                };
            }
        }
    }
}
