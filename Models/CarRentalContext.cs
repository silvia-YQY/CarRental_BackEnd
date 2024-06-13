using Microsoft.EntityFrameworkCore;

namespace CarRentalPlatform.Models
{
  public class CarRentalContext : DbContext
  {
    public CarRentalContext(DbContextOptions<CarRentalContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Rental> Rentals { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Car>().ToTable("cars");
      modelBuilder.Entity<Rental>().ToTable("rentals");
      modelBuilder.Entity<User>().ToTable("users");

      modelBuilder.Entity<Rental>()
          .HasOne(r => r.Car)         // 一个 Rental 拥有一个 Car
          .WithMany()                 // 一个 Car 可能被多个 Rental 引用
          .HasForeignKey(r => r.Car_Id);  // Rental 表中的 Car_Id 外键

      modelBuilder.Entity<Rental>()
          .HasOne(r => r.User)        // 一个 Rental 拥有一个 User
          .WithMany()                 // 一个 User 可能拥有多个 Rental
          .HasForeignKey(r => r.User_Id); // Rental 表中的 User_Id 外键

      base.OnModelCreating(modelBuilder);
    }


  }

}
