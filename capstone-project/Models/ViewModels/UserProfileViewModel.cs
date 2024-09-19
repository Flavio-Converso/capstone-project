namespace capstone_project.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public byte[]? ProfileImg { get; set; }
        public List<GameKey> OwnedGames { get; set; } = [];
        public int UserCategoriesCount { get; set; }
        public int UserReviewsCount { get; set; }
    }
}
