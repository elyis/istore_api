using istore_api.src.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace istore_api.src.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public AppDbContext(
            DbContextOptions<AppDbContext> options, 
            IConfiguration config
        ) 
        : base(options)
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<DeviceModel> DeviceModels { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductCharacteristic> ProductCharacteristics { get; set; }
        public DbSet<ProductCharacteristicVariant> ProductCharacteristicVariants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }
        public DbSet<OptionsCombiningCharacteristic> OptionsCombiningCharacteristics { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<BlogTopic> Blogs { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _config.GetConnectionString("Default");
            optionsBuilder.UseSqlite(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DeviceModel>()
                .HasOne(e => e.ProductCategory)
                .WithMany(e => e.DeviceModels)
                .HasForeignKey(e => e.ProductCategoryName);

            modelBuilder.Entity<Product>()
                .HasOne(e => e.DeviceModel)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.DeviceModelName);

            modelBuilder.Entity<OptionsCombiningCharacteristic>().HasKey(e => new {
                e.ImageFilename,
                e.CharacteristicName
            });

            modelBuilder.Entity<ProductCharacteristicVariant>()
                .HasOne(e => e.Characteristic)
                .WithMany(e => e.CharacteristicVariants)
                .HasForeignKey(e => e.CharacteristicName);

            modelBuilder.Entity<ProductCharacteristicVariant>().HasKey(e => new {
                e.ProductId,
                e.CharacteristicName
            });

            modelBuilder.Entity<OrderProducts>().HasKey(e => new
            {
                e.OrderId,
                e.ProductId
            });
        }
    }

}