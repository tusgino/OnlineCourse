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
using System.Net.Mail;
using System.Net;
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
        public List<StudentDTO> GetAllStudentForAnalytics(string? _student_name_like, int? _start_purchase_course, int? _end_purchase_course, int? _start_finish_course, int? _end_finish_course)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                LessonRep lessonRep = new LessonRep();

                List<User> students = context.Users.Where(user => user.Name.Contains(_student_name_like == null? "" : _student_name_like) &&
                                                       user.IdTypeOfUser == 2).ToList();

                List<StudentDTO> data = new List<StudentDTO>();
                foreach(User student in students.ToList())
                {
                    List<Purchase> purchases = context.Purchases.Where(purchase => purchase.IdUser == student.IdUser).ToList();
                    List<Course> purchasedcourses = new List<Course>();
                    foreach (Purchase purchase in purchases)
                    {
                        var trade = context.TradeDetails.FirstOrDefault(trade => trade.IdTrade == purchase.IdTrade);
                        if (trade.TradeStatus == 1)
                        {
                            var course = context.Courses.FirstOrDefault(course => course.IdCourse == purchase.IdCourse);
                            purchasedcourses.Add(course!);
                        }
                    }

                    List<Study> studies = context.Studies.Where(study => study.IdUser == student.IdUser && study.Status == 1).ToList();
                    List<Course> finishedcourses = new List<Course>();
                    foreach (Study study in studies)
                    {
                        if (lessonRep.IsLastOfCourse(study.IdLesson) == true)
                        {
                            var lesson = context.Lessons.FirstOrDefault(lesson => lesson.IdLesson == study.IdLesson);
                            var chapter = context.Chapters.FirstOrDefault(chapter => chapter.IdChapter == lesson.IdChapter);
                            var course = context.Courses.FirstOrDefault(course => course.IdCourse == chapter.IdCourse);

                            finishedcourses.Add(course!);
                        }
                    }

                    if (purchasedcourses.Count >= _start_purchase_course && purchasedcourses.Count <= _end_purchase_course && finishedcourses.Count >= _start_finish_course && finishedcourses.Count <= _end_finish_course)
                    {
                        data.Add(new StudentDTO
                        {
                            Name = student.Name,
                            NumOfPurchasedCourse = purchasedcourses.Count,
                            NumOfFinishedCourse = finishedcourses.Count,
                        });
                    }
                }

                return data;
            }
        }
        public List<ExpertDTO> GetAllExpertsForAnalytics(string? _expert_name_like, int? _start_upload_course, int? _end_upload_course, long? _start_revenue, long? _end_revenue)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                CourseRep courseRep = new CourseRep();

                List<User> experts = context.Users.Where(user => user.Name.Contains(_expert_name_like == null ? "" : _expert_name_like) && user.IdTypeOfUser == 1).ToList();

                List<ExpertDTO> data = new List<ExpertDTO>();

                foreach(User expert in experts.ToList())
                {
                    List<long> revenue = new List<long>();
                    for (int i = 0; i < DateTime.Now.Month; i++) revenue.Add(0);

                    int totalSales = 0;
                    string bestSalesCourse = ""; int count = 0;
                    var uploadedCourses = context.Courses.Where(course => course.IdUser == expert.IdUser).ToList();
                    foreach (Course course in uploadedCourses)
                    {
                        int salepercourse = context.Purchases.Where(purchase => purchase.IdCourse == course.IdCourse).Count();
                        totalSales += salepercourse;
                        if(salepercourse > count)
                        {
                            count = salepercourse;
                            bestSalesCourse = course.CourseName;
                        }

                        for (int i = 0; i < DateTime.Now.Month; i++)
                        {
                            var purchases = context.Purchases.Where(purchase => purchase.IdCourse == course.IdCourse && purchase.DateOfPurchase.Value.Month == i + 1).ToList();
                            foreach (Purchase purchase in purchases)
                            {
                                var trade = context.TradeDetails.FirstOrDefault(trade => trade.IdTrade == purchase.IdTrade);
                                revenue[i] += Convert.ToInt64(Convert.ToInt64(trade.Balance) * (100 - course.FeePercent) / 100);
                            }
                        }
                    }
                    if (uploadedCourses.Count >= _start_upload_course && uploadedCourses.Count <= _end_upload_course && revenue[DateTime.Now.Month - 1] >= _start_revenue && revenue[DateTime.Now.Month - 1] <= _end_revenue)
                    {
                        data.Add(new ExpertDTO
                        {
                            ID = expert.IdUser,
                            Name = expert.Name,
                            CurrentYearRevenue = revenue,
                            NumOfUploadedCourse = uploadedCourses.Count,
                            TotalSales = totalSales,
                            BestSalesCourse = bestSalesCourse
                        });
                    } 
                }

                return data;
            }
        }
        public List<long> GetExpertRevenueByID(Guid IdExpert)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                List<long> revenue = new List<long>(12);

                var uploadedCourses = context.Courses.Where(course => course.IdUser == IdExpert).ToList();
                foreach (Course course in uploadedCourses)
                {
                    for(int i = 0; i < 12; i++)
                    {
                        var purchases = context.Purchases.Where(purchase => purchase.IdCourse == course.IdCourse && purchase.DateOfPurchase.Value.Month == i + 1).ToList();
                        foreach (Purchase purchase in purchases)
                        {
                            var trade = context.TradeDetails.FirstOrDefault(trade => trade.IdTrade == purchase.IdTrade);
                            revenue[i] += Convert.ToInt64(Convert.ToInt64(trade.Balance) * (100 - course.FeePercent) / 100);
                        }
                    }
                }

                return revenue;
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
                return context.Users.Where(user => user.Status == 1)
                                    .Join(context.Accounts, user => user.IdAccount, account => account.IdAccount, (user, account) => new { User = user, Account = account })
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

                foreach(var user in context.Users.Where(user => user.Name.Contains(_name) && user.IdTypeOfUser == 1 && user.Status == -2)
                             .Join(context.Accounts, user => user.IdAccount, account => account.IdAccount, (user, account) => new { _User = user, _Account = account })
                             .Where(group => group._Account.DateCreate >= _date_create_from && group._Account.DateCreate <= _date_create_to)
                             .Select(group => new { group._User, group._Account.DateCreate}).ToList())
                {
                    BankInfo bankInfo = bankInfoRep.GetBankInfoByID(user._User.IdBankAccount ?? Guid.Empty);
                    List<Degree> degrees = degreeRep.GetDegreesByIdUser(user._User.IdUser);

                    res.Add(new
                    {
                        Avatar = user._User.Avatar,
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

                return res;
            }
        }
        public List<StudentDTO> GetBestStudents()
        {
            var students = GetAllStudentForAnalytics(null, 0, 99999, 0, 99999);
            return students.OrderByDescending(student => student.NumOfFinishedCourse).Take(4).ToList();
        }
        public List<ExpertDTO> GetBestExperts()
        {
            var experts = GetAllExpertsForAnalytics(null, 0, 99999, 0, 999999999999999);
            return experts.OrderByDescending(expert => expert.CurrentYearRevenue[DateTime.Now.Month - 1]).Take(4).ToList();
        }
        public MailMessage SendMail(EmailDTO emailDTO)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var target = context.Users.FirstOrDefault(user => user.IdUser == emailDTO.IdUser);

                string senderEmail = "thhonlinecourse@gmail.com";
                string senderPassword = "rlzbjtvrqjnuftyt"; // su dung app password thay vi password binh thuong

                MailMessage message = new MailMessage(senderEmail, target.Email);

                message.Subject = emailDTO.Subject;
                message.Body = emailDTO.Body;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                

                client.Send(message);

                return message;
            }
        }
    }
}
