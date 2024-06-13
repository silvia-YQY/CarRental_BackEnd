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

      modelBuilder.Entity<Car>()
          .HasMany(c => c.Rentals)
          .WithOne(r => r.Car)
          .HasForeignKey(r => r.Car_Id);

      modelBuilder.Entity<User>()
          .HasMany(u => u.Rentals)
          .WithOne(r => r.User)
          .HasForeignKey(r => r.User_Id);

      base.OnModelCreating(modelBuilder);
    }


  }

}
