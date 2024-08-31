using capstone_project.Models;
using capstone_project.Models.DTOs;

namespace capstone_project.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO dto);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryById(int id);
        Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
