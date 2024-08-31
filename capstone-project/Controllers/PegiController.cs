﻿using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Controllers
{
    public class PegiController : Controller
    {
        private readonly IPegiService _pegiService;
        private readonly DataContext _dataContext;

        public PegiController(IPegiService pegiService, DataContext dataContext)
        {
            _pegiService = pegiService;
            _dataContext = dataContext;
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
                await _pegiService.CreatePegiAsync(dto);
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
            var pegiList = await _pegiService.GetAllPegiAsync();
            return View(pegiList);
        }

        // GET: /Pegi/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var pegi = await _pegiService.GetPegiById(id);
            return View(pegi);
        }

        // GET: /Pegi/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var pegi = await _dataContext.Pegis.FirstOrDefaultAsync(p => p.PegiId == id);
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
                }
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _pegiService.UpdatePegiAsync(dto);
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
            var pegi = await _pegiService.GetPegiById(id);

            return View(pegi); // Mostra la vista di conferma dell'eliminazione con i dettagli del gioco
        }
        // POST: /Pegi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _pegiService.DeletePegiAsync(id);

            return RedirectToAction("List"); // Dopo l'eliminazione, ritorna alla lista dei giochi
        }
    }
}
