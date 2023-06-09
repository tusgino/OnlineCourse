﻿using Common.BLL;
using Common.Req.Course;
using Common.Req.User;
using Common.Rsp;
using Common.Rsp.DTO;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var courses = _courseRep.GetAllCourseByName(coursesPaginationReq.Title_like).ToList();

            var offset = (coursesPaginationReq.Page - 1) * coursesPaginationReq.Limit;
            var total = courses.Count();

            var _data = _courseRep.GetAllCourse(offset, coursesPaginationReq.Limit, coursesPaginationReq.Title_like);

            var rsp = new SingleRsp();
            rsp.Data = new
            {
                data = _data,
                pagination = new
                {
                    _page = coursesPaginationReq.Page,
                    _limit = coursesPaginationReq.Limit,
                    _totalRows = total,
                }
            };

            return rsp;
        }

        public SingleRsp GetAllCourseByIdUser(Guid id)
        {
            var rsp = new SingleRsp();
            if ((rsp.Data = _courseRep.GetAllCourseByIdUser(id)) == null)
            {
                rsp.SetError("Not found Course of this Expert");
            }
            return rsp;
        }

        public SingleRsp GetACourse(Guid idCourse, Guid idUser)
        {
            var rsp = new SingleRsp();

            if ((rsp.Data = _courseRep.GetACourse(idCourse, idUser)) == null) {
                rsp.SetError("Course not found");
            };

            return rsp;
        }

        public SingleRsp GetCourseByID(Guid id)
        {
            var res = new SingleRsp();

            if (_courseRep.GetCourseByID(id) == null)
            {
                res.SetError("Course not found");
            }
            else
            {
                res.Data = _courseRep.GetCourseByID(id);
            }

            return res;
        }
        public SingleRsp GetAllCoursesByFiltering(CoursesFilteringReq coursesFilteringReq)
        {
            coursesFilteringReq.ValidateData();

            var courses = _courseRep.GetAllCourseByFiltering(coursesFilteringReq.title_like,
                                                             coursesFilteringReq.category_name,
                                                             coursesFilteringReq.start_upload_day,
                                                             coursesFilteringReq.end_upload_day,
                                                             coursesFilteringReq.status_active,
                                                             coursesFilteringReq.status_store);

            int limit = 10;
            int offset = (coursesFilteringReq.Page - 1) * limit;
            int total = courses.Count;

            var data = courses.OrderBy(course => course.Name).Skip(offset).Take(limit).ToList();

            object res = new
            {
                _data = data,
                _totalRows = total,
            };

            var rsp = new SingleRsp();

            if (data == null)
            {
                rsp.SetError("Course not found");
            }
            else
            {
                rsp.Data = res;
            }

            return rsp;
        }
        public SingleRsp GetCoursesByCategoryID(Guid _category_id)
        {
            var courses = _courseRep.GetAllCourseByCategoryID(_category_id)
                                    .OrderBy(course => course.CourseName)
                                    .Select(course => course.CourseName).ToList(); ;

            var rsp = new SingleRsp();

            if (courses == null)
            {
                rsp.SetError("Course not found");
            }
            else
            {
                rsp.Data = courses;
            }

            return rsp;
        }
        public SingleRsp UpdateCourse(Guid _id_course, JsonPatchDocument patchDoc)
        {
            var rsp = new SingleRsp();

            if (!_courseRep.UpdateCourse(_id_course, patchDoc))
            {
                rsp.SetError("Update failed");
            }

            return rsp;
        }
        public SingleRsp GetAllCourseForAnalytics(CourseAnalyticsReq courseAnalyticsReq)
        {
            courseAnalyticsReq.ValidateData();

            var courses = _courseRep.GetAllCoursesForAnalytics(courseAnalyticsReq.title_like,
                                                               courseAnalyticsReq.start_reg_user,
                                                               courseAnalyticsReq.end_reg_user,
                                                               courseAnalyticsReq.start_rate,
                                                               courseAnalyticsReq.end_rate);

            int limit = 10;
            int offset = (courseAnalyticsReq.Page - 1) * limit;
            int total = courses.Count;

            var data = courses.Skip(offset).Take(limit).ToList();

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
        public SingleRsp AddCourse(CourseReq courseReq)
        {
            var rsp = new SingleRsp();

            Course course = new Course
            {
                IdCategory = courseReq.IdCategory,
                CourseName = courseReq.CourseName,
                DateOfUpload = DateTime.Now,
                Description = courseReq.Description,
                Discount = courseReq.Discount,
                FeePercent = courseReq.FeePercent,
                IdCourse = Guid.NewGuid(),
                IdUser = courseReq.IdUser,
                Price = courseReq.Price,
                Status = courseReq.Status,
                Thumbnail = courseReq.Thumbnail,
                VideoPreview = courseReq.VideoPreview,
            };

            if (_courseRep.AddCourse(course))
            {
                rsp.Data = course.IdCourse;
            }
            else
            {
                rsp.SetError("Not found any category");
            }

            return rsp;
        }
        public SingleRsp GetAverageFeePercent()
        {
            var data = _courseRep.GetAverageFeePercent();
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
        public SingleRsp GetBestCourses()
        {
            var data = _courseRep.GetBestCourses();
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
        public SingleRsp ChangeStatus(Guid idCourse)
        {
            var rsp = new SingleRsp();

            if (!_courseRep.ChangeStatus(idCourse))
            {
                rsp.SetError("Can not change status");
            }

            return rsp;
        }
        public SingleRsp GetNumOfUploadedCourseByMonth(int year)
        {
            var data = _courseRep.GetNumOfUploadedCourseByMonth(year);

            var rsp = new SingleRsp();
            if(data == null)
            {
                rsp.SetError("Not found course");
            }
            else
            {
                rsp.Data = data;
            }
            return rsp;
        }
        public SingleRsp OverviewCourse()
        {
            var data = _courseRep.OverviewCourse();

            var rsp = new SingleRsp();
            if(data == null)
            {
                rsp.SetError("No information");
            }
            else
            {
                rsp.Data = data;
            }
            return rsp;
        }
    }

}
