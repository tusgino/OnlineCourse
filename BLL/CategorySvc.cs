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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

            bool checkValid = _categoryRep.AddCategory(category);

            var rsp = new SingleRsp();
            if (checkValid)
            {
                rsp.Data = category;
            } 
            else
            {
                rsp.SetMessage("Trung");
            }
            return rsp;
        }
        public SingleRsp GetAllCategories (string? _title_like, int page)
        {
            var categories = _categoryRep.GetAllCategories(_title_like);

            int limit = 10;
            int offset = (page - 1) * limit;
            int total = categories.Count;
            int totalPages = (total % limit == 0) ? total / limit : 1 + total / limit;

            var data = categories.OrderBy(category => category.Name).Skip(offset).Take(limit).ToList();

            object res = new
            {
                _data = data,
                _totalRows = total,
            };

            var rsp = new SingleRsp();

            if (data == null)
            {
                rsp.SetError("Not found category");
            }
            else
            {
                rsp.Data = res;
            }

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
                        JsonPatchDocument newCourse = new JsonPatchDocument();
                        newCourse.Replace("/IdCategory", null);

                        _courseRep.UpdateCourse(course.IdCourse, newCourse);
                        courses.Add(course);
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
