using capstone_project.Models;
using capstone_project.Models.DTOs;

namespace capstone_project.Interfaces
{
    public interface IPegiService
    {
        Task<PegiDTO> CreatePegiAsync(PegiDTO dto);
        Task<Pegi> GetPegiById(int id);
        Task<IEnumerable<Pegi>> GetAllPegiAsync();
        Task<bool> DeletePegiAsync(int id);
        Task<PegiDTO> UpdatePegiAsync(PegiDTO dto);
    }
}
