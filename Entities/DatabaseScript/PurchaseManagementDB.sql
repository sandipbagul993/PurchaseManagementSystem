Create Database PurchaseManagementDB

use PurchaseManagementDB

drop Database PurchaseManagementDB

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL
);

CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY,
    UserId NVARCHAR(450) NOT NULL, 
    OrderDate DATETIME NOT NULL  ,
	Total decimal Not Null,
	CONSTRAINT FK_Orders_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

ALTER TABLE Orders
ADD CONSTRAINT FK_Orders_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id);


CREATE TABLE OrderItems (
    Id INT PRIMARY KEY IDENTITY,
    OrderId INT FOREIGN KEY REFERENCES Orders(Id),
    ProductId INT FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL
);





Select * from  AspNetUsers
select  * from  Products
select * from  Orders
Select * from  OrderItems




