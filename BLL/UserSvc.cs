﻿using Common.BLL;
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
                rsp.SetError("Not found user");
            }
            return rsp;
        }
        public SingleRsp GetAllUsersByFiltering(UserFilteringReq userFilteringReq, int page)
        {
            if (userFilteringReq.start_date_create == null) userFilteringReq.start_date_create = new DateTime(1, 1, 1);
            if (userFilteringReq.end_date_create == null) userFilteringReq.end_date_create = new DateTime(9999, 1, 1);

            var users = _userRep.GetAllUsersByFiltering(userFilteringReq.text, userFilteringReq.start_date_create, userFilteringReq.end_date_create, userFilteringReq.is_student, userFilteringReq.is_expert, userFilteringReq.is_admin, userFilteringReq.status_active, userFilteringReq.status_banned);

            int limit = 10;
            int offset = (page - 1) * limit;
            int total = users.Count;
            int totalPage = (total % limit == 0) ? (total / limit) : (1 + total / limit);

            var data = users.Skip(offset).Take(limit).ToList();

            var rsp = new SingleRsp();

            if (data == null)
            {
                rsp.SetError("Not found user");
            }
            else
            {
                rsp.Data = data;
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
        public SingleRsp GetAllStudentForAnalytics(StudentAnalyticsReq studentAnalyticsReq, CoursesPaginationReq coursesPaginationReq)
        {
            if (studentAnalyticsReq.start_purchase_course == null) studentAnalyticsReq.start_purchase_course = 0;
            if (studentAnalyticsReq.end_purchase_course == null) studentAnalyticsReq.end_purchase_course = int.MaxValue;

            if (studentAnalyticsReq.start_finish_course == null) studentAnalyticsReq.start_finish_course = 0;
            if (studentAnalyticsReq.end_finish_course == null) studentAnalyticsReq.end_finish_course = int.MaxValue;

            var students = _userRep.GetAllStudentForAnalytics(studentAnalyticsReq.student_name_like,
                                                          studentAnalyticsReq.start_purchase_course,
                                                          studentAnalyticsReq.end_purchase_course,
                                                          studentAnalyticsReq.start_finish_course,
                                                          studentAnalyticsReq.end_finish_course);

            int limit = 10;
            int offset = (coursesPaginationReq.Page - 1) * coursesPaginationReq.Limit;
            int total = students.Count;
            int totalPage = (total % limit == 0) ? (total / limit) : (1 + total / limit);

            var data = students.Skip(offset).Take(limit).ToList();


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
        public SingleRsp GetAllExpertsForAnalytics (ExpertAnalyticsReq expertAnalyticsReq, CoursesPaginationReq coursesPaginationReq)
        {
            if (expertAnalyticsReq.start_upload_course == null) expertAnalyticsReq.start_upload_course = 0;
            if (expertAnalyticsReq.end_upload_course == null) expertAnalyticsReq.end_upload_course = int.MaxValue;

            if (expertAnalyticsReq.start_revenue == null) expertAnalyticsReq.start_revenue = 0;
            if(expertAnalyticsReq.end_revenue == null) expertAnalyticsReq.end_revenue = int.MaxValue;

            var experts = _userRep.GetAllExpertsForAnalytics(expertAnalyticsReq.expert_name,
                                                             expertAnalyticsReq.start_upload_course,
                                                             expertAnalyticsReq.end_upload_course,
                                                             expertAnalyticsReq.start_revenue,
                                                             expertAnalyticsReq.end_revenue);

            int limit = 10;
            int offset = (coursesPaginationReq.Page - 1) * coursesPaginationReq.Limit;
            int total = experts.Count;
            int totalPage = (total % limit == 0) ? (total / limit) : (1 + total / limit);

            var data = experts.Skip(offset).Take(limit).ToList();

            var rsp = new SingleRsp();
            if(data == null)
            {
                rsp.SetError("Not found expert");
            }
            else
            {
                rsp.Data = data;
            }



            return rsp;

        }
    }
}
