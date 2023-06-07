using Common.BLL;
using Common.Req.User;
using Common.Rsp;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Req.Course;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Common.Rsp.DTO;

namespace BLL
{
    public class UserSvc : GenericSvc<UserRep, User>
    {
        private readonly UserRep _userRep = new UserRep();
        public SingleRsp GetUserByID(Guid? id)
        {
            var rsp = new SingleRsp();
            if((rsp.Data = _userRep.GetUserByID(id)) == null)
            {
                rsp.SetError("User not found");
            }
            return rsp;
        }
        public SingleRsp GetAllUsersByFiltering(UserFilteringReq userFilteringReq)
        {
            userFilteringReq.ValidateData();

            var users = _userRep.GetAllUsersByFiltering(userFilteringReq.title_like,
                                                        userFilteringReq.start_date_create,
                                                        userFilteringReq.end_date_create,
                                                        userFilteringReq.is_student,
                                                        userFilteringReq.is_expert,
                                                        userFilteringReq.is_admin,
                                                        userFilteringReq.status_active,
                                                        userFilteringReq.status_banned);

            int limit = 10;
            int offset = (userFilteringReq.Page - 1) * limit;
            int total = users.Count;

            var data = users.OrderBy(user => user.Name).Skip(offset).Take(limit).ToList();

            object res = new
            {
                _data = data,
                _totalRows = total,
            };

            var rsp = new SingleRsp();

            if (data == null)
            {
                rsp.SetError("User not found");
            }
            else
            {
                rsp.Data = res;
            }
            return rsp;
        }
        public SingleRsp UpdateUser(Guid iD_User, JsonPatchDocument patchDoc)
        {
            var rsp = new SingleRsp();
            if (!_userRep.UpdateUserByID(iD_User, patchDoc))
            {
                rsp.SetError("Update failed");
            }
            return rsp;
        }
        public SingleRsp GetAllStudentForAnalytics(StudentAnalyticsReq studentAnalyticsReq)
        {
            studentAnalyticsReq.ValidateData();

            var students = _userRep.GetAllStudentForAnalytics(studentAnalyticsReq.student_name_like,
                                                          studentAnalyticsReq.start_purchase_course,
                                                          studentAnalyticsReq.end_purchase_course,
                                                          studentAnalyticsReq.start_finish_course,
                                                          studentAnalyticsReq.end_finish_course);

            int limit = 10;
            int offset = (studentAnalyticsReq.Page - 1) * limit;
            int total = students.Count;

            var data = students.Skip(offset).Take(limit).ToList();

            object res = new
            {
                _data = data,
                _totalRows = total,
            };

            var rsp = new SingleRsp();
            if(data == null)
            {
                rsp.SetError("Not found student");
            }
            else
            {
                rsp.Data = res;
            }

            return rsp;
        }
        public SingleRsp GetAllExpertsForAnalytics (ExpertAnalyticsReq expertAnalyticsReq)
        {
            expertAnalyticsReq.ValidateData();

            var experts = _userRep.GetAllExpertsForAnalytics(expertAnalyticsReq.expert_name,
                                                             expertAnalyticsReq.start_upload_course,
                                                             expertAnalyticsReq.end_upload_course,
                                                             expertAnalyticsReq.start_revenue,
                                                             expertAnalyticsReq.end_revenue);

            int limit = 10;
            int offset = (expertAnalyticsReq.Page - 1) * limit;
            int total = experts.Count;

            var data = experts.Skip(offset).Take(limit).ToList();

            object res = new
            {
                _data = data,
                _totalRows = total,
            };

            var rsp = new SingleRsp();
            if(data == null)
            {
                rsp.SetError("Not found expert");
            }
            else
            {
                rsp.Data = res;
            }

            return rsp;
        }
        public SingleRsp GetExpertRevenueByID(Guid IdExpert)
        {
            var data = _userRep.GetExpertRevenueByID(IdExpert);
            
            var rsp = new SingleRsp();
            if(data == null)
            {
                rsp.SetError("Data not found");
            }
            else
            {
                rsp.Data = data;
            }

            return rsp;
        }
        public SingleRsp GetAllUsersByType()
        {
            var data = _userRep.GetAllUsersByType();

            var rsp = new SingleRsp();

            if(data == null)
            {
                rsp.SetError("Data not found");
            }
            else
            {
                rsp.Data = data;
            }
            return rsp;
        }
        public SingleRsp GetNewUsers()
        {
            var data = _userRep.GetNewUsers();

            var rsp = new SingleRsp();

            if(data == null)
            {
                rsp.SetError("Not found user");
            }
            else
            {
                rsp.Data = data;
            }
            return rsp;
        }
        public SingleRsp GetAllExpertRequests(ExpertRegisterReq expertRegisterReq)
        {
            expertRegisterReq.ValidateData();

            var experts = _userRep.GetAllExpertRequests(expertRegisterReq.name,
                                                        expertRegisterReq.date_create_from,
                                                        expertRegisterReq.date_create_to);

            int limit = 10;
            int offset = (expertRegisterReq.Page - 1) * limit;
            int total = experts.Count;

            var res = experts.Skip(offset).Take(limit).ToList();

            object data = new
            {
                _data = res,
                _totalRows = total,
            };

            var rsp = new SingleRsp();

            if(data == null)
            {
                rsp.SetError("Not found request");
            }
            else
            {
                rsp.Data = data;
            }
            return rsp;
        }
        public SingleRsp GetBestStudents()
        {
            var data = _userRep.GetBestStudents();

            var rsp = new SingleRsp();

            if(data == null)
            {
                rsp.SetError("Not found student");
            }
            else
            {
                rsp.Data = data;
            }

            return rsp;
        }
        public SingleRsp GetBestExperts()
        {
            var data = _userRep.GetBestExperts();

            var rsp = new SingleRsp();

            if (data == null)
            {
                rsp.SetError("Not found expert");
            }
            else
            {
                rsp.Data = data;
            }

            return rsp;
        }
        public SingleRsp SendMail(EmailDTO emailDTO)
        {
            var data = _userRep.SendMail(emailDTO);

            var rsp = new SingleRsp();
            
            if(data == null)
            {
                rsp.SetError("Can not send mail");
            }
            else
            {
                rsp.Data = data;
            }
            return rsp;
        }
    }
}
