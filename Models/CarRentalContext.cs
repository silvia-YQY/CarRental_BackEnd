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
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Rental>(entity =>
      {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.StartDate).IsRequired();
        entity.Property(e => e.EndDate).IsRequired();
        entity.Property(e => e.Fee).IsRequired();
        entity.Property(e => e.Status).IsRequired()
                                      .IsRequired()
                                      .HasConversion<int>(); // 添加枚举到整数的转换


        entity.HasOne(e => e.Car)
                .WithMany(c => c.Rentals)
                .HasForeignKey(e => e.CarId)
                .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
      });
    }

  }

}
