document.addEventListener('DOMContentLoaded', async () => {
    // Отримуємо ID репетитора з URL
    const urlParams = new URLSearchParams(window.location.search);
    const tutorId = urlParams.get('id');

    if (!tutorId) {
        alert('ID репетитора не вказано');
        window.location.href = '../search/search.html';
        return;
    }

    try {
        // Отримуємо дані профілю репетитора
        const tutorData = await apiClient.getTutorProfile(tutorId);
        if (!tutorData) {
            throw new Error('Не вдалося отримати дані профілю');
        }

        // Заповнюємо основну інформацію
        document.getElementById('tutorName').textContent = `${tutorData.firstName || ''} ${tutorData.lastName || ''}`.trim() || 'Не вказано';
        document.getElementById('tutorSubject').textContent = `Репетитор з ${tutorData.subjects && tutorData.subjects.length > 0 ? tutorData.subjects.map(s => s.name).join(', ') : 'Не вказано'}`;
        document.getElementById('tutorLocation').textContent = tutorData.location || 'Не вказано';
        document.getElementById('tutorEducation').textContent = tutorData.education || 'Не вказано';
        document.getElementById('tutorExperience').textContent = (tutorData.yearsOfExperience !== undefined && tutorData.yearsOfExperience !== null) ? tutorData.yearsOfExperience + ' років' : 'Не вказано';
        document.getElementById('tutorPrice').textContent = (tutorData.hourlyRate !== undefined && tutorData.hourlyRate !== null) ? tutorData.hourlyRate + ' грн/год' : 'Не вказано';
        document.getElementById('tutorAbout').textContent = tutorData.bio || tutorData.about || 'Інформація відсутня';

        // Оновлюємо рейтинг
        const ratingElement = document.getElementById('tutorRating');
        const rating = tutorData.averageRating || tutorData.rating || 0;
        const reviewsCount = tutorData.reviewsCount || (tutorData.reviews ? tutorData.reviews.length : 0) || 0;
        const ratingStars = Math.round(rating);
        ratingElement.innerHTML = `
            ${Array(5).fill().map((_, i) => 
                `<i class="fas fa-star${i < ratingStars ? '' : '-o'} text-warning"></i>`
            ).join('')}
            <span class="ms-1">${rating.toFixed(1)}</span>
            <span class="text-muted ms-2">(${reviewsCount} відгуків)</span>
        `;

        // Заповнюємо предмети як список
        const subjectsContainer = document.getElementById('tutorSubjects');
        if (tutorData.subjects && tutorData.subjects.length > 0) {
            subjectsContainer.innerHTML = `<ul class="list-unstyled mb-0">` +
                tutorData.subjects.map(subject => `<li><span class="fw-bold">${subject.name}</span></li>`).join('') +
                `</ul>`;
        } else {
            subjectsContainer.innerHTML = '<p class="text-muted">Предмети не вказані</p>';
        }

        // Заповнюємо відгуки
        const reviewsContainer = document.getElementById('tutorReviews');
        if (tutorData.reviews && tutorData.reviews.length > 0) {
            reviewsContainer.innerHTML = tutorData.reviews.map(review => `
                <div class="review-item mb-4">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <h6 class="mb-0">${review.studentName || 'Анонім'}</h6>
                        <div class="rating">
                            ${Array(5).fill().map((_, i) => 
                                `<i class="fas fa-star${i < review.rating ? '' : '-o'} text-warning"></i>`
                            ).join('')}
                        </div>
                    </div>
                    <p class="text-muted small mb-2">${formatDate(review.date)}</p>
                    <p>${review.text || ''}</p>
                </div>
            `).join('');
        } else {
            reviewsContainer.innerHTML = '<p class="text-muted">Відгуків поки немає</p>';
        }

        // Замість alert відкриваємо модалку
        const bookLessonBtn = document.getElementById('bookLessonBtn');
        const lessonModal = new bootstrap.Modal(document.getElementById('lessonModal'));
        bookLessonBtn.addEventListener('click', () => {
            if (!localStorage.getItem('token')) {
                alert('Будь ласка, увійдіть в систему для запису на урок');
                window.location.href = '../login/login.html';
                return;
            }
            lessonModal.show();
        });

        // Обробка відправки форми
        document.getElementById('lessonRequestForm').addEventListener('submit', async function(e) {
            e.preventDefault();
            const message = document.getElementById('lessonMessage').value.trim();
            const date = document.getElementById('lessonDate').value;
            if (!message || !date) return;
            try {
                // TODO: замінити на реальний API
                await apiClient.sendLessonRequest({
                    tutorId,
                    message,
                    date
                });
                lessonModal.hide();
                alert('Ваше повідомлення надіслано репетитору на email!');
            } catch (err) {
                alert('Сталася помилка при надсиланні повідомлення. Спробуйте ще раз.');
            }
        });

        document.getElementById('sendMessageBtn').addEventListener('click', () => {
            if (!localStorage.getItem('token')) {
                alert('Будь ласка, увійдіть в систему для написання повідомлення');
                window.location.href = '../login/login.html';
                return;
            }
            // TODO: Реалізувати відправку повідомлення
            alert('Функція обміну повідомленнями буде доступна найближчим часом');
        });

        document.getElementById('addReviewBtn').addEventListener('click', () => {
            if (!localStorage.getItem('token')) {
                alert('Будь ласка, увійдіть в систему для додавання відгуку');
                window.location.href = '../login/login.html';
                return;
            }
            // TODO: Реалізувати додавання відгуку
            alert('Функція додавання відгуку буде доступна найближчим часом');
        });

    } catch (error) {
        console.error('Помилка завантаження профілю:', error);
        alert('Не вдалося завантажити профіль репетитора');
    }
});

// Функція для форматування дати
function formatDate(dateString) {
    const date = new Date(dateString);
    const now = new Date();
    const diff = now - date;
    
    const days = Math.floor(diff / (1000 * 60 * 60 * 24));
    const months = Math.floor(days / 30);
    const years = Math.floor(months / 12);

    if (years > 0) {
        return `${years} ${getPluralForm(years, 'рік', 'роки', 'років')} тому`;
    }
    if (months > 0) {
        return `${months} ${getPluralForm(months, 'місяць', 'місяці', 'місяців')} тому`;
    }
    if (days > 0) {
        return `${days} ${getPluralForm(days, 'день', 'дні', 'днів')} тому`;
    }
    return 'Сьогодні';
}

// Функція для отримання правильної форми слова
function getPluralForm(number, one, two, five) {
    let n = Math.abs(number);
    n %= 100;
    const n1 = n % 10;
    if (n > 10 && n < 20) return five;
    if (n1 > 1 && n1 < 5) return two;
    if (n1 === 1) return one;
    return five;
}
