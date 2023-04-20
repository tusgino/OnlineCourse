using Common.DAL;
using Common.Req.User;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserRep : GenericRep<WebsiteKhoaHocOnline_V4Context, User>
    {
        public User GetUserByID(Guid? id)
        {
            return All.Where(u => u.IdUser == id).FirstOrDefault()!;
        }
        public List<UserModel_Admin> GetAllUsersByFiltering(string? _title_like, DateTime? _start_date_create, DateTime? _end_date_create, bool? _is_student, bool? _is_expert, bool? _is_admin, bool? _status_active, bool? _status_banned)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {

                var id_accounts = context.Accounts.Where(account => account.DateCreate >= _start_date_create && account.DateCreate <= _end_date_create).Select(account => account.IdAccount).ToList();


                List<int> user_types = new List<int>();
                if (_is_admin == true) user_types.Add(0);
                if (_is_expert == true) user_types.Add(1);
                if (_is_student == true) user_types.Add(2);

                List<int> user_status = new List<int>();
                if (_status_active == true) user_status.Add(1);
                if (_status_banned == true) user_status.Add(0);

                //return context.Users.Where(user =>
                //    user.Name.Contains(_title_like == null ? "" : _title_like) &&
                //    id_accounts.Contains(user.IdAccount ?? Guid.Empty) &&
                //    user_types.Contains(user.IdTypeOfUser ?? -1) &&
                //    user_status.Contains(user.Status ?? -1)
                //).ToList();

                List<User> data = context.Users.Where(user =>
                    user.Name.Contains(_title_like == null ? "" : _title_like) &&
                    id_accounts.Contains(user.IdAccount ?? Guid.Empty) &&
                    user_types.Contains(user.IdTypeOfUser ?? -1) &&
                    user_status.Contains(user.Status ?? -1)
                ).ToList();

                List<UserModel_Admin> res = new List<UserModel_Admin>();
                foreach(User user in data)
                {
                    context.Entry(user).Reference(user => user.IdAccountNavigation).Load();
                    context.Entry(user).Reference(user => user.IdBankAccountNavigation).Load();
                    context.Entry(user).Reference(user => user.IdTypeOfUserNavigation).Load();

                    res.Add(new UserModel_Admin
                    {
                        ID = user.IdUser,
                        Name = user.Name,
                        TypeOfUser = user.IdTypeOfUserNavigation!.TypeOfUserName,
                        DateOfBirth = (user.DateOfBirth == null) ? "" : user.DateOfBirth.Value.ToShortDateString(),
                        PhoneNumber = (user.PhoneNumber == null) ? "" : user.PhoneNumber,
                        IDCard = (user.IdCard == null) ? "" : user.IdCard,
                        Email = (user.Email == null) ? "" : user.Email,
                        DateCreate = user.IdAccountNavigation!.DateCreate!.Value.ToShortDateString(),
                        Status = user.Status == 1 ? "Hoạt động" : (user.Status == 0 ? "Bị khoá" : "Cấm vĩnh viễn"), 
                    });
                }


                return res;
            }
        }
        public bool UpdateUserByID(Guid id, JsonPatchDocument newUser)
        {
            var user = GetUserByID(id);
            if (user != null)
            {
                newUser.ApplyTo(user);
                Context.SaveChanges();
                return true;
            }
            return false;
        }
        public List<object> GetAllStudentForAnalytics(string? _student_name_like, int? _start_purchase_course, int? _end_purchase_course, int? _start_finish_course, int? _end_finish_course)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                LessonRep lessonRep = new LessonRep();

                List<User> users = context.Users.Where(user => user.Name.Contains(_student_name_like == null? "" : _student_name_like) &&
                                                       user.IdTypeOfUser == 2
                ).ToList();


                // fitler by  purchased course
                foreach(User user in users)
                {
                    List<Course> courses = new List<Course>();
                    foreach (Purchase purchase in context.Purchases)
                    {
                        if (purchase.IdUser == user.IdUser)
                        {
                            var course = context.Courses.FirstOrDefault(course => course.IdCourse == purchase.IdCourse);

                            courses.Add(course!);
                        }
                    }
                    if (courses.Count < _start_purchase_course || courses.Count > _end_purchase_course)
                    {
                        users.Remove(user);
                    }
                }

                //filter by finished course
                foreach(User user in users)
                {
                    List<Course> courses = new List<Course>();
                    foreach(Study study in context.Studies)
                    {
                        if (study.IdUser == user.IdUser && study.Status == 1 && lessonRep.IsLastOfCourse(study.IdLesson ?? Guid.Empty) == true)
                        {
                            var lesson = context.Lessons.FirstOrDefault(lesson => lesson.IdLesson == study.IdLesson);
                            var chapter = context.Chapters.FirstOrDefault(chapter => chapter.IdChapter == lesson.IdChapter);
                            var course = context.Courses.FirstOrDefault(course => course.IdCourse == chapter.IdCourse);

                            courses.Add(course);
                        }
                    }
                    if(courses.Count < _start_finish_course || courses.Count > _end_finish_course)
                    {
                        users.Remove(user);
                    } 
                }


                List<List<Course>> purchased_courses = new List<List<Course>>();

                foreach (User user in users)
                {
                    List<Course> courses = new List<Course>();
                    foreach (Purchase purchase in context.Purchases)
                    {
                        if (purchase.IdUser == user.IdUser)
                        {
                            var course = context.Courses.FirstOrDefault(course => course.IdCourse == purchase.IdCourse);

                            courses.Add(course!);
                        }
                    }
                    purchased_courses.Add(courses);
                }

                List<List<Course>> finish_courses = new List<List<Course>>();
                foreach(User user in users)
                {
                    List<Course> courses = new List<Course>();
                    foreach (Study study in context.Studies)
                    {
                        if (study.IdUser == user.IdUser && study.Status == 1 && lessonRep.IsLastOfCourse(study.IdLesson ?? Guid.Empty) == true)
                        {
                            var lesson = context.Lessons.FirstOrDefault(lesson => lesson.IdLesson == study.IdLesson);
                            var chapter = context.Chapters.FirstOrDefault(chapter => chapter.IdChapter == lesson.IdChapter);
                            var course = context.Courses.FirstOrDefault(course => course.IdCourse == chapter.IdCourse);

                            courses.Add(course);
                        }
                    }
                    finish_courses.Add(courses);
                }



                List<object> data = new List<object>();
                int i = 0;
                foreach(User user in context.Users)
                {
                    if (user.IdTypeOfUser == 2)
                    {
                        data.Add(new 
                        {
                            student_name = user.Name,
                            purchased_courses_count = purchased_courses[i].Count,
                            finished_courses_count = finish_courses[i].Count
                        });
                        ++i;
                    }
                }

                return data;


            }
        }
        public List<object> GetAllExpertsForAnalytics(string? _expert_name_like, int? _start_upload_course, int? _end_upload_course, long? _start_revenue, long? _end_revenue)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                CourseRep courseRep = new CourseRep();

                List<User> experts = context.Users.Where(user => user.Name.Contains(_expert_name_like == null ? "" : _expert_name_like) && user.IdTypeOfUser == 1).ToList();

                // filter expert 

                foreach(User user in experts)
                {
                    List<Course> courses = new List<Course>();
                    long revenue = 0;
                    foreach(Course course in context.Courses)
                    {
                        if(course.IdUser == user.IdUser)
                        {
                            courses.Add(course);
                            revenue += Convert.ToInt64((100 - course.FeePercent) * course.Price * courseRep.GetNumberOfRegisterdUser(course.IdCourse));

                        }
                    }
                    if(courses.Count < _start_upload_course || courses.Count > _end_upload_course || revenue < _start_revenue || revenue > _end_revenue)
                    {
                        experts.Remove(user);
                    } 
                }


                List<object> data = new List<object>();
                
                foreach(User user in experts)
                {
                    List<Course> courses = new List<Course>();
                    long revenue = 0;
                    foreach (Course course in context.Courses)
                    {
                        if (course.IdUser == user.IdUser)
                        {
                            courses.Add(course);
                            revenue += Convert.ToInt64((100 - course.FeePercent) * course.Price * courseRep.GetNumberOfRegisterdUser(course.IdCourse));

                        }
                    }
                    data.Add(new
                    {
                        _expert_name = user.Name,
                        _numOfUploadCourse = courses.Count,
                        _revenue = revenue,
                    });
                }

                return data;

            }
        }
    }
}
