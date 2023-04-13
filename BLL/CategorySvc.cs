using Common.BLL;
using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Rsp;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BLL
{
    public class CategorySvc : GenericSvc<CategoryRep, Category>
    {
        private readonly CategoryRep _categoryRep = new CategoryRep();
        private readonly CourseRep _courseRep = new CourseRep();
        public SingleRsp AddCategory (string _category_name)
        {
            Category category = new Category
            {
                IdCategory = Guid.NewGuid(),
                Name = _category_name,
            };

            _categoryRep.AddCategory(category);

            var rsp = new SingleRsp();
            rsp.Data = category;
            return rsp;
        }
        public SingleRsp GetAllCategories ()
        {
            var categories = _categoryRep.GetAllCategories();

            var rsp = new SingleRsp();
            rsp.Data = categories;
            return rsp;
        }
        public SingleRsp DeleteCategories(List<Guid> categoryIds)
        {
            List<Course> courses = new List<Course>();
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                foreach(Guid id in categoryIds)
                {
                    foreach(Course course in _courseRep.GetAllCourseByCategoryID(id))
                    {
                        courses.Add(course);
                        _courseRep.DeleteCourseByID(course.IdCourse);
                    }
                    _categoryRep.DeleteCategoryByID(id);
                }
            }

            var rsp = new SingleRsp();
            rsp.Data = courses;
            return rsp;
        }
    }
}
