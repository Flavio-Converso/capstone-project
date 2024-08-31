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
            if (await _dataContext.Restrictions.AnyAsync(r => r.Name == dto.Name))
            {
                throw new ArgumentException("Il nome specificato è già in uso.");
            }

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
            if (await _dataContext.Restrictions.AnyAsync(r => r.Name == dto.Name && r.RestrictionId != dto.RestrictionId))
            {
                throw new ArgumentException("Il nome specificato è già in uso.");
            }

            var restriction = await _dataContext.Restrictions.FirstOrDefaultAsync(r => r.RestrictionId == dto.RestrictionId);

            restriction!.Name = dto.Name;
            restriction.Description = dto.Description;

            if (dto.Img != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await dto.Img.CopyToAsync(memoryStream);
                    restriction.Img = memoryStream.ToArray();
                }
            }
            else if (dto.ImgByte != null)
            {
                restriction.Img = dto.ImgByte;
            }

            await _dataContext.SaveChangesAsync();
            return dto;
        }

    }
}
