using Common.DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DegreeRep : GenericRep<WebsiteKhoaHocOnline_V4Context, Degree>
    {
        public List<Degree> GetDegreesByIdUser(Guid idUser)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.Degrees.Where(d => d.IdUserNavigation!.IdUser == idUser).ToList();
            }
        }
    }
}
