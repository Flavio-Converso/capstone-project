using capstone_project.Data;
using capstone_project.Helpers;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _ctx;
        private readonly IImgValidateHelper _imgValidateHelper;
        private readonly IUserHelper _userHelper;
        private readonly IPasswordHelper _passwordHelper;

        public UserService(DataContext dataContext, IUserHelper userHelper, IImgValidateHelper imgValidateHelper, IPasswordHelper passwordHelper)
        {
            _ctx = dataContext;
            _userHelper = userHelper;
            _imgValidateHelper = imgValidateHelper;
            _passwordHelper = passwordHelper;
        }


        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _ctx.Users.FindAsync(id);
            _ctx.Users.Remove(user!);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _ctx.Users.Include(u => u.Roles).ToListAsync();
        }


        public async Task<UserEditProfileViewModel> GetProfileForEditAsync(int userId)
        {
            var user = await _ctx.Users.FindAsync(userId);

            return new UserEditProfileViewModel
            {
                Username = user!.Username,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Gender = user.Gender.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Country = user.Country,
                City = user.City,
                Address = user.Address,
                ZipCode = user.ZipCode,
                PhoneNumber = user.PhoneNumber,
                ProfileImg = user.ProfileImg
            };
        }

        public async Task UpdateProfileAsync(int userId, UserEditProfileViewModel viewModel)
        {
            var user = await _userHelper.GetUserIdAsync(userId);

            var existingUserWithPhoneNumber = await _ctx.Users
                .Where(u => u.PhoneNumber == viewModel.PhoneNumber && u.UserId != userId)
                .FirstOrDefaultAsync();

            if (existingUserWithPhoneNumber != null)
            {
                throw new Exception("Il numero di telefono è già in uso.");
            }

            user.Name = viewModel.Name;
            user.Surname = viewModel.Surname;
            user.Country = viewModel.Country;
            user.City = viewModel.City;
            user.Address = viewModel.Address;
            user.ZipCode = viewModel.ZipCode;
            user.PhoneNumber = viewModel.PhoneNumber;

            if (!string.IsNullOrEmpty(viewModel.SelectedPredefinedImage))
            {
                var predefinedImagePath = Path.Combine("wwwroot/images/predefined/", viewModel.SelectedPredefinedImage);
                user.ProfileImg = await File.ReadAllBytesAsync(predefinedImagePath);
            }
            else if (viewModel.NewProfileImg != null)
            {
                user.ProfileImg = await _imgValidateHelper.HandleUserImageAsync(viewModel.NewProfileImg);
            }

            _ctx.Users.Update(user);
            await _ctx.SaveChangesAsync();
        }
        public async Task ChangePasswordAsync(int userId, ChangePasswordViewModel viewModel)
        {
            var user = await _userHelper.GetUserIdAsync(userId);

            if (!_passwordHelper.VerifyPassword(viewModel.OldPassword, user.PasswordHash))
            {
                throw new Exception("La password attuale è sbagliata.");
            }

            user.PasswordHash = _passwordHelper.HashPassword(viewModel.NewPassword);

            _ctx.Users.Update(user);
            await _ctx.SaveChangesAsync();
        }

    }
}
