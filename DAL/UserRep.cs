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
                var id_accounts = context.Accounts.Where(account =>
                                    account.DateCreate >= _start_date_create && 
                                    account.DateCreate <= _end_date_create)
                                    .Select(account => account.IdAccount).ToList();

                List<int> user_types = new List<int>();
                if (_is_admin == true) user_types.Add(0);
                if (_is_expert == true) user_types.Add(1);
                if (_is_student == true) user_types.Add(2);

                List<int> user_status = new List<int>();
                if (_status_active == true) user_status.Add(1);
                if (_status_banned == true) user_status.Add(0);

                List<User> data = context.Users.Where(user =>
                    user.Name.Contains(_title_like) &&
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

                List<User> students = context.Users.Where(user => user.Name.Contains(_student_name_like) &&
                                                                  user.IdTypeOfUser == 2).ToList();
                List<StudentDTO> data = new List<StudentDTO>();
                foreach(User student in students.ToList())
                {
                    // find purchased courses
                    int NumOfPurchasedCourse = context.Purchases.Where(purchase => purchase.IdUser == student.IdUser && purchase.IdTradeNavigation.TradeStatus == 1).Count();

                    // find finished courses
                    int NumOfFinishedCourse = 0;
                    List<Study> studies = context.Studies.Where(study => study.IdUser == student.IdUser && study.Status == 1).ToList();
                    foreach (Study study in studies)
                    {
                        if (lessonRep.IsLastOfCourse(study.IdLesson) == true)
                        {
                            ++NumOfFinishedCourse;
                        }
                    }

                    if (NumOfPurchasedCourse >= _start_purchase_course && NumOfPurchasedCourse <= _end_purchase_course && NumOfFinishedCourse >= _start_finish_course && NumOfFinishedCourse <= _end_finish_course)
                    {
                        data.Add(new StudentDTO
                        {
                            Name = student.Name,
                            NumOfPurchasedCourse = NumOfPurchasedCourse,
                            NumOfFinishedCourse = NumOfFinishedCourse,
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

                List<User> experts = context.Users.Where(user => user.Name.Contains(_expert_name_like) && user.IdTypeOfUser == 1).ToList();

                List<ExpertDTO> data = new List<ExpertDTO>();

                foreach(User expert in experts.ToList())
                {
                    // find revenue of expert
                    List<long> revenue = new List<long>();
                    for (int i = 0; i < DateTime.Now.Month; i++) revenue.Add(0);

                    int totalSales = 0;
                    string bestSalesCourse = ""; int count = 0;
                    var uploadedCourses = context.Courses.Where(course => course.IdUser == expert.IdUser).ToList();
                    foreach (Course course in uploadedCourses)
                    {
                        // find best sales course
                        int salepercourse = context.Purchases.Where(purchase => purchase.IdCourse == course.IdCourse && purchase.IdTradeNavigation.TradeStatus == 1).Count();
                        totalSales += salepercourse;
                        if(salepercourse > count)
                        {
                            count = salepercourse;
                            bestSalesCourse = course.CourseName;
                        }

                        // find expert current year revenue
                        for (int i = 0; i < DateTime.Now.Month; i++)
                        {
                            var purchases = context.Purchases.Where(purchase => purchase.IdCourse == course.IdCourse && purchase.DateOfPurchase.Value.Month == i + 1 && purchase.DateOfPurchase.Value.Year == DateTime.Now.Year).ToList();
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
                                    .OrderByDescending(user => user.IdAccountNavigation.DateCreate)
                                    .Select(user => user.Name)
                                    .Take(6)
                                    .ToList<object>();
            }
        } 
        public List<object> GetAllExpertRequests(string? _name, DateTime? _date_create_from, DateTime? _date_create_to)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                DegreeRep degreeRep = new DegreeRep();

                List<object> res = new List<object>();

                foreach(var user in context.Users.Where(user => user.Name.Contains(_name) && 
                                                                user.IdTypeOfUser == 1 && 
                                                                user.Status == -2 &&
                                                                user.IdAccountNavigation.DateCreate >= _date_create_from && 
                                                                user.IdAccountNavigation.DateCreate <= _date_create_to)
                                                 .ToList())
                {
                    context.Entry(user).Reference(user => user.IdAccountNavigation).Load();
                    context.Entry(user).Reference(user => user.IdBankAccountNavigation).Load();

                    List<Degree> degrees = degreeRep.GetDegreesByIdUser(user.IdUser);

                    res.Add(new
                    {
                        Avatar = user.Avatar,
                        IdUser = user.IdUser,
                        Name = user.Name,
                        DateOfBirth = user.DateOfBirth,
                        PhoneNumber = user.PhoneNumber,
                        IdCard = user.IdCard,
                        Email = user.Email,
                        DateCreate = user.IdAccountNavigation.DateCreate,
                        BankNumber = user.IdBankAccountNavigation == null ? null : user.IdBankAccountNavigation.BankAccountNumber,
                        BankName = user.IdBankAccountNavigation == null ? null : user.IdBankAccountNavigation.BankName,
                        Degrees = degrees,
                    });

                }

                return res;
            }
        }
        public List<StudentDTO> GetBestStudents()
        {
            var students = GetAllStudentForAnalytics("", 0, 99999, 0, 99999);
            return students.OrderByDescending(student => student.NumOfFinishedCourse).Take(4).ToList();
        }
        public List<ExpertDTO> GetBestExperts()
        {
            var experts = GetAllExpertsForAnalytics("", 0, 99999, 0, 999999999999999);
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

        public object StatisticExpert(Guid idExpert)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                ExpertDTO data = new ExpertDTO();

                List<long> revenue = new List<long>();
                for (int i = 0; i < DateTime.Now.Month; i++) revenue.Add(0);

                int totalSales = 0;
                string bestSalesCourse = ""; int count = 0;
                var uploadedCourses = context.Courses.Where(course => course.IdUser == idExpert).ToList();
                foreach (Course course in uploadedCourses)
                {
                    int salepercourse = context.Purchases.Where(purchase => purchase.IdCourse == course.IdCourse).Count();
                    totalSales += salepercourse;
                    if (salepercourse > count)
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

                data = new ExpertDTO
                {
                    ID = idExpert,
                    Name = "alo",
                    CurrentYearRevenue = revenue,
                    NumOfUploadedCourse = uploadedCourses.Count,
                    TotalSales = totalSales,
                    BestSalesCourse = bestSalesCourse
                };

                return data;
            }
            
        }
    }
}
