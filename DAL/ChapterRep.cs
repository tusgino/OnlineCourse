using Common.DAL;
using DAL.Models;
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
    }
}
