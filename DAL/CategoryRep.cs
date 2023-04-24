using Common.DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CategoryRep : GenericRep<WebsiteKhoaHocOnline_V4Context, Category>
    {
        public bool AddCategory(Category _category)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                foreach(Category category in context.Categories)
                {
                    if(category.Name == _category.Name)
                    {
                        return false;
                    }
                }
                context.Categories.Add(_category);
                context.SaveChanges();
                return true;
            }
        }
        public List<Category> GetAllCategories(string? _title_like)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {

                return context.Categories.Where(category => category.Name.Contains(_title_like == null ? "" : _title_like)).ToList();
            }
        }
        public void DeleteCategoryByID(Guid _category_id)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var category = context.Categories.FirstOrDefault(category => category.IdCategory == _category_id);
                if (category != null)
                {
                    context.Categories.Remove(category); 
                    context.SaveChanges();
                }

                //return category.Courses;
            }
        }
    }
}
