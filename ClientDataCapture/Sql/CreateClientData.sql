/*Setup for mySql Server*/

CREATE DATABASE ClientData;

USE ClientData;

CREATE TABLE Clients (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL UNIQUE,
    DateRegistered DATETIME NOT NULL,
    Location VARCHAR(255),
    NumberOfUsers INT
);