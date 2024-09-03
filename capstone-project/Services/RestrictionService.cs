using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class RestrictionService : IRestrictionService
    {
        private readonly DataContext _ctx;

        public RestrictionService(DataContext dataContext)
        {
            _ctx = dataContext;
        }

        public async Task<RestrictionDTO> CreateRestrictionAsync(RestrictionDTO dto)
        {
            if (await _ctx.Restrictions.AnyAsync(r => r.Name == dto.Name))
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
            await _ctx.Restrictions.AddAsync(restriction);
            await _ctx.SaveChangesAsync();
            return dto;
        }

        public async Task<IEnumerable<Restriction>> GetAllRestrictionAsync()
        {
            var restrictions = await _ctx.Restrictions.ToListAsync();
            return restrictions;
        }

        public async Task<Restriction> GetRestrictionById(int id)
        {
            var restriction = await _ctx.Restrictions.FirstOrDefaultAsync(r => r.RestrictionId == id);
            return restriction!;
        }

        public async Task<RestrictionDTO> UpdateRestrictionAsync(RestrictionDTO dto)
        {
            if (await _ctx.Restrictions.AnyAsync(r => r.Name == dto.Name && r.RestrictionId != dto.RestrictionId))
            {
                throw new ArgumentException("Il nome specificato è già in uso.");
            }

            var restriction = await _ctx.Restrictions.FirstOrDefaultAsync(r => r.RestrictionId == dto.RestrictionId);

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

            await _ctx.SaveChangesAsync();
            return dto;
        }
        public async Task<bool> DeleteRestrictionAsync(int id)
        {
            var restriction = await _ctx.Restrictions.FirstOrDefaultAsync(r => r.RestrictionId == id);
            if (restriction != null)
            {
                _ctx.Restrictions.Remove(restriction);
                await _ctx.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
