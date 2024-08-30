using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class RestrictionService : IRestrictionService
    {
        private readonly DataContext _dataContext;

        public RestrictionService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<RestrictionDTO> CreateRestrictionAsync(RestrictionDTO dto)
        {
            byte[]? imageBytes = null;
            if (dto.Img != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await dto.Img.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }
            var restriction = new Restriction
            {
                Name = dto.Name,
                Img = imageBytes,
                Description = dto.Description,
            };
            await _dataContext.Restrictions.AddAsync(restriction);
            await _dataContext.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteRestrictionAsync(int id)
        {
            var restriction = await _dataContext.Restrictions.FirstOrDefaultAsync(r => r.RestrictionId == id);
            if (restriction != null)
            {
                _dataContext.Restrictions.Remove(restriction);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<Restriction>> GetAllRestrictionAsync()
        {
            return await _dataContext.Restrictions.ToListAsync();
        }

        public async Task<Restriction> GetRestrictionById(int id)
        {
            return await _dataContext.Restrictions.FirstOrDefaultAsync(r => r.RestrictionId == id);
        }

        public async Task<RestrictionDTO> UpdateRestrictionAsync(RestrictionDTO dto)
        {
            var restriction = await _dataContext.Restrictions.FirstOrDefaultAsync(r => r.RestrictionId == dto.RestrictionId);

            if (restriction == null)
            {
                throw new Exception("Restriction not found");
            }

            restriction.Name = dto.Name;
            restriction.Description = dto.Description;

            // Se un'immagine nuova è stata caricata, aggiornala
            if (dto.Img != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await dto.Img.CopyToAsync(memoryStream);
                    restriction.Img = memoryStream.ToArray();
                }
            }
            else
            {
                if (dto.ImgByte != null)
                {
                    restriction.Img = dto.ImgByte;
                }
            }

            await _dataContext.SaveChangesAsync();
            return dto;
        }
    }
}
