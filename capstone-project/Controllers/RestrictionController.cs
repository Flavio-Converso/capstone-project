using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Controllers
{
    public class RestrictionController : Controller
    {
        private readonly IRestrictionService _restrictionService;
        private readonly DataContext _dataContext;

        public RestrictionController(IRestrictionService restrictionService, DataContext dataContext)
        {
            _restrictionService = restrictionService;
            _dataContext = dataContext;
        }

        public IActionResult Create()
        {
            return View();
        }

        // GET: /Restriction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RestrictionDTO dto)
        {
            await _restrictionService.CreateRestrictionAsync(dto);
            return RedirectToAction("List");
        }

        // GET: /Restriction
        public async Task<IActionResult> List()
        {
            var restrictionList = await _restrictionService.GetAllRestrictionAsync();
            return View(restrictionList);
        }

        // GET: /Restriction/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var restriction = await _restrictionService.GetRestrictionById(id);
            return View(restriction);
        }

        // GET: /Restriction/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var restriction = await _dataContext.Restrictions.FirstOrDefaultAsync(p => p.RestrictionId == id);
            var dto = new RestrictionDTO
            {
                RestrictionId = restriction.RestrictionId,
                Name = restriction.Name,
                Description = restriction.Description,
                ImgByte = restriction.Img,
            };
            return View(dto);
        }

        // POST: /Restriction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RestrictionDTO dTO)
        {
            await _restrictionService.UpdateRestrictionAsync(dTO);
            return RedirectToAction("List");
        }

        // GET: /Restriction/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var restriction = await _restrictionService.GetRestrictionById(id);

            return View(restriction); // Mostra la vista di conferma dell'eliminazione con i dettagli del gioco
        }
        // POST: /Restriction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _restrictionService.DeleteRestrictionAsync(id);

            return RedirectToAction("List"); // Dopo l'eliminazione, ritorna alla lista dei giochi
        }
    }
}
