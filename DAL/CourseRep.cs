using Common.DAL;
using Common.Req.Course;
using DAL.Models;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Cryptography.Xml;

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
                var course = context.Courses.FirstOrDefault(course => course.IdCourse == _course_id);

                context.Courses.Remove(course!);
                context.SaveChanges();

            }
        }
        
        public bool UpdateCourse(Guid _course_id, JsonPatchDocument newCourse)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var course = context.Courses.FirstOrDefault(course => course.IdCourse == _course_id);
                
                if(course != null)
                {
                    newCourse.ApplyTo(course);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public int GetNumberOfRegisterdUser(Guid _course_id)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                int count_reg_user = 0;
                foreach (Purchase purchase in context.Purchases)
                {
                    if (purchase.IdCourse == _course_id)
                    {
                        count_reg_user++;
                    }
                }
                return count_reg_user;
            }
        }
        public int GetCourseRate(Guid _course_id)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                int rate_point = 0, count_turn = 0;
                foreach(Rate rate in context.Rates)
                {
                    if(rate.IdCourse == _course_id)
                    {
                        rate_point += rate.Rate1 ?? 0;
                        count_turn++;
                    }
                }
                return count_turn == 0 ? 0 : rate_point/count_turn;
            }
        }
        public List<object> GetAllCoursesForAnalytics(string? _title_like, int? _start_reg_user, int? _end_reg_user, int? _start_rate, int? _end_rate)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                List<Course> courses = context.Courses.Where(course => course.CourseName.Contains(_title_like == null? "" : _title_like)).ToList();


                // filter courses by number of registered users
                foreach(Course course in courses)
                {
                    if(GetNumberOfRegisterdUser(course.IdCourse) < _start_reg_user || GetNumberOfRegisterdUser(course.IdCourse) > _end_reg_user)
                    {
                        courses.Remove(course);
                    }
                }


                // filter courses by rate
                foreach(Course course in courses.ToList())
                {
                    if(GetCourseRate(course.IdCourse) < _start_rate || GetCourseRate(course.IdCourse) > _end_rate)
                    {
                        courses.Remove(course) ;
                    }
                }


                List<object> data = new List<object>();
                foreach(Course course in courses)
                {
                    data.Add(new
                    {
                        course_name = course.CourseName,
                        num_of_reg = GetNumberOfRegisterdUser(course.IdCourse),
                        rate = GetCourseRate(course.IdCourse)
                    });
                }

                return data;


            }
        }
    }
}
