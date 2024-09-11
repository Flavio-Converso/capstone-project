using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace capstone_project.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IReviewLikeService _reviewLikeService;
        private readonly DataContext _ctx;

        public ReviewController(IReviewService reviewService, IReviewLikeService reviewLikeService, DataContext dataContext)
        {
            _reviewService = reviewService;
            _reviewLikeService = reviewLikeService;
            _ctx = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewDTO dto, int id)
        {
            // Recupera l'ID dell'utente loggato (es. da User.Identity)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int.TryParse(userIdString, out int userId);
            var result = await _reviewService.CreateReviewAsync(dto, userId);


            if (result != null)
            {
                return RedirectToAction("Details", "Game", new { id = dto.GameId });
            }

            // Se la recensione non è valida, ritorna alla vista dell'evento con un messaggio di errore
            return RedirectToAction("Details", "Event", new { id = dto.GameId });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLike(int id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdString, out int userId);

            var result = await _reviewLikeService.LikeReviewAsync(id, userId);
            if (!result)
            {
                return NotFound(); // O puoi gestire diversamente a seconda della logica del tuo servizio
            }

            // Ottieni il conteggio aggiornato dei like
            var review = await _ctx.Reviews
                .Include(r => r.ReviewLikes)
                .FirstOrDefaultAsync(r => r.ReviewId == id);

            if (review == null)
            {
                return NotFound();
            }

            return Json(new { likeCount = review.ReviewLikes.Count });
        }


        [HttpGet]
        public async Task<IActionResult> GetLikeCount(int id)
        {
            var review = await _ctx.Reviews
                .Include(r => r.ReviewLikes)
                .FirstOrDefaultAsync(r => r.ReviewId == id);

            if (review == null)
            {
                return NotFound();
            }

            return Json(new { likeCount = review.ReviewLikes.Count });
        }

    }
}
