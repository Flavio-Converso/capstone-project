﻿using capstone_project.Models.DTOs;

namespace capstone_project.Interfaces
{
    public interface ICartService
    {
        Task<CartDTO> GetCartByUserIdAsync(int userId);
        Task<CartDTO> AddGameToCartAsync(int userId, int gameId, int quantity);
        Task<bool> RemoveGameFromCartAsync(int userId, int gameId);
        Task<bool> UpdateCartItemQuantityAsync(int userId, int gameId, int newQuantity);
    }
}
