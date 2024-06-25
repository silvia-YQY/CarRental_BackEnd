# Car Rental System

# start

```
dotnet run
```

open 
`https://localhost:7132;`
or
`http://localhost:5012`

# publish

`dotnet publish -c Release -o ./publish`

# Structure

```
CarRentalPlatform
├── CarRentalPlatform.csproj
├── CarRentalPlatform.sln
├── Controllers
│   ├── AuthController.cs
│   ├── CarControllers.cs
│   ├── RentalControllers.cs
│   └── UserControllers.cs
├── DTOs
│   ├── CarDto.cs
│   ├── RentalCreateDto.cs
│   ├── UserDto.cs
│   ├── UserLoginDto.cs
│   ├── UserRegisterDto.cs
│   └── UserResponseDto.cs
├── Enums
│   └── Status.cs
├── Models
│   ├── Car.cs
│   ├── CarRentalContext.cs
│   ├── Mapper.cs
│   ├── Rental.cs
│   └── User.cs
├── Program.cs
├── Properties
│   └── launchSettings.json
├── README.md
├── Services
│   ├── CarService.cs
│   ├── Interface
│   │   ├── ICarService.cs
│   │   ├── IRentalService.cs
│   │   └── IUserService.cs
│   ├── RentalService.cs
│   └── UserService.cs
├── appsettings.Development.json
├── appsettings.json
├── http
│   ├── Auth.http
│   ├── Car.http
│   ├── Rental.http
│   └── User.http
├── initdb.sql
└── tree.md

```