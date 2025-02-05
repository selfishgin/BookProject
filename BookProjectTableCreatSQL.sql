Create Database BookProject
GO

Create Table Users(
	UserId int primary key identity,
	UserProfilePicturePath VARBINARY(MAX),
	UserFirstname NVARCHAR(100),
	UserLastname NVARCHAR(100),
	UserAge INT,
	UserSpeciality NVARCHAR(255)
);

Create Table Books(
	BookId int primary key identity, 
	BookPicturePath VARBINARY(MAX),
	BookName NVARCHAR(100),
	BookAuthor NVARCHAR(100),
	BookGenre NVARCHAR(100),
	BookDesc NVARCHAR(100),
	BookPages INT,
	BookReadCount INT,
	BookPrice DECIMAL(6,2)
);

Create Table Courses(
	CourseId int primary key identity,
	CoursePicturePath VARBINARY(MAX),
	CourseName NVARCHAR(100),
	CourseMentor NVARCHAR(100),
	CourseReqSkill NVARCHAR(255),
	CourseDuration INT, 
	CoursePrice DECIMAL(6,2),
);



