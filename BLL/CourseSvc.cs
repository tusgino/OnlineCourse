using Common.BLL;
using Common.Req.Course;
using Common.Rsp;
using Common.Rsp.DTO;
using DAL;
using DAL.Models;
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

        public SingleRsp GetAllCourseByIdUser(Guid id)
        {
            var rsp = new SingleRsp();
            
            

            if((rsp.Data = _courseRep.GetAllCourseByIdUser(id)) == null)
            {
                rsp.SetError("Not found Course of this Expert");
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

            var data = courses.Skip(offset).Take(coursesPaginationReq.Limit).ToList();

            var rsp = new SingleRsp();

            if (data == null)
            {
                rsp.SetError("Not found course");
            }
            else
            {
                rsp.Data = data;
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

    }
}
