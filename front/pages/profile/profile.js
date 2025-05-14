document.addEventListener('DOMContentLoaded', async () => {
    // Получаем ID репетитора из URL
    const urlParams = new URLSearchParams(window.location.search);
    const tutorId = urlParams.get('id');
    if (!tutorId) {
        alert('ID репетитора не указан');
        return;
    }

    // DOM элементы для профиля
    const tutorName = document.getElementById('tutorName');
    const tutorSubject = document.getElementById('tutorSubject');
    const tutorLocation = document.getElementById('tutorLocation');
    const tutorEducation = document.getElementById('tutorEducation');
    const tutorExperience = document.getElementById('tutorExperience');
    const tutorPrice = document.getElementById('tutorPrice');
    const tutorAbout = document.getElementById('tutorAbout');
    const tutorSubjects = document.getElementById('tutorSubjects');
    const tutorRating = document.getElementById('tutorRating');

    // DOM элементы для отзывов
    const reviewsContainer = document.getElementById('tutorReviews');
    const addReviewBtn = document.getElementById('addReviewBtn');
    const reviewModal = new bootstrap.Modal(document.getElementById('reviewModal'));
    const reviewStars = document.getElementById('reviewStars');
    const reviewForm = document.getElementById('reviewForm');
    const reviewText = document.getElementById('reviewText');

    let selectedRating = 0;
    let hoveredRating = 0;
    let userReviewExists = false;
    let currentUserId = null;
    let tutorData = null;

    // Получаем userId из токена
    function getUserIdFromToken() {
        const token = localStorage.getItem('token');
        if (!token) return null;
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            return payload.nameid || payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] || null;
        } catch {
            return null;
        }
    }
    currentUserId = getUserIdFromToken();

    // Загрузка и отображение профиля и отзывов
    async function loadAndRenderProfile() {
        try {
            tutorData = await apiClient.getTutorProfile(tutorId);
            console.log('Сервер повернув профіль:', tutorData); // Діагностика
            // Основная информация
            tutorName.textContent = `${tutorData.user?.firstName || ''} ${tutorData.user?.lastName || ''}`.trim() || 'Не вказано';
            tutorSubject.textContent = `Репетитор з ${tutorData.subjects && tutorData.subjects.length > 0 ? tutorData.subjects.map(s => s.name).join(', ') : 'Не вказано'}`;
            tutorLocation.textContent = tutorData.location || 'Не вказано';
            tutorEducation.textContent = tutorData.education || 'Не вказано';
            tutorExperience.textContent = (tutorData.yearsOfExperience !== undefined && tutorData.yearsOfExperience !== null) ? tutorData.yearsOfExperience + ' років' : 'Не вказано';
            tutorPrice.textContent = (tutorData.hourlyRate !== undefined && tutorData.hourlyRate !== null) ? tutorData.hourlyRate + ' грн/год' : 'Не вказано';
            tutorAbout.textContent = tutorData.bio || tutorData.about || 'Інформація відсутня';
            // Список предметов
            if (tutorData.subjects && tutorData.subjects.length > 0) {
                tutorSubjects.innerHTML = `<ul class="list-unstyled mb-0">` +
                    tutorData.subjects.map(subject => `<li><span class="fw-bold">${subject.name}</span></li>`).join('') +
                    `</ul>`;
            } else {
                tutorSubjects.innerHTML = '<p class="text-muted">Предмети не вказані</p>';
            }
            // Рейтинг
            const rating = tutorData.averageRating || 0;
            const reviewsCount = tutorData.totalReviews || 0;
            const ratingStars = Math.round(rating);
            tutorRating.innerHTML = `
                ${Array(5).fill().map((_, i) => 
                    `<i class="fas fa-star${i < ratingStars ? ' text-warning' : ' text-secondary'}"></i>`
                ).join('')}
                <span class="ms-1">${rating.toFixed(1)}</span>
                <span class="text-muted ms-2">(${reviewsCount} відгуків)</span>
            `;
            // Отзывы
            renderReviews(tutorData.reviews || []);
            // Проверяем, оставлял ли пользователь отзыв
            userReviewExists = false;
            if (currentUserId) {
                userReviewExists = (tutorData.reviews || []).some(r => r.studentId == currentUserId);
            }
            if (addReviewBtn) {
                addReviewBtn.disabled = userReviewExists;
                addReviewBtn.textContent = userReviewExists ? 'Відгук вже залишено' : 'Додати відгук';
            }
        } catch (err) {
            tutorName.textContent = 'Помилка завантаження';
            reviewsContainer.innerHTML = '<p class="text-danger">Не вдалося завантажити відгуки</p>';
        }
    }

    // Функція для генерації зірок (як у lkab/student.js)
    function generateStars(rating) {
        let stars = '';
        for (let i = 1; i <= 5; i++) {
            if (i <= rating) {
                stars += '<i class="fas fa-star text-warning"></i>';
            } else {
                stars += '<i class="far fa-star text-secondary"></i>';
            }
        }
        return stars;
    }

    function renderReviews(reviews) {
        if (!reviews || reviews.length === 0) {
            reviewsContainer.innerHTML = '<p class="text-muted">Відгуків поки немає</p>';
            return;
        }
        reviewsContainer.innerHTML = reviews.map(review => `
            <div class="review-item mb-4">
                <div class="d-flex justify-content-between align-items-center mb-2">
                    <h6 class="mb-0">${review.student ? (review.student.firstName + ' ' + review.student.lastName) : 'Анонім'}</h6>
                    <div class="rating">
                        ${generateStars(Number(review.rating))}
                    </div>
                </div>
                <p class="text-muted small mb-2">${formatDate(review.date)}</p>
                <p>${review.comment || ''}</p>
            </div>
        `).join('');
    }

    // Форматирование даты
    function formatDate(dateString) {
        const date = new Date(dateString);
        return date.toLocaleDateString('uk-UA');
    }

    // --- Новий простий рейтинг ---
    function renderSimpleStarRating() {
        reviewStars.innerHTML = '';
        // Одна зірка
        const star = document.createElement('span');
        star.className = 'star-btn';
        let starClass = 'fas fa-star';
        starClass += selectedRating > 0 ? ' text-warning' : ' text-secondary';
        star.innerHTML = `<i class="${starClass}" style="font-size:2rem;"></i>`;
        star.style.cursor = 'default';
        star.style.fontSize = '2rem';
        star.style.padding = '0 8px 0 0';
        reviewStars.appendChild(star);
        // Поле для вводу числа
        const input = document.createElement('input');
        input.type = 'number';
        input.min = 0;
        input.max = 5;
        input.step = 1;
        input.value = selectedRating;
        input.style.width = '60px';
        input.style.fontSize = '1.2rem';
        input.style.marginLeft = '8px';
        input.addEventListener('input', (e) => {
            let val = parseInt(e.target.value);
            if (isNaN(val) || val < 0) val = 0;
            if (val > 5) val = 5;
            selectedRating = val;
            input.value = val;
            renderSimpleStarRating();
        });
        reviewStars.appendChild(input);
    }

    // Кнопка "Додати відгук"
    if (addReviewBtn) {
        addReviewBtn.addEventListener('click', () => {
            if (!localStorage.getItem('token')) {
                alert('Будь ласка, увійдіть в систему для додавання відгуку');
                window.location.href = '../login/login.html';
                return;
            }
            if (userReviewExists) {
                alert('Ви вже залишили відгук цьому репетитору!');
                return;
            }
            reviewText.value = '';
            selectedRating = 0;
            renderSimpleStarRating();
            reviewModal.show();
        });
    }

    // Отправка отзыва
    reviewForm.addEventListener('submit', async function(e) {
        e.preventDefault();
        const comment = reviewText.value.trim();
        if (!selectedRating || !comment) {
            alert('Будь ласка, виберіть рейтинг і напишіть коментар!');
            return;
        }
        try {
            const res = await apiClient.post('/api/Reviews', {
                tutorId: tutorId,
                rating: selectedRating,
                comment: comment
            });
            if (res.ok) {
                reviewModal.hide();
                await loadAndRenderProfile();
            } else {
                const errorText = await res.text();
                alert('Не вдалося залишити відгук. ' + errorText);
            }
        } catch (err) {
            alert('Сталася помилка при надсиланні відгуку. Спробуйте ще раз.');
        }
    });

    // Викликаємо новий рендер при відкритті модалки
    const reviewModalEl = document.getElementById('reviewModal');
    if (reviewModalEl) {
      reviewModalEl.addEventListener('shown.bs.modal', () => {
        selectedRating = 0;
        renderSimpleStarRating();
      });
    }

    // Первичная загрузка профиля и отзывов
    await loadAndRenderProfile();
});
