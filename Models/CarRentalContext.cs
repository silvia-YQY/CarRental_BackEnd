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

  }

}
