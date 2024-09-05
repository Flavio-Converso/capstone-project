namespace capstone_project.Helpers
{
    public interface IImgValidateHelper
    {
        bool IsValidImage(IFormFile file, out string errorMessage);
    }
}
