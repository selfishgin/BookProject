CREATE TABLE Cart (
    CartId INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,
    BookId INT NULL,
    CourseId INT NULL,
    Quantity INT DEFAULT 1 NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (BookId) REFERENCES Books(BookId) ON DELETE CASCADE,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE
);



CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);



CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY,
    OrderId INT NOT NULL,
    BookId INT NULL,
    CourseId INT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE,
    FOREIGN KEY (BookId) REFERENCES Books(BookId) ON DELETE CASCADE,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE
);



ALTER TABLE Books ADD CONSTRAINT UQ_BookName UNIQUE (BookName);


ALTER TABLE Courses ADD CONSTRAINT UQ_CourseName UNIQUE (CourseName);


INSERT INTO Users (UserFirstname, UserLastname, UserAge, UserSpeciality)
VALUES 
('Alice', 'Johnson', 25, 'Software Engineer'),
('Bob', 'Smith', 30, 'Data Analyst'),
('Charlie', 'Brown', 28, 'Cybersecurity Specialist');


INSERT INTO Books (BookName, BookAuthor, BookGenre, BookDesc, BookPages, BookPrice)
VALUES 
('The Pragmatic Programmer', 'Andrew Hunt', 'Programming', 'A classic book for developers', 352, 39.99),
('Clean Code', 'Robert C. Martin', 'Software Engineering', 'A must-read for clean coding practices', 464, 45.50);


INSERT INTO Courses (CourseName, CourseMentor, CourseReqSkill, CourseDuration, CoursePrice)
VALUES 
('Full Stack Development', 'John Doe', 'HTML, CSS, JavaScript, SQL', 40, 199.99),
('Cybersecurity Basics', 'Jane Smith', 'Networking, Security Fundamentals', 30, 149.99);


