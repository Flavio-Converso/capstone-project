using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace capstone_project.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly DataContext _dataContext;

        public CategoryController(ICategoryService categoryService, DataContext dataContext)
        {
            _categoryService = categoryService;
            _dataContext = dataContext;
        }

        public IActionResult Create()
        {
            return View();
        }

        // GET: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDTO dto)
        {
            await _categoryService.CreateCategoryAsync(dto);
            return RedirectToAction("List");
        }

        // GET: /Category
        public async Task<IActionResult> List()
        {
            var categoryList = await _categoryService.GetAllCategoriesAsync();
            return View(categoryList);
        }

        // GET: /Category/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return View(category);
        }

        // GET: /Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = _dataContext.Categories.FirstOrDefault(c => c.CategoryId == id);
            var dto = new CategoryDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
            };
            return View(dto);
        }

        // POST: /Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDTO dTO)
        {
            await _categoryService.UpdateCategoryAsync(dTO);
            return RedirectToAction("List");
        }

        // GET: /Category/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryById(id);

            return View(category); // Mostra la vista di conferma dell'eliminazione con i dettagli del gioco
        }
        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);

            return RedirectToAction("List"); // Dopo l'eliminazione, ritorna alla lista dei giochi
        }
    }
}
