using capstone_project.Helpers;
using capstone_project.Interfaces;
using capstone_project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace capstone_project.Controllers
{
    public class RestrictionController : Controller
    {
        private readonly IRestrictionService _restrictionSvc;
        private readonly IImgValidateHelper _imgValidateHelper;

        public RestrictionController(IRestrictionService restrictionService, IImgValidateHelper imgValidateHelper)
        {
            _restrictionSvc = restrictionService;
            _imgValidateHelper = imgValidateHelper;
        }

        public IActionResult Create()
        {
            return View();
        }
        // POST: /Restriction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RestrictionDTO dto)
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
                await _restrictionSvc.CreateRestrictionAsync(dto);
                return RedirectToAction("List");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: /Restriction
        public async Task<IActionResult> List()
        {
            var restrictionList = await _restrictionSvc.GetAllRestrictionAsync();
            return View(restrictionList);
        }

        // GET: /Restriction/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var restriction = await _restrictionSvc.GetRestrictionById(id);
            return View(restriction);
        }

        // GET: /Restriction/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var restriction = await _restrictionSvc.GetRestrictionById(id);
            var dto = new RestrictionDTO
            {
                RestrictionId = restriction!.RestrictionId,
                Name = restriction.Name,
                Description = restriction.Description,
                ImgByte = restriction.Img,
            };
            return View(dto);
        }
        // POST: /Restriction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RestrictionDTO dto)
        {
            if (dto.Img != null && !_imgValidateHelper.IsValidImage(dto.Img, out string errorMessage))
            {
                ModelState.AddModelError("Img", errorMessage);
                ModelState.Remove("ImgByte");

                dto.ImgByte = await _imgValidateHelper.HandleInvalidImageForRestrictionEditAsync(dto.Img, _restrictionSvc, dto.RestrictionId);
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _restrictionSvc.UpdateRestrictionAsync(dto);
                return RedirectToAction("List");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: /Restriction/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var restriction = await _restrictionSvc.GetRestrictionById(id);

            return View(restriction);
        }
        // POST: /Restriction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _restrictionSvc.DeleteRestrictionAsync(id);

            return RedirectToAction("List");
        }
    }
}
