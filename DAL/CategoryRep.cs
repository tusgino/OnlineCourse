using Common.DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CategoryRep : GenericRep<WebsiteKhoaHocOnline_V4Context, Category>
    {
        public void AddCategory(Category category)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }
        public List<Category> GetAllCategories()
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.Categories.ToList();
            }
        }
        public void DeleteCategoryByID(Guid _category_id)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var category = context.Categories.FirstOrDefault(category => category.IdCategory == _category_id);
                context.Categories.Remove(category);
                context.SaveChanges();
                //return category.Courses;
            }
        }
    }
}
