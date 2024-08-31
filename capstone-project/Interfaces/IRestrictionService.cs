using capstone_project.Models;
using capstone_project.Models.DTOs;

namespace capstone_project.Interfaces
{
    public interface IRestrictionService
    {
        Task<RestrictionDTO> CreateRestrictionAsync(RestrictionDTO dto);
        Task<IEnumerable<Restriction>> GetAllRestrictionAsync();
        Task<Restriction> GetRestrictionById(int id);
        Task<RestrictionDTO> UpdateRestrictionAsync(RestrictionDTO dto);
        Task<bool> DeleteRestrictionAsync(int id);
    }
}
