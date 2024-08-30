using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _dataContext;
        public CategoryService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name,

                Description = dto.Description,
            };
            await _dataContext.Categories.AddAsync(category);
            await _dataContext.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category != null)
            {
                _dataContext.Categories.Remove(category);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _dataContext.Categories.ToListAsync();

        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _dataContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

        }

        public async Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO dto)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == dto.CategoryId);

            if (category == null)
            {
                throw new Exception("Pegi not found");
            }

            category.Name = dto.Name;
            category.Description = dto.Description;

            await _dataContext.SaveChangesAsync();
            return dto;
        }
    }
}
