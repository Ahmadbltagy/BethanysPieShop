using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> AllCategories  {
            get{
                return  _context.Categories
                                .OrderBy(p=>p.CategoryName);
            }
        }
    }
}