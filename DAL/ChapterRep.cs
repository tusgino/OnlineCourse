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
    public class ChapterRep : GenericRep<WebsiteKhoaHocOnline_V4Context, Chapter>
    {
        public bool AddChapter(Chapter chapter)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                context.Add(chapter);
                context.SaveChanges();
                return true;
            }
        }

        public bool DeleteChapter(Guid idChapter)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var chapter = context.Chapters.SingleOrDefault(c => c.IdChapter == idChapter);
                if (chapter == null)
                {
                    return false;
                }
                var lessons = context.Lessons.Where(c => c.IdChapter == idChapter);
                
                var quizzes = context.Quizzes.Where(q => q.IdLessonNavigation.IdChapter == idChapter);

                context.Quizzes.RemoveRange(quizzes);
                context.Lessons.RemoveRange(lessons);

                context.Chapters.Remove(chapter);

                var chapters = context.Chapters.Where(c => c.IdChapter == idChapter && c.Index > chapter.Index);

                foreach(var c in chapters)
                {
                    c.Index--;
                }

                context.SaveChanges();
                return true;
            }
        }

        public List<ChapterDTO> GetChaptersByIDCourse(Guid idCourse)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                List<ChapterDTO> chapters = new List<ChapterDTO>();
                List<Chapter> chapterTemp = context.Chapters.Where(ch => ch.IdCourse == idCourse).OrderBy(ch => ch.Index).ToList();
                foreach (var chapter in chapterTemp)
                {
                    List<LessonDTO> lessons = new List<LessonDTO>();
                    List<Lesson> lessonTemp = context.Lessons.Where(l => l.IdChapter == chapter.IdChapter).OrderBy(l => l.Index).ToList();
                    foreach (var lesson in lessonTemp) {
                        List<QuizDTO> quizzes = new List<QuizDTO>();
                        List<Quiz> quizzesTemp = context.Quizzes.Where(q => q.IdLesson == lesson.IdLesson).ToList();
                        foreach (var quizz in quizzesTemp)
                        {
                            quizzes.Add(new QuizDTO
                            {
                                IdLesson = lesson.IdLesson,
                                IdQuiz = quizz.IdQuiz,
                                Image = quizz.Image,
                                Answer = quizz.Answer,
                                Option1 = quizz.Option1,
                                Option2 = quizz.Option2,
                                Option3 = quizz.Option3,
                                Option4 = quizz.Option4,
                                Question = quizz.Question,
                            });
                        }
                        lessons.Add(new LessonDTO
                        {
                            Desc = lesson.Description,
                            Duration = lesson.Duration,
                            IdLesson = lesson.IdLesson,
                            Index = lesson.Index,
                            Quizzes = quizzes,
                            Title = lesson.Title,
                            Video = lesson.Video,
                        });
                    }
                    chapters.Add(new ChapterDTO
                    {
                        IdChapter = chapter.IdChapter,
                        Index = chapter.Index,
                        Name = chapter.Name,
                        Lessons = lessons,
                    });
                }
                return chapters;
            }
        }

        public bool UpdateChapter(Guid idChapter, JsonPatchDocument patchDoc)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var chapter = context.Chapters.SingleOrDefault(c => c.IdChapter == idChapter);

                if (chapter == null)
                {
                    return false;
                }

                patchDoc.ApplyTo(chapter);
                context.SaveChanges();
                return true;
            }
        }
    }
}
