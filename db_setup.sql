CREATE DATABASE UserManagment;
GO

USE UserManagment;
GO

-- Create the Users table
CREATE TABLE Users
(
	Id VARCHAR(50) NOT NULL,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Age INT NOT NULL,
	Email VARCHAR(50) NOT NULL
)
GO

-- Initial data
INSERT INTO Users (Id, FirstName, LastName, Age, Email)
VALUES
	(NEWID(), 'Harold', 'Lowe', 30, 'hlowe@gmail.com'),
	(NEWID(), 'Nina', 'Gibson', 21, 'ngibson@gmail.com'),
	(NEWID(), 'Michael', 'Brewer', 28, 'mbrewer@gmail.com')

GO
-- Create the stored procedures
CREATE PROCEDURE spGetAllUsers
AS
Begin
	SELECT *
	FROM Users
End
GO

----
CREATE PROCEDURE spGetOneUser 
(
	@Id VARCHAR(50)
)
AS
Begin
	SELECT * FROM Users
	WHERE Id = @Id
End
GO

----
CREATE PROCEDURE spCreateUser
(
	@Id VARCHAR(50),
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@Age INT,
	@Email VARCHAR(30)
)
AS
Begin
	INSERT INTO Users (Id, FirstName, LastName, Age, Email)
	VALUES (@Id, @FirstName, @LastName, @Age, @Email)
End
GO
----
CREATE PROCEDURE spUpdateUser
(
	@Id VARCHAR(50),
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@Age INT,
	@Email VARCHAR(30)
)
AS
Begin
	UPDATE Users
    SET FirstName = @FirstName,
        LastName = @LastName,
        Age = @Age,
        Email = @Email
    WHERE Id = @Id
End
GO
----
CREATE PROCEDURE spDeleteUser
(
	@Id VARCHAR(50)
)
AS
BEGIN
	DELETE FROM Users WHERE Id = @Id
End