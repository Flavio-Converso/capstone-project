using capstone_project.Data;
using capstone_project.Interfaces;
using capstone_project.Models;
using capstone_project.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Services
{
    public class PegiService : IPegiService
    {
        private readonly DataContext _dataContext;
        public PegiService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<PegiDTO> CreatePegiAsync(PegiDTO dto)
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
            var pegi = new Pegi
            {
                Name = dto.Name,
                Img = imageBytes,
                Description = dto.Description,
            };
            await _dataContext.Pegis.AddAsync(pegi);
            await _dataContext.SaveChangesAsync();
            return dto;
        }
        public async Task<IEnumerable<Pegi>> GetAllPegiAsync()
        {
            return await _dataContext.Pegis.ToListAsync();
        }

        public async Task<Pegi> GetPegiById(int id)
        {
            return await _dataContext.Pegis.FirstOrDefaultAsync(p => p.PegiId == id);

        }

        public async Task<bool> DeletePegiAsync(int id)
        {
            var pegi = await _dataContext.Pegis.FirstOrDefaultAsync(p => p.PegiId == id);
            if (pegi != null)
            {
                _dataContext.Pegis.Remove(pegi);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<PegiDTO> UpdatePegiAsync(PegiDTO dto)
        {
            var pegi = await _dataContext.Pegis.FirstOrDefaultAsync(p => p.PegiId == dto.PegiId);

            if (pegi == null)
            {
                throw new Exception("Pegi not found");
            }

            pegi.Name = dto.Name;
            pegi.Description = dto.Description;

            // Se un'immagine nuova è stata caricata, aggiornala
            if (dto.Img != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await dto.Img.CopyToAsync(memoryStream);
                    pegi.Img = memoryStream.ToArray();
                }
            }
            else
            {
                // Se nessuna nuova immagine è stata fornita, mantieni l'immagine esistente
                pegi.Img = dto.ImgByte;
            }

            await _dataContext.SaveChangesAsync();
            return dto;
        }

    }
}
