using AutoMapper;
using CarRentalPlatform.Models;
using CarRentalPlatform.DTOs;

public class MappingProfile : Profile
{
       public MappingProfile()
       {
              CreateMap<CarDto, Car>()
                     .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
              CreateMap<Car, CarDto>()
                     .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

              CreateMap<User, UserDto>();
              CreateMap<User, UserResponseDto>();


              CreateMap<RentalCreateDto, Rental>()
                         .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                         .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                         .ForMember(dest => dest.Car, opt => opt.Ignore())
                         .ForMember(dest => dest.User, opt => opt.Ignore());

              CreateMap<Rental, RentalCreateDto>()
                  // .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                  // .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                  .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.Car.Model))
                  .ForMember(dest => dest.CarMake, opt => opt.MapFrom(src => src.Car.Make))
                  .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));


       }
}
