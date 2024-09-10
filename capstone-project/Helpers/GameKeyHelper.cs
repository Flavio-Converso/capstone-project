using capstone_project.Data;

namespace capstone_project.Helpers
{
    public class GameKeyHelper : IGameKeyHelper
    {
        private readonly DataContext _ctx;

        public GameKeyHelper(DataContext context)
        {
            _ctx = context;
        }

        public string GenerateUniqueKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            string key;
            do
            {
                key = new string(Enumerable.Repeat(chars, 20)
                                           .Select(s => s[random.Next(s.Length)])
                                           .ToArray());
            }
            while (_ctx.GameKeys.Any(gk => gk.KeyNum == key));  // Ensure the key is unique

            return key;
        }
    }
}
