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
        public bool IsLastOfChapter(Guid _lesson_id)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var lesson = context.Lessons.FirstOrDefault(lesson => lesson.IdLesson == _lesson_id);
                var chapter = context.Chapters.FirstOrDefault(chapter => chapter.IdChapter == lesson.IdLesson);

                if (lesson.Index == chapter.Lessons.Count - 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsLastOfCourse(Guid _lesson_id)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var lesson = context.Lessons.FirstOrDefault(lesson => lesson.IdLesson == _lesson_id);
                var chapter = context.Chapters.FirstOrDefault(chapter => chapter.IdChapter == lesson.IdLesson);
                var course = context.Courses.FirstOrDefault(course => course.IdCourse == chapter.IdCourse);

                if (lesson.Index == chapter.Lessons.Count - 1 && chapter.Index == course.Chapters.Count -1)
                {
                    return true;
                }
                else
                {
                    return false;
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
