using capstone_project.Interfaces;
using capstone_project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace capstone_project.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categorySvc;

        public CategoryController(ICategoryService categoryService)
        {
            _categorySvc = categoryService;
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _categorySvc.CreateCategoryAsync(dto);
                return RedirectToAction("List");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(dto);
            }
        }

        // GET: /Category
        public async Task<IActionResult> List()
        {
            var categoryList = await _categorySvc.GetAllCategoriesAsync();
            return View(categoryList);
        }

        // GET: /Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categorySvc.GetCategoryById(id);
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
        public async Task<IActionResult> Edit(CategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _categorySvc.UpdateCategoryAsync(dto);
                return RedirectToAction("List");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(dto);
            }
        }

        // GET: /Category/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categorySvc.GetCategoryById(id);
            return View(category); // Show the delete confirmation view with category details
        }
        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categorySvc.DeleteCategoryAsync(id);
            return RedirectToAction("List"); // Return to the list after deletion
        }
    }
}
