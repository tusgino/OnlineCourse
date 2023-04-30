using Common.DAL;
using Common.Req.Course;
using Common.Rsp.DTO;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                        Avatar = user.Avatar,
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


                List<Purchase> purchases = context.Purchases.ToList();

                // fitler by  purchased course
                foreach(User user in users.ToList())
                {
                    List<Course> courses = new List<Course>();
                    foreach (Purchase purchase in purchases)
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

                List<Study> studies = context.Studies.ToList(); 

                //filter by finished course
                foreach(User user in users)
                {
                    List<Course> courses = new List<Course>();
                    foreach(Study study in studies)
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
                    foreach (Purchase purchase in purchases)
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
                    foreach (Study study in studies)
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
                foreach(User user in users)
                {
                    
                    data.Add(new 
                    {
                        student_name = user.Name,
                        purchased_courses_count = purchased_courses[i].Count,
                        finished_courses_count = finish_courses[i].Count
                    });
                    ++i;
                    
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

                foreach(User user in experts.ToList())
                {
                    List<Course> courses = new List<Course>();
                    long revenue = 0;
                    foreach(Course course in context.Courses)
                    {
                        if(course.IdUser == user.IdUser) 
                        {
                            courses.Add(course);
                            double? earn = course.Price * (1 - course.Discount/100) * courseRep.GetNumberOfRegisterdUser(course.IdCourse);
                            revenue += Convert.ToInt64((1 - course.FeePercent / 100) * earn);
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
                            revenue += Convert.ToInt64((1 - course.FeePercent/100) * course.Price * (1 - course.Discount/100) * courseRep.GetNumberOfRegisterdUser(course.IdCourse));

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
        public List<object> GetAllUsersByType()
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.Users.GroupBy(user => user.IdTypeOfUser)
                                    .Select(group => new { TypeOfUser = group.Key, Number = group.Count()})
                                    .ToList<object>();
            }
        }
        public List<object> GetNewUsers()
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.Users.Join(context.Accounts, user => user.IdAccount, account => account.IdAccount, (user, account) => new { User = user, Account = account })
                                    .OrderByDescending(element => element.Account.DateCreate)
                                    .Select(element => element.User.Name)
                                    .Take(6)
                                    .ToList<object>();
            }
        } 
        public List<object> GetAllExpertRequests(string _name, DateTime _date_create_from, DateTime _date_create_to)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                AccountRep accountRep = new AccountRep();
                BankInfoRep bankInfoRep = new BankInfoRep();
                DegreeRep degreeRep = new DegreeRep();

                List<object> res = new List<object>();

                //context.Users.Where(user => user.Name.Contains(_name) && user.IdTypeOfUser == 1 && user.Status == -2)
                //            .Join(context.Accounts, user => user.IdAccount, account => account.IdAccount, (user, account) => new { _User = user, _Account = account })
                //            .Where(group => group._Account.DateCreate >= _date_create_from && group._Account.DateCreate <= _date_create_to)
                //            .Join(context.BankInfos, group => group._User.IdBankAccount, bankinfo => bankinfo.IdBankAccount, (group, bankinfo) => new { _Group = group, _BankInfo = bankinfo })
                //            .Join(context.Degrees, group => group._Group._User.IdUser, degree => degree.IdUser, (group, degree) => new { _Group = group, _Degree = degree })
                //            .Select(group => new
                //            {
                //                group._Group._Group._User.IdUser,
                //                group._Group._Group._User.Name,
                //                group._Group._Group._User.DateOfBirth,
                //                group._Group._Group._User.PhoneNumber,
                //                group._Group._Group._User.IdCard,
                //                group._Group._Group._User.Email,
                //                group._Group._Group._Account.DateCreate,
                //                group._Group._BankInfo.BankAccountNumber,
                //                group._Group._BankInfo.BankName,
                //                DegreeName = group._Degree.Name,
                //                group._Degree.Image,
                //                group._Degree.Description
                //            });


                foreach(var user in context.Users.Where(user => user.Name.Contains(_name) && user.IdTypeOfUser == 1 && user.Status == -2)
                             .Join(context.Accounts, user => user.IdAccount, account => account.IdAccount, (user, account) => new { _User = user, _Account = account })
                             .Where(group => group._Account.DateCreate >= _date_create_from && group._Account.DateCreate <= _date_create_to)
                             .Select(group => new { group._User, group._Account.DateCreate}).ToList())
                {
                    BankInfo bankInfo = bankInfoRep.GetBankInfoByID(user._User.IdBankAccount ?? Guid.Empty);
                    List<Degree> degrees = degreeRep.GetDegreesByIdUser(user._User.IdUser);

                    res.Add(new
                    {
                        IdUser = user._User.IdUser,
                        Name = user._User.Name,
                        DateOfBirth = user._User.DateOfBirth,
                        PhoneNumber = user._User.PhoneNumber,
                        IdCard = user._User.IdCard,
                        Email = user._User.Email,
                        DateCreate = user.DateCreate,
                        BankNumber = bankInfo != null ? bankInfo.BankAccountNumber : "",
                        BankName = bankInfo != null ? bankInfo.BankName : "",
                        Degrees = degrees,
                    });

                }

                






                //var query = from user in context.Users
                //            join account in context.Accounts on user.IdAccount equals account.IdAccount
                //            join bankInfo in context.BankInfos on user.IdBankAccount equals bankInfo.IdBankAccount into bankInfoGroup
                //            from bankInfo in bankInfoGroup.DefaultIfEmpty()
                //            join degree in context.Degrees on user.IdUser equals degree.IdUser into degreeGroup
                //            from degree in degreeGroup.DefaultIfEmpty()
                //            where user.IdTypeOfUser == 1 && user.Status == -2
                //            select new
                //            {
                //                user.IdUser,
                //                user.Name,
                //                user.DateOfBirth,
                //                user.PhoneNumber,
                //                user.IdCard,
                //                user.Email,
                //                account.DateCreate,
                //                BankAccountNumber = (bankInfo == null ? "" : bankInfo.BankAccountNumber),
                //                BankName = (bankInfo == null ? "" : bankInfo.BankName),
                //                Degrees = degreeGroup.Select(degree => new {degree.IdDegree, degree.Name, degree.Image, degree.Description}).ToList(),
                //            };

                //foreach (var element in query)
                //{
                //    Console.WriteLine(element);
                //    res.Add(new
                //    {
                //        IdUser = element.IdUser,
                //        Name = element.Name,
                //        DateOfBirth = element.DateOfBirth,
                //        PhoneNumber = element.PhoneNumber,
                //        IdCard = element.IdCard,
                //        Email = element.Email,
                //        DateCreate = element.DateCreate,
                //        BankNumber = element.BankAccountNumber,
                //        BankName = element.BankName,
                //        Degree = element.Degrees
                //    });

                //}



                return res;
            }
        }
    }
}
