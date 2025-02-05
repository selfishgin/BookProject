using Microsoft.EntityFrameworkCore;

namespace BookProject.Models
{
    public class BookProjectDbContext : DbContext
    {
        public BookProjectDbContext(DbContextOptions<BookProjectDbContext> options) : base(options) { }

        // DbSet properties for tables
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // Configure entity mappings
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserFirstname).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UserLastname).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UserSpeciality).HasMaxLength(255);
                entity.Property(e => e.UserProfilePicturePath).HasColumnType("varbinary(max)");
            });

            // Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.BookName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.BookAuthor).IsRequired().HasMaxLength(100);
                entity.Property(e => e.BookGenre).HasMaxLength(100);
                entity.Property(e => e.BookDesc).HasMaxLength(255);
                entity.Property(e => e.BookPrice).IsRequired().HasColumnType("decimal(6,2)");
                entity.Property(e => e.BookPicturePath).HasColumnType("varbinary(max)");
            });

            // Course entity
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId);
                entity.Property(e => e.CourseName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CourseMentor).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CourseReqSkill).HasMaxLength(255);
                entity.Property(e => e.CoursePrice).IsRequired().HasColumnType("decimal(6,2)");
                entity.Property(e => e.CoursePicturePath).HasColumnType("varbinary(max)");
            });

            // Cart entity
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.CartId);
                entity.Property(e => e.Quantity).IsRequired().HasDefaultValue(1);

                // Relationships
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Book)
                      .WithMany()
                      .HasForeignKey(e => e.BookId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Course)
                      .WithMany()
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.OrderDate).IsRequired();

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem entity
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemId);
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(6,2)");

                entity.HasOne(e => e.Order)
                      .WithMany()
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Book)
                      .WithMany()
                      .HasForeignKey(e => e.BookId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Course)
                      .WithMany()
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }

        // Fallback for OnConfiguring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=YOUR_SERVER_NAME;Database=BookProject;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }
    }
}
