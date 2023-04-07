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
                var courses = GetAllCourseByName(_title_like); // Get all course whose name contains _title_like

                var data = courses.OrderBy(x => x.CourseName).Skip(offset).Take(limit).ToList(); // pagination

                foreach(var course in data)
                {
                    var user = context.Users.FirstOrDefault(x => x.IdUser== course.IdUser);

                    res.Add(new CourseComponent
                    {
                        Id = course.IdCourse,
                        Title = course.CourseName!,
                        Thumbnail = course.Thumbnail!,
                        AvatarUser = user!.Avatar,
                        NameUser = user!.Name,
                    });
                }
            }
            
            return res;
        }

        public List<CourseComponent> GetAllCourseByExpert(Guid id)
        {

            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var res = new List<CourseComponent>();

                foreach (var course in All.Where(course => course.IdUser == id).ToList())
                {
                    var user = context.Users.FirstOrDefault(x => x.IdUser == course.IdUser);

                    res.Add(new CourseComponent
                    {
                        Id = course.IdCourse,
                        Title = course.CourseName!,
                        Thumbnail = course.Thumbnail!,
                        AvatarUser = user!.Avatar,
                        NameUser = user!.Name,
                    });
                };

                return res;
            }
            
        }

        public IQueryable<Course> GetAllCourseByName(string? _title_like)
        {
            return All.Where(course => course.CourseName.Contains(_title_like==null?"":_title_like));
        }

        public Course GetCourseByID(Guid id)
        {
            return All.Where(course => course.IdCourse == id).FirstOrDefault()!;
        }
    }
}
