using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _ctx;

        public CategoryService(DataContext dataContext)
        {
            _ctx = dataContext;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO dto)
        {


            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
            };

            await _ctx.Categories.AddAsync(category);
            await _ctx.SaveChangesAsync();
            return dto;
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await _ctx.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _ctx.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            return category!;
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO dto)
        {


            var category = await _ctx.Categories.FirstOrDefaultAsync(c => c.CategoryId == dto.CategoryId);

            category!.Name = dto.Name;
            category.Description = dto.Description;

            await _ctx.SaveChangesAsync();
            return dto;
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _ctx.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category != null)
            {
                _ctx.Categories.Remove(category);
                await _ctx.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
