using capstone_project.Interfaces;

namespace capstone_project.Helpers
{
    public class ImgValidateHelper : IImgValidateHelper
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };

        public bool IsValidImage(IFormFile file, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (file == null)
            {
                errorMessage = "Devi inserire un'immagine.";
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!_allowedExtensions.Contains(extension))
            {
                errorMessage = "Sono consentiti solo file JPG e PNG.";
                return false;
            }

            return true;
        }
        public async Task<byte[]> HandleInvalidImageForPegiEditAsync(IFormFile file, IPegiService pegiService, int pegiId)
        {
            if (!IsValidImage(file, out _))
            {
                var existingPegiImg = (await pegiService.GetPegiById(pegiId))?.Img;
                return existingPegiImg!;
            }
            return null!;
        }

        public async Task<byte[]> HandleInvalidImageForRestrictionEditAsync(IFormFile file, IRestrictionService restrictionService, int restrictionId)
        {
            if (!IsValidImage(file, out _))
            {
                var existingRestrictionImg = (await restrictionService.GetRestrictionById(restrictionId))?.Img;
                return existingRestrictionImg!;
            }
            return null!;
        }
    }
}