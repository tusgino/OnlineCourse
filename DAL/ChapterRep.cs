using Common.DAL;
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
