-- 创建租车平台数据库
CREATE DATABASE IF NOT EXISTS carrentaldb;
USE carrentaldb;

-- 创建Car表
CREATE TABLE Cars (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Make VARCHAR(255) NOT NULL,
    Model VARCHAR(255) NOT NULL,
    Year DECIMAL(4,0) NOT NULL,
    Mileage DECIMAL(10,2) NOT NULL,
    Available_Now BOOLEAN NOT NULL,
    Url VARCHAR(1000) NOT NULL,
    Price_Per_Day DECIMAL(10,2) NOT NULL
);

-- 创建User表
CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    isAdmin BOOLEAN NOT NULL,
    Email VARCHAR(255) NOT NULL
);

-- 创建Rental表
CREATE TABLE Rentals (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CarId INT NOT NULL,
    UserId INT NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    Fee DECIMAL(10,2) NOT NULL,
    Status ENUM ('Confirm', 'Done', 'Cancel', 'Pending') NOT NULL,
    FOREIGN KEY (CarId) REFERENCES Cars(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
