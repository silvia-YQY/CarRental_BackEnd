using CarRentalPlatform.Models;

namespace CarRentalPlatform.Services;

public static class CarService
{
  static List<Car> Cars { get; }
  static int nextId = 3;
  static CarService()
  {
    Cars = new List<Car>
        {
            new Car { Id = 1, Make = "Classic Italian", Model = "aaa", Price_Per_Day = 60, Mileage = 1000, Year = 1999, Url = "http://xxx", Available_Now = false  },
            new Car { Id = 2, Make = "Veggie",Model = "uuu", Price_Per_Day = 70, Mileage = 2000, Year = 2009, Url = "http://xxx", Available_Now = true }
        };
  }

  public static List<Car> GetAll() => Cars;

  public static Car? Get(int id) => Cars.FirstOrDefault(p => p.Id == id);

  public static void Add(Car Car)
  {
    Car.Id = nextId++;
    Cars.Add(Car);
  }

  public static void Delete(int id)
  {
    var Car = Get(id);
    if (Car is null)
      return;

    Cars.Remove(Car);
  }

  public static void Update(Car Car)
  {
    var index = Cars.FindIndex(p => p.Id == Car.Id);
    if (index == -1)
      return;

    Cars[index] = Car;
  }
}