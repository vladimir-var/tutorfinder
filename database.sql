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

-- Заповнення таблиці предметів
INSERT INTO subjects (Name) VALUES
('Математика'),
('Фізика'),
('Хімія'),
('Англійська мова'),
('Українська мова'),
('Історія України'),
('Інформатика');

 
-- Додаємо користувачів-репетиторів
INSERT INTO users (Email, PasswordHash, FirstName, LastName, Phone, ProfileImage, Role) VALUES
('math.tutor@email.com',      'hash1', 'Олег',   'Математичний',  '+380501111111', 'https://randomuser.me/api/portraits/men/11.jpg', 'tutor'),
('physics.tutor@email.com',   'hash2', 'Ірина',  'Фізична',       '+380502222222', 'https://randomuser.me/api/portraits/women/12.jpg', 'tutor'),
('chem.tutor@email.com',      'hash3', 'Віктор', 'Хімік',         '+380503333333', 'https://randomuser.me/api/portraits/men/13.jpg', 'tutor'),
('eng.tutor@email.com',       'hash4', 'Олена',  'Англійська',    '+380504444444', 'https://randomuser.me/api/portraits/women/14.jpg', 'tutor'),
('ukr.tutor@email.com',       'hash5', 'Марія',  'Українська',    '+380505555555', 'https://randomuser.me/api/portraits/women/15.jpg', 'tutor'),
('history.tutor@email.com',   'hash6', 'Андрій', 'Історик',       '+380506666666', 'https://randomuser.me/api/portraits/men/16.jpg', 'tutor'),
('it.tutor@email.com',        'hash7', 'Світлана','ІТ',           '+380507777777', 'https://randomuser.me/api/portraits/women/17.jpg', 'tutor');

-- Додаємо репетиторів (userId співпадає з id користувача)
INSERT INTO tutors (UserId, Bio, Education, YearsOfExperience, HourlyRate, TeachingStyle, Certifications, AverageRating, TotalReviews) VALUES
(1, 'Досвідчений викладач математики.', 'КНУ ім. Шевченка', 10, 400, 'online', 'PhD', 4.8, 2),
(2, 'Пояснюю фізику просто.', 'ЛНУ ім. Франка', 8, 350, 'offline', 'Магістр', 4.7, 1),
(3, 'Хімія для життя та ЗНО.', 'ХНУ', 12, 300, 'both', 'Кандидат наук', 5.0, 1),
(4, 'Англійська для дітей та дорослих.', 'КНЕУ', 7, 250, 'online', 'CELTA', 4.9, 2),
(5, 'Українська мова та література.', 'КПІ', 9, 270, 'offline', '', 4.6, 1),
(6, 'Історія України цікаво!', 'ДНУ', 15, 320, 'both', '', 5.0, 1),
(7, 'ІТ та програмування для школярів.', 'ЛНУ', 6, 500, 'online', 'Сертифікат Cisco', 4.8, 1);

-- Прив’язуємо репетиторів до предметів (id предметів: 1-7)
INSERT INTO tutor_subjects (TutorsId, SubjectsId) VALUES
(1, 1), -- Математика
(2, 2), -- Фізика
(3, 3), -- Хімія
(4, 4), -- Англійська мова
(5, 5), -- Українська мова
(6, 6), -- Історія України
(7, 7); -- Інформатика

-- Додаємо учнів для відгуків
INSERT INTO users (Email, PasswordHash, FirstName, LastName, Phone, ProfileImage, Role) VALUES
('student1@email.com', 'hash8', 'Анна', 'Студентка', '+380508888888', 'https://randomuser.me/api/portraits/women/18.jpg', 'student'),
('student2@email.com', 'hash9', 'Дмитро', 'Учень', '+380509999999', 'https://randomuser.me/api/portraits/men/19.jpg', 'student');

-- Додаємо відгуки (рейтинг, коментар)
INSERT INTO reviews (TutorId, StudentId, Rating, Comment) VALUES
(1, 8, 5, 'Дуже доступно пояснює!'),
(1, 9, 4, 'Допоміг підготуватися до ЗНО.'),
(2, 8, 5, 'Фізика стала зрозумілою.'),
(3, 9, 5, 'Хімія більше не страшна!'),
(4, 8, 5, 'Англійська з Оленою — це задоволення!'),
(4, 9, 5, 'Рекомендую для підготовки до іспитів.'),
(5, 8, 4, 'Гарний підхід до учнів.'),
(6, 9, 5, 'Історія стала улюбленим предметом!'),
(7, 8, 5, 'Дуже цікаві уроки з програмування.');
ALTER TABLE users
ALTER COLUMN profileimage TYPE text;