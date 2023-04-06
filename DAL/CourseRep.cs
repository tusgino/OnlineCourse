using Common.DAL;
using Common.Req.Course;
using DAL.Models;

namespace DAL
{
    public class CourseRep : GenericRep<WebsiteKhoaHocOnline_V4Context, Course>
    {
        public List<CourseComponent> GetAllCourse(int offset, int limit, string _title_like)
        {
            var res = new List<CourseComponent>();
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var courses = GetAllCourseByName(_title_like);
                /*try
                {*/

                /*foreach (var course in courses)
                {
                    context.Entry(course).Reference(course => course.IdUserNavigation).Load();
                }*/
                /*}catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }*/



                var data = courses.OrderBy(x => x.CourseName).Skip(offset).Take(limit).ToList();

                foreach(var course in data)
                {
                    var user = context.Users.FirstOrDefault(x => x.IdUser== course.IdUser);

                    res.Add(new CourseComponent
                    {
                        Title = course.CourseName!,
                        Thumbnail = course.Thumbnail!,
                        AvatarUser = user!.Avatar,
                        NameUser = user!.Name,
                    });
                }
            }
            
            return res;
        }

        public IQueryable<Course> GetAllCourseByName(string _title_like)
        {
            return All.Where(course => course.CourseName.Contains(_title_like==null?"":_title_like));
        }
    }
}
