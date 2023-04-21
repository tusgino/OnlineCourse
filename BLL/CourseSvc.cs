using Common.BLL;
using Common.Req.Course;
using Common.Req.User;
using Common.Rsp;
using Common.Rsp.DTO;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CourseSvc : GenericSvc<CourseRep, Course>
    {
        private readonly CourseRep _courseRep = new CourseRep();
        public SingleRsp GetAllCourse(CoursesPaginationReq coursesPaginationReq)
        {
            var rsp = new SingleRsp();

            var courses = _courseRep.GetAllCourseByName(coursesPaginationReq.Title_like).ToList();
            var offset = (coursesPaginationReq.Page - 1) * coursesPaginationReq.Limit;
            var total = courses.Count();
            int totalPage = (total % coursesPaginationReq.Limit) == 0 ? (int)(total / coursesPaginationReq.Limit) :
                (int)(1 + (total / coursesPaginationReq.Limit));


            var _data = _courseRep.GetAllCourse(offset, coursesPaginationReq.Limit, coursesPaginationReq.Title_like);

            object res = new
            {
                data = _data,
                pagination = new
                {
                    _page = coursesPaginationReq.Page,
                    _limit = coursesPaginationReq.Limit,
                    _totalRows = total,
                }
            };
            rsp.Data = res;

            return rsp;
        }

        public SingleRsp GetAllCourseByExpert(Guid id)
        {
            var rsp = new SingleRsp();

            if(_courseRep.GetAllCourseByExpert(id) == null)
            {
                rsp.SetError("Not found Course of this Expert");
            }
            else
            {
                rsp.Data = _courseRep.GetAllCourseByExpert(id);
            }

            return rsp;
        }

        public SingleRsp GetACourse(Guid id)
        {
            var rsp = new SingleRsp();

            if((rsp.Data = _courseRep.GetACourse(id)) == null){
                rsp.SetError("Not found Course");
            };

            return rsp;
        }

        public SingleRsp GetCourseByID(Guid id)
        {
            var res = new SingleRsp();

            if(_courseRep.GetCourseByID(id) == null)
            {
                res.SetError("Not found course");
            }
            else
            {
                res.Data = _courseRep.GetCourseByID(id);
            }

            return res;
        }
        public SingleRsp GetAllCoursesByFiltering(CoursesFilteringReq coursesFilteringReq, CoursesPaginationReq coursesPaginationReq)
        {
            if (coursesFilteringReq.start_day == null) coursesFilteringReq.start_day = new DateTime(1, 1, 1);
            if (coursesFilteringReq.end_day == null) coursesFilteringReq.end_day = new DateTime(9999, 1, 1);
            var courses = _courseRep.GetAllCourseByFiltering(coursesFilteringReq.text, coursesFilteringReq.category_name, coursesFilteringReq.start_day, coursesFilteringReq.end_day, coursesFilteringReq.status_active, coursesFilteringReq.status_store);

            int offset = (coursesPaginationReq.Page - 1) * coursesPaginationReq.Limit;
            int total = courses.Count;
            int totalPage = (total % coursesPaginationReq.Limit) == 0 ? (int)(total / coursesPaginationReq.Limit) :
                (int)(1 + (total / coursesPaginationReq.Limit));

            var data = courses.OrderBy(course => course.Name).Skip(offset).Take(coursesPaginationReq.Limit).ToList();

            object res = new
            {
                _data = data,
                _totalRows = total,
            };

            var rsp = new SingleRsp();

            if (data == null)
            {
                rsp.SetError("Not found course");
            }
            else
            {
                rsp.Data = res;
            }

            return rsp;
        }
        public SingleRsp GetCoursesByCategoryID(Guid _category_id)
        {
            var courses = _courseRep.GetAllCourseByCategoryID(_category_id);
            var rsp = new SingleRsp();

            rsp.Data = courses;
            return rsp;
        }
        public SingleRsp UpdateCourse(Guid _id_course, JsonPatchDocument newCourse)
        {
            var rsp = new SingleRsp();

            if(!_courseRep.UpdateCourse(_id_course, newCourse))
            {
                rsp.SetError("Update failed");
            }

            return rsp;
        }
        public SingleRsp GetAllCourseForAnalytics(CourseAnalyticsReq courseAnalyticsReq, CoursesPaginationReq coursesPaginationReq)
        {
            if (courseAnalyticsReq.start_reg_user == null) courseAnalyticsReq.start_reg_user = 0;
            if (courseAnalyticsReq.end_reg_user == null) courseAnalyticsReq.end_reg_user = int.MaxValue;

            if (courseAnalyticsReq.start_rate == null) courseAnalyticsReq.start_rate = 1;
            if (courseAnalyticsReq.end_rate == null) courseAnalyticsReq.end_rate = 5;

            var courses = _courseRep.GetAllCoursesForAnalytics(courseAnalyticsReq.title_like,
                                                               courseAnalyticsReq.start_reg_user,
                                                               courseAnalyticsReq.end_reg_user,
                                                               courseAnalyticsReq.start_rate,
                                                               courseAnalyticsReq.end_rate);

            int limit = 10;
            int offset = (coursesPaginationReq.Page - 1) * limit;
            int total = courses.Count;
            int totalPage = (total % coursesPaginationReq.Limit) == 0 ? (int)(total / coursesPaginationReq.Limit) :
                (int)(1 + (total / coursesPaginationReq.Limit));

            var data = courses.Skip(offset).Take(limit).ToList();

            object res = new
            {
                _data = data,
                _totalRows = total,
            };

            var rsp = new SingleRsp();
            if(data == null)
            {
                rsp.SetError("Not found course");
            } 
            else
            {
                rsp.Data = res;
            }



            return rsp;
        }
    }
}
