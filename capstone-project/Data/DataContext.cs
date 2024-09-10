using capstone_project.Models;
using Microsoft.EntityFrameworkCore;

namespace capstone_project.Data
{
    public class DataContext : DbContext
    {

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameImage> GameImages { get; set; }
        public virtual DbSet<GameKey> GameKeys { get; set; }
        public virtual DbSet<Pegi> Pegis { get; set; }
        public virtual DbSet<Restriction> Restrictions { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }
        public virtual DbSet<WishlistItem> WishlistItems { get; set; }
        public virtual DbSet<ReviewLike> ReviewLikes { get; set; }
        public virtual DbSet<OrderSummary> OrderSummaries { get; set; }

        public DataContext(DbContextOptions<DataContext> opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura le relazioni con eliminazione a cascata
            modelBuilder.Entity<ReviewLike>()
            .HasOne(rl => rl.Review)
            .WithMany(r => r.ReviewLikes) // Configura la relazione inversa
            .HasForeignKey(rl => rl.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewLike>()
                .HasOne(rl => rl.User)
                .WithMany()
                .HasForeignKey(rl => rl.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
