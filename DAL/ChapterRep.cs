using Common.DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
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
                if(context.Courses.SingleOrDefault(c => c.IdCourse == chapter.IdCourse) == null)
                {
                    return false;
                }
                context.Chapters.Add(chapter);
                context.SaveChanges();
                return true;
            }
        }

        public bool RemoveChapter(Guid idChapter)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var chapter = context.Chapters.SingleOrDefault(c => c.IdChapter == idChapter);
                if(chapter != null)
                {
                    var lessons = context.Lessons.Where(l => l.IdChapterNavigation.IdChapter == idChapter).ToList();
                    
                    foreach(var lesson in lessons)
                    {
                        var quizzes = context.Quizzes.Where(q => q.IdLessonNavigation.IdLesson == lesson.IdLesson).ToList();
                        context.Quizzes.RemoveRange(quizzes);
                    }
                    context.Lessons.RemoveRange(lessons);
                    
                    context.Chapters.Remove(chapter);

                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
