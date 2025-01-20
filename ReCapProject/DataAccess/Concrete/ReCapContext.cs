using Core;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess
{
    public class ReCapContext : DbContext
    {
        // PostgreSQL için DbContext yapılandırması
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // PostgreSQL bağlantı dizesi
            optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres");
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<CarRental> CarRentals {  get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DriverLicence> DriverLicences { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<BalancePackage> BalancePackages { get; set; }

        public DbSet<UserBalance> UserBalances { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FuelType enum'unu string olarak sakla
            modelBuilder.Entity<Car>()
                .Property(c => c.FuelType)
                .HasConversion(new EnumToStringConverter<FuelType>());

            // Transmission enum'unu string olarak sakla
            modelBuilder.Entity<Car>()
                .Property(c => c.Transmission)
                .HasConversion(new EnumToStringConverter<Transmission>());

            modelBuilder.Entity<CarRental>()
                .Property(cr=>cr.RentalStatus)
                .HasConversion(new EnumToStringConverter<RentalStatus>());

            modelBuilder.Entity<Car>()
                .HasMany(c => c.CarRentals)
                .WithOne(cr => cr.Car)
                .HasForeignKey(cr => cr.CarId);

            modelBuilder.Entity<Car>()
    .HasMany(c => c.CarImages)
    .WithOne(ci => ci.Car)
    .HasForeignKey(ci => ci.CarId);



            modelBuilder.Entity<CarImage>()
    .Property(c => c.Date)
    .HasConversion(
        v => v.ToUniversalTime(),   // UTC'ye çevir
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Veritabanından UTC olarak al
    );

            modelBuilder.Entity<UserOperationClaim>()
       .HasKey(u => new { u.UserId, u.OperationClaimId }); // Kompozit birincil anahtar tanımlaması

            modelBuilder.Entity<Card>()
             .HasOne(c => c.User)          // Card'ın bir User'a bağlı olduğunu belirtiyoruz
             .WithMany()       // Bir User'ın birden fazla Card'ı olabileceğini belirtiyoruz
             .HasForeignKey(c => c.UserId) // Card'daki UserId, User tablosuna dış anahtar olacak
             .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silindiğinde ilişkili kartları da sileriz

         


            modelBuilder.Entity<CarRental>()
             .HasOne(c => c.Card)          // Card'ın bir User'a bağlı olduğunu belirtiyoruz
             .WithMany(c => c.CarRentals)     // Bir User'ın birden fazla Card'ı olabileceğini belirtiyoruz
             .HasForeignKey(c => c.CardId) // Card'daki UserId, User tablosuna dış anahtar olacak
             .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silindiğinde ilişkili kartları da sileriz


            modelBuilder.Entity<Payment>()
           .HasOne<CarRental>()
           .WithMany(c => c.Payments)
           .HasForeignKey(p => p.RentalId)
           .OnDelete(DeleteBehavior.Cascade);

            // Payment -> Car (Bir ödeme bir araca bağlıdır)
            modelBuilder.Entity<Payment>()
                .HasOne<Car>()
                .WithMany()
                .HasForeignKey(p => p.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment -> User (Bir ödeme bir kullanıcıya bağlıdır)
            modelBuilder.Entity<Payment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment -> Card (Bir ödeme bir kart ile yapılır)
            modelBuilder.Entity<Payment>()
                .HasOne<Card>()
                .WithMany()
                .HasForeignKey(p => p.CardId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }
}
