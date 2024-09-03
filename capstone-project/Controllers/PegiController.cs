using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Controllers
{
    public class PegiController : Controller
    {
        private readonly IPegiService _pegiSvc;
        private readonly DataContext _ctx;

        public PegiController(IPegiService pegiService, DataContext dataContext)
        {
            _pegiSvc = pegiService;
            _ctx = dataContext;
        }

        public IActionResult Create()
        {
            return View();
        }

        // GET: /Pegi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PegiDTO dto)
        {
            if (dto.Img != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(dto.Img.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Img", "Sono consentiti solo file JPG e PNG.");
                }

            }
            else
            {
                ModelState.AddModelError("Img", "Devi inserire un'immagine.");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _pegiSvc.CreatePegiAsync(dto);
                return RedirectToAction("List");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: /Pegi
        public async Task<IActionResult> List()
        {
            var pegiList = await _pegiSvc.GetAllPegiAsync();
            return View(pegiList);
        }

        // GET: /Pegi/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var pegi = await _pegiSvc.GetPegiById(id);
            return View(pegi);
        }

        // GET: /Pegi/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var pegi = await _ctx.Pegis.FirstOrDefaultAsync(p => p.PegiId == id);
            var dto = new PegiDTO
            {
                PegiId = pegi!.PegiId,
                Name = pegi.Name,
                Description = pegi.Description,
                ImgByte = pegi.Img,
            };
            return View(dto);
        }
        // POST: /Pegi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PegiDTO dto)
        {
            if (dto.Img != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(dto.Img.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Img", "Sono consentiti solo file JPG e PNG.");

                    ModelState.Remove("ImgByte");
                    dto.ImgByte = (await _pegiSvc.GetPegiById(dto.PegiId))?.Img;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _pegiSvc.UpdatePegiAsync(dto);
                return RedirectToAction("List");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: /Pegi/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var pegi = await _pegiSvc.GetPegiById(id);

            return View(pegi); // Mostra la vista di conferma dell'eliminazione con i dettagli del gioco
        }
        // POST: /Pegi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pegiSvc.DeletePegiAsync(id);

            return RedirectToAction("List"); // Dopo l'eliminazione, ritorna alla lista dei giochi
        }
    }
}
