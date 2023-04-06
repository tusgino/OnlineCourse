using Common.BLL;
using Common.Req.Course;
using Common.Rsp;
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
    }
}
