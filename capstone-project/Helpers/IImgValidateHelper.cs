using capstone_project.Interfaces;

namespace capstone_project.Helpers
{
    public interface IImgValidateHelper
    {
        bool IsValidImage(IFormFile file, out string errorMessage);
        Task<byte[]> HandleInvalidImageForPegiEditAsync(IFormFile file, IPegiService pegiService, int pegiId);
        Task<byte[]> HandleInvalidImageForRestrictionEditAsync(IFormFile file, IRestrictionService restrictionService, int restrictionId);
    }
}
