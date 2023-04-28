using Common.DAL;
using Common.Rsp.DTO;
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
        public CourseDTO GetACourse(Guid idCourse, Guid idUser)
        {
            var courses = new List<CourseDTO>();
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                Course course;
                if((course = context.Courses.SingleOrDefault(c => c.IdCourse == idCourse)!) != null)
                {
                    List<Chapter> chapters = context.Chapters.Where(ch => ch.IdCourseNavigation!.IdCourse == idCourse).OrderBy(ch => ch.Index).ToList();
                    List<ChapterDTO> chapterDTOs = new List<ChapterDTO>();
                    foreach(var chapter in chapters) {
                        List<LessonDTO> lessonDTOs= new List<LessonDTO>();
                        foreach(var lesson in context.Lessons.Where(l => l.IdChapterNavigation.IdChapter == chapter.IdChapter).OrderBy(l => l.Index).ToList())
                        {
                            var study = context.Studies.SingleOrDefault(st => st.IdLesson== lesson.IdLesson && st.IdUser == idUser)!;
                            lessonDTOs.Add(new LessonDTO
                            {
                                IdLesson = lesson.IdLesson,
                                Title = lesson.Title,
                                Index= lesson.Index,
                                Video=lesson.Video, 
                                Status = study==null? 0 : study.Status,
                            });
                        }
                        chapterDTOs.Add(new ChapterDTO
                        {
                            IdChapter = chapter.IdChapter,
                            Index = chapter.Index,
                            Lessons = lessonDTOs,
                            Name = chapter.Name,
                        });
                    }
                    
                    return new CourseDTO
                    {
                        IdCourse = idCourse,
                        CourseName = course.CourseName,
                        Chapters = chapterDTOs
                    };
                }
            }
            return null!;
        }

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
        public List<CourseModel_Admin> GetAllCourseByFiltering(string? _title_like, string? _category_name, DateTime? _start_upload_day, DateTime? _end_upload_day, bool? _status_active, bool? _status_store)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var categories = context.Categories.Where(category => category.Name.Contains(_category_name == null ? "" : _category_name)).Select(category => category.IdCategory).ToList();
                List<int> course_status = new List<int>();
                if (_status_active == true) course_status.Add(1);
                if (_status_store == true) course_status.Add(0);

                List<Course> data = context.Courses.Where(course => 
                   course.CourseName.Contains(_title_like == null ? "" : _title_like) &&
                   course.DateOfUpload >= _start_upload_day && course.DateOfUpload <= _end_upload_day &&
                   (categories.Contains(course.IdCategory ?? Guid.Empty) || course.IdCategory == null)&&
                   course_status.Contains(course.Status ?? -1)
                 ).ToList();

                List<CourseModel_Admin> res = new List<CourseModel_Admin>();
                foreach(Course course in data)
                {
                    if(course.IdCategory != null) 
                        context.Entry(course).Reference(course => course.IdCategoryNavigation).Load();
                    context.Entry(course).Reference(course => course.IdUserNavigation).Load();

                    res.Add(new CourseModel_Admin
                    {
                        ID = course.IdCourse,
                        Name = course.CourseName,
                        Category = course.IdCategory == null ? null : course.IdCategoryNavigation!.Name,
                        UploadUser = course.IdUserNavigation!.Name,
                        DateUpload = (course.DateOfUpload == null) ? "" : course.DateOfUpload.Value.ToShortDateString(),
                        Price = course.Price.ToString(),
                        Discount = course.Discount.ToString(),
                        FeePercent = course.FeePercent.ToString(),
                        Status = course.Status == 1 ? "Hoạt động" : (course.Status == 0 ? "Lưu trữ" : "Cấm vĩnh viễn"),
                    });
                }


                return res;
            }
        }
        public List<Course> GetAllCourseByCategoryID(Guid _category_id)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.Courses.Where(course => course.IdCategory == _category_id).ToList();
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
                Console.WriteLine(course);
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
                foreach(Course course in courses.ToList())
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

        public List<CourseComponent> GetAllCourseByIdUser(Guid id)
        {
            List<CourseComponent> courses = new List<CourseComponent>();

            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var expert = context.Users.SingleOrDefault(u => u.IdUser == id && u.IdTypeOfUserNavigation.IdTypeOfUser == 1);
                if (expert != null)
                {
                    foreach(var course in context.Courses.Where(c => c.IdUser == id).ToList())
                    {
                        courses.Add(new CourseComponent
                        {
                            AvatarUser = expert.Avatar,
                            Id = course.IdCourse,
                            Thumbnail = course.Thumbnail,
                            NameUser = null,
                            Title = course.CourseName,
                        });
                    }
                }
                else
                {
                    var purchases = context.Purchases.Where(p => p.IdUser == id).ToList();
                    foreach (var purchase in purchases)
                    {
                        var course = context.Courses.SingleOrDefault(c => c.IdCourse == purchase.IdCourse);
                        context.Entry(course).Reference(c => c.IdUserNavigation).Load();
                        courses.Add(new CourseComponent
                        {
                            AvatarUser = course.IdUserNavigation.Avatar,
                            Id = course.IdCourse,
                            NameUser = course.IdUserNavigation.Name,
                            Thumbnail= course.Thumbnail,
                            Title= course.CourseName,
                        });
                    }
                }
            }

            return courses;
        }

        public bool AddCourse(Course course)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                if(context.Categories.SingleOrDefault(c => c.IdCategory == course.IdCategory) == null) {
                    return false;
                }
                else
                {
                    context.Courses.Add(course);
                    context.SaveChanges();
                    return true;
                }
            }
        }

        /*public bool RemoveCourse(Guid idCourse)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                if(context.Categories.SingleOrDefault(c => c.IdCategory == ))
            }
        }*/

        public double? GetAverageFeePercent()
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.Courses.Average(c => c.FeePercent);
            }
        }
        public List<string> GetBestCourses()
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.Courses.Where(course => context.Purchases
                                          .GroupBy(purchase => purchase.IdCourse)
                                          .OrderByDescending(group => group.Count())
                                          .Take(2)
                                          .Select(group => group.Key)
                                          .Contains(course.IdCourse))
                                     .Select(course => course.CourseName)
                                     .ToList();
            }
        }
    }
}
