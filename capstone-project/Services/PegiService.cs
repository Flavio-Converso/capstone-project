using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class PegiService : IPegiService
    {
        private readonly DataContext _ctx;
        public PegiService(DataContext dataContext)
        {
            _ctx = dataContext;
        }

        public async Task<PegiDTO> CreatePegiAsync(PegiDTO dto)
        {
            if (await _ctx.Pegis.AnyAsync(p => p.Name == dto.Name))
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
            var pegi = new Pegi
            {
                Name = dto.Name,
                Img = imageBytes,
                Description = dto.Description,
            };
            await _ctx.Pegis.AddAsync(pegi);
            await _ctx.SaveChangesAsync();
            return dto;
        }
        public async Task<IEnumerable<Pegi>> GetAllPegiAsync()
        {
            return await _ctx.Pegis.ToListAsync();
        }

        public async Task<Pegi> GetPegiById(int id)
        {
            return await _ctx.Pegis.FirstOrDefaultAsync(p => p.PegiId == id);
        }

        public async Task<PegiDTO> UpdatePegiAsync(PegiDTO dto)
        {
            if (await _ctx.Pegis.AnyAsync(p => p.Name == dto.Name && p.PegiId != dto.PegiId))
            {
                throw new ArgumentException("Il nome specificato è già in uso.");
            }
            var pegi = await _ctx.Pegis.FirstOrDefaultAsync(p => p.PegiId == dto.PegiId);

            pegi!.Name = dto.Name;
            pegi.Description = dto.Description;

            // Se un'immagine nuova è stata caricata, aggiornala
            if (dto.Img != null && dto.Img.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await dto.Img.CopyToAsync(memoryStream);
                    pegi.Img = memoryStream.ToArray();
                }
            }

            await _ctx.SaveChangesAsync();
            return dto;
        }
        public async Task<bool> DeletePegiAsync(int id)
        {
            var pegi = await _ctx.Pegis.FirstOrDefaultAsync(p => p.PegiId == id);
            if (pegi != null)
            {
                _ctx.Pegis.Remove(pegi);
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
