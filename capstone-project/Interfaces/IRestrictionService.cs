using capstone_project.Models;
using capstone_project.Models.DTOs;

namespace capstone_project.Interfaces
{
    public interface IRestrictionService
    {
        Task<RestrictionDTO> CreateRestrictionAsync(RestrictionDTO dto);
        Task<Restriction> GetRestrictionById(int id);
        Task<IEnumerable<Restriction>> GetAllRestrictionAsync();
        Task<bool> DeleteRestrictionAsync(int id);
        Task<RestrictionDTO> UpdateRestrictionAsync(RestrictionDTO dto);
    }
}
