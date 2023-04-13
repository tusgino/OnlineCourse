using Common.DAL;
using Common.Req.Course;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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

        public object GetCourseByID(Guid id)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var course = context.Courses.Where(c => c.IdCourse== id).FirstOrDefault();
                if (course == null)
                {
                    return null;
                }
                var user = context.Users.Where(u => u.IdUser == course.IdUser).FirstOrDefault();

                return new
                {
                    Title = course.CourseName,
                    Discount = course.Discount,
                    Price = course.Price,   
                    Author = user?.Name,
                    VideoPreview = course.VideoPreview,
                };

            }
        }
        public List<Course> GetAllCourseByFiltering(string? _title_like, string? _category_name, DateTime? _start_upload_day, DateTime? _end_upload_day, int _status_active, int _status_store)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var categories = context.Categories.Where(category => category.Name.Contains(_category_name == null ? "" : _category_name)).Select(category => category.IdCategory).ToList();

                var data = GetAllCourseByName(_title_like).Where(course =>
                    course.DateOfUpload >= _start_upload_day && course.DateOfUpload <= _end_upload_day &&
                    categories.Contains(course.IdCategory ?? Guid.Empty) &&
                    (course.Status == _status_active ||
                    course.Status == _status_store)
                );

                return data.ToList();
            }
        }
        public List<Course> GetAllCourseByCategoryID(Guid _category_id)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                List<Course> courses = new List<Course>();
                foreach (Course course in context.Courses)
                {
                    if (course.IdCategory == _category_id)
                    {
                        courses.Add(course);
                    }
                }
                return courses;
            }
        }
        public void DeleteCourseByID(Guid _course_id)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var course = context.Courses.FirstOrDefault(course => course.IdCategory == _course_id);

                context.Courses.Remove(course!);
                context.SaveChanges();

            }
        }
    }
}
