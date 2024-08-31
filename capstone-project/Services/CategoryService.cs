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
            // Check if a category with the same name already exists
            if (await _ctx.Categories.AnyAsync(c => c.Name == dto.Name))
            {
                throw new ArgumentException("Il nome specificato è già in uso.");
            }

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
            return await _ctx.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _ctx.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO dto)
        {
            // Check if another category with the same name exists (ignoring the current category being updated)
            if (await _ctx.Categories.AnyAsync(c => c.Name == dto.Name && c.CategoryId != dto.CategoryId))
            {
                throw new ArgumentException("Il nome specificato è già in uso.");
            }

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
