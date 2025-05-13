// Базовая навигация
document.addEventListener('DOMContentLoaded', function() {
    const subjectFilter = document.getElementById('subjectFilter');
    const searchFilters = document.getElementById('searchFilters');
    const tutorResults = document.getElementById('tutorResults');
    const resultCount = document.getElementById('resultCount');
    const sortDropdown = document.getElementById('sortDropdown');
    const currentSort = document.getElementById('currentSort');
    let currentSortBy = 'rating';
    let currentSortDesc = true;

    // Встановлюємо початковий стан сортування
    if (currentSort) {
        currentSort.textContent = 'Рейтингом ↓';
    }

    // 1. Підвантаження предметів з API
    async function loadSubjects() {
        try {
            const response = await apiClient.get('/api/subjects');
            if (response.ok) {
                const subjects = await response.json();
                subjectFilter.innerHTML = '<option value="">Всі предмети</option>';
                subjects.forEach(subj => {
                    subjectFilter.innerHTML += `<option value="${subj.id}">${subj.name}</option>`;
                });
            }
        } catch (e) {
            console.error('Помилка завантаження предметів:', e);
        }
    }

    // 2. Пошук репетиторів з урахуванням фільтрів
    async function searchTutors(page = 1) {
        const params = new URLSearchParams();
        const subjectId = subjectFilter.value;
        const experience = document.getElementById('experienceFilter').value;
        const minPrice = document.getElementById('minPrice').value;
        const maxPrice = document.getElementById('maxPrice').value;
        const rating = document.getElementById('ratingFilter').value;
        const place = document.getElementById('placeFilter').value;

        if (subjectId) params.append('subjectId', subjectId);
        if (experience) params.append('minExperience', experience);
        if (minPrice) params.append('minPrice', minPrice);
        if (maxPrice) params.append('maxPrice', maxPrice);
        if (rating) params.append('rating', rating);
        if (place) params.append('place', place);
        params.append('page', page);
        params.append('sortBy', currentSortBy);
        params.append('sortDesc', currentSortDesc);

        try {
            const response = await apiClient.get('/api/Tutors/search?' + params.toString());
            if (response.ok) {
                const tutors = await response.json();
                renderTutors(tutors);
                resultCount.textContent = tutors.length;
            } else {
                tutorResults.innerHTML = '<div class="alert alert-danger">Не вдалося завантажити репетиторів</div>';
                resultCount.textContent = 0;
            }
        } catch (e) {
            tutorResults.innerHTML = '<div class="alert alert-danger">Сталася помилка при пошуку</div>';
            resultCount.textContent = 0;
        }
    }

    // 3. Рендер карток репетиторів
    function getUserRoleFromToken() {
        const token = localStorage.getItem('token');
        if (!token) return null;
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            return payload.role || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
        } catch {
            return null;
        }
    }

    function renderTutors(tutors) {
        tutorResults.innerHTML = '';
        if (!tutors || tutors.length === 0) {
            tutorResults.innerHTML = '<div class="text-center text-muted">Репетиторів не знайдено</div>';
            return;
        }
        const userRole = getUserRoleFromToken();
        tutors.forEach(tutor => {
            const detailsBtn = userRole === 'student'
                ? `<a href="../profile/profile.html?id=${tutor.id}" class="btn btn-outline-primary">Детальніше</a>`
                : `<button class="btn btn-outline-primary" disabled title="Доступно лише для учнів">Детальніше</button>`;
            tutorResults.innerHTML += `
                <div class="card tutor-card mb-4">
                    <div class="card-body">
                        <div class="d-flex align-items-center mb-3">
                            <div class="flex-shrink-0">
                                <img src="https://randomuser.me/api/portraits/men/1.jpg" class="rounded-circle" width="60" height="60" alt="Avatar">
                            </div>
                            <div class="flex-grow-1 ms-3">
                                <h5 class="fw-bold mb-1">${tutor.user?.firstName || ''} ${tutor.user?.lastName || ''}</h5>
                                <div class="text-muted mb-1">${tutor.subjects?.map(s => s.name).join(', ') || ''}</div>
                                <div class="text-warning mb-1">${generateStars(tutor.averageRating || 0)} <span class="ms-2">${tutor.averageRating?.toFixed(1) || '0.0'}</span></div>
                                <div class="small text-muted">Досвід: ${tutor.yearsOfExperience} років | Ціна: ${tutor.hourlyRate} грн/год</div>
                                <div class="small text-muted">${tutor.teachingStyle ? 'Формат: ' + tutor.teachingStyle : ''}</div>
                            </div>
                        </div>
                        <p>${tutor.bio || ''}</p>
                        ${detailsBtn}
                    </div>
                </div>
            `;
        });
    }

    function generateStars(rating) {
        let stars = '';
        for (let i = 1; i <= 5; i++) {
            if (i <= Math.round(rating)) {
                stars += '<i class="fas fa-star"></i>';
            } else {
                stars += '<i class="far fa-star"></i>';
            }
        }
        return stars;
    }

    // 4. Сабміт фільтрів
    if (searchFilters) {
        searchFilters.addEventListener('submit', function(e) {
            e.preventDefault();
            searchTutors();
        });
    }

    // 5. Обробка сортування
    document.querySelectorAll('.dropdown-item').forEach(item => {
        item.addEventListener('click', function(e) {
            e.preventDefault();
            const sortBy = this.dataset.sort;
            if (sortBy === currentSortBy) {
                currentSortDesc = !currentSortDesc;
            } else {
                currentSortBy = sortBy;
                currentSortDesc = true;
            }
            currentSort.textContent = this.textContent + (currentSortDesc ? ' ↓' : ' ↑');
            console.log('Sorting changed:', { sortBy: currentSortBy, sortDesc: currentSortDesc });
            searchTutors();
        });
    });

    // 6. Початкове завантаження
    loadSubjects();
    searchTutors();
});

