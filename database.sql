-- Создание базы данных
CREATE DATABASE tutorfinder;

-- Подключение к базе данных
\c tutorfinder;

-- Создание таблиц
CREATE TABLE users (
    Id SERIAL PRIMARY KEY,
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Phone VARCHAR(20),
    ProfileImage VARCHAR(255),
    Role VARCHAR(20) NOT NULL CHECK (Role IN ('student', 'tutor'))
);

CREATE TABLE tutors (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER UNIQUE NOT NULL REFERENCES users(Id) ON DELETE CASCADE,
    Bio TEXT NOT NULL,
    Education VARCHAR(200) NOT NULL,
    YearsOfExperience INTEGER NOT NULL,
    HourlyRate DECIMAL(10,2) NOT NULL CHECK (HourlyRate >= 0 AND HourlyRate <= 10000)
    TeachingStyle TEXT,
    Certifications TEXT,
    AverageRating DECIMAL(3,2) DEFAULT 0,
    TotalReviews INTEGER DEFAULT 0
);

CREATE TABLE subjects (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT
);

CREATE TABLE tutor_subjects (
    TutorsId INTEGER NOT NULL REFERENCES tutors(Id) ON DELETE CASCADE,
    SubjectsId INTEGER NOT NULL REFERENCES subjects(Id) ON DELETE CASCADE,
    PRIMARY KEY (TutorsId, SubjectsId)
);

CREATE TABLE reviews (
    Id SERIAL PRIMARY KEY,
    TutorId INTEGER NOT NULL REFERENCES tutors(Id) ON DELETE CASCADE,
    StudentId INTEGER NOT NULL REFERENCES users(Id) ON DELETE CASCADE,
    Rating INTEGER NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
    Comment TEXT
);

-- Создание индексов
CREATE INDEX idx_tutors_user_id ON tutors(UserId);
CREATE INDEX idx_reviews_tutor_id ON reviews(TutorId);
CREATE INDEX idx_reviews_student_id ON reviews(StudentId);
CREATE INDEX idx_subjects_category ON subjects(Category);

 
