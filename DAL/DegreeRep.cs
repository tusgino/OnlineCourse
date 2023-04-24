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
    public class DegreeRep : GenericRep<WebsiteKhoaHocOnline_V4Context, Degree>
    {
        public bool AddDegree(Degree degree)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                if(context.Users.SingleOrDefault(u => u.IdUser == degree.IdUser) == null) {
                    return false;
                }
                else
                {
                    context.Degrees.Add(degree);
                    context.SaveChanges();
                    return true;
                }
            }
        }

        public Degree GetDegreeByIdDegree(Guid idDegree)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.Degrees.SingleOrDefault(d => d.IdDegree == idDegree)!;
            }
        }

        public List<Degree> GetDegreesByIdUser(Guid idUser)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.Degrees.Where(d => d.IdUserNavigation!.IdUser == idUser).ToList();
            }
        }

        public bool UpdateDegree(Guid idDegree, JsonPatchDocument patchDoc)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var degree = context.Degrees.SingleOrDefault(d => d.IdDegree == idDegree);
                if (degree != null)
                {
                    patchDoc.ApplyTo(degree);
                    context.SaveChanges();
                    return true;
                }
                return false;

            }
        }
    }
}
