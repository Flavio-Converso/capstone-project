using capstone_project.Data;
using capstone_project.Helpers;
using capstone_project.Interfaces.Auth;
using capstone_project.Models;
using capstone_project.Models.DTOs.Auth;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _ctx;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ILogger<AuthService> _logger;
        private readonly IImgValidateHelper _imgValidateHelper;
        private readonly IUserHelper _userHelper;

        public AuthService(DataContext dataContext, IPasswordHelper passwordHelper, IUserHelper userHelper, ILogger<AuthService> logger, IImgValidateHelper imgValidateHelper)
        {
            _ctx = dataContext;
            _passwordHelper = passwordHelper;
            _logger = logger;
            _imgValidateHelper = imgValidateHelper;
            _userHelper = userHelper;
        }

        public async Task<User> LoginAsync(UserLoginDTO dto)
        {
            try
            {
                var existingUser = await _ctx.Users
                    .Include(u => u.Roles)
                    .Where(u => u.Username == dto.Username)
                    .FirstOrDefaultAsync();

                if (existingUser == null)
                {
                    _logger.LogWarning("Tentativo di login fallito: username {Username} non trovato.", dto.Username);
                    throw new Exception("Il nome utente non esiste.");
                }

                if (!_passwordHelper.VerifyPassword(dto.Password, existingUser.PasswordHash))
                {
                    _logger.LogWarning("Tentativo di login fallito: password errata per l'username {Username}.", dto.Username);
                    throw new Exception("Password errata.");
                }

                _logger.LogInformation("Login riuscito per l'username: {Username}", dto.Username);
                return existingUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il login per l'username: {Username}", dto.Username);
                throw;
            }
        }

        public async Task<User> RegisterAsync(UserRegisterDTO dto)
        {
            if (dto.PasswordHash != dto.ConfirmPassword)
            {
                _logger.LogWarning("Le password non corrispondono.");
                throw new Exception("Le password non corrispondono.");
            }

            try
            {
                var existingUser = await _ctx.Users
                    .Where(u => u.Username == dto.Username || u.Email == dto.Email || u.PhoneNumber == dto.PhoneNumber)
                    .FirstOrDefaultAsync();

                if (existingUser != null)
                {
                    if (existingUser.Username == dto.Username)
                    {
                        _logger.LogWarning("Tentativo di registrazione con username già in uso: {Username}", dto.Username);
                        throw new Exception("Il nome utente è già in uso.");
                    }
                    if (existingUser.Email == dto.Email)
                    {
                        _logger.LogWarning("Tentativo di registrazione con email già in uso: {Email}", dto.Email);
                        throw new Exception("L'indirizzo email è già in uso.");
                    }
                    if (existingUser.PhoneNumber == dto.PhoneNumber)
                    {
                        _logger.LogWarning("Tentativo di registrazione con numero di telefono già in uso: {PhoneNum}", dto.PhoneNumber);
                        throw new Exception("Il numero di telefono è già in uso.");
                    }
                }

                byte[]? imageBytes = null;

                // If the user selected a predefined image, use that
                if (!string.IsNullOrEmpty(dto.SelectedPredefinedImage))
                {
                    var predefinedImagePath = Path.Combine("wwwroot/images/predefined/", dto.SelectedPredefinedImage);
                    imageBytes = await File.ReadAllBytesAsync(predefinedImagePath);
                }
                else if (dto.Img != null)
                {
                    // Otherwise, handle the uploaded image
                    imageBytes = await _imgValidateHelper.HandleUserImageAsync(dto.Img);
                }

                var selectedCategories = await _ctx.Categories
                    .Where(g => dto.SelectedCategories.Contains(g.CategoryId))
                    .ToListAsync();

                var userRegister = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    PasswordHash = string.Empty,
                    Name = dto.Name,
                    Surname = dto.Surname,
                    BirthDate = dto.BirthDate,
                    Gender = dto.Gender,
                    Country = dto.Country,
                    City = dto.City,
                    Address = dto.Address,
                    ZipCode = dto.ZipCode,
                    PhoneNumber = dto.PhoneNumber,
                    ProfileImg = imageBytes,
                    Categories = selectedCategories,
                };

                userRegister.PasswordHash = _passwordHelper.HashPassword(dto.PasswordHash);

                var userRole = await _ctx.Roles.Where(r => r.RoleId == 1).FirstOrDefaultAsync();
                userRegister.Roles.Add(userRole!);

                await _ctx.Users.AddAsync(userRegister);
                await _ctx.SaveChangesAsync();

                _logger.LogInformation("Registrazione completata con successo per l'username: {Username}", dto.Username);
                return userRegister;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la registrazione per l'username: {Username}", dto.Username);
                throw;
            }
        }


    }
}
