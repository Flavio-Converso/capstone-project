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
    }
}