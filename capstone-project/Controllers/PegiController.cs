using capstone_project.Helpers;
using capstone_project.Interfaces;
using capstone_project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace capstone_project.Controllers
{
    public class PegiController : Controller
    {
        private readonly IPegiService _pegiSvc;
        private readonly IImgValidateHelper _imgValidateHelper;

        public PegiController(IPegiService pegiService, IImgValidateHelper imgValidateHelper)
        {
            _pegiSvc = pegiService;
            _imgValidateHelper = imgValidateHelper;
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
            if (!_imgValidateHelper.IsValidImage(dto.Img!, out string errorMessage))
            {
                ModelState.AddModelError("Img", errorMessage);
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

            var pegi = await _pegiSvc.GetPegiById(id);
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
            if (dto.Img != null && !_imgValidateHelper.IsValidImage(dto.Img, out string errorMessage))
            {
                ModelState.AddModelError("Img", errorMessage);
                ModelState.Remove("ImgByte");

                dto.ImgByte = await _imgValidateHelper.HandleInvalidImageForPegiEditAsync(dto.Img, _pegiSvc, dto.PegiId);
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

            return View(pegi);
        }
        // POST: /Pegi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pegiSvc.DeletePegiAsync(id);

            return RedirectToAction("List");
        }
    }
}
