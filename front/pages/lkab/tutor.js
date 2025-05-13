document.addEventListener('DOMContentLoaded', function() {
    if (typeof apiClient === 'undefined') {
        console.error('ApiClient не определен. Проверьте подключение файла api.js');
        return;
    }
    loadUserInfo();
    loadCertificates();
    loadReviews();
    setupCertificateForm();
});

async function loadUserInfo() {
    try {
        const response = await apiClient.get('/api/Tutors/profile');
        if (response.ok) {
            const data = await response.json();
            document.getElementById('firstName').value = data.user?.firstName || '';
            document.getElementById('lastName').value = data.user?.lastName || '';
            document.getElementById('email').value = data.user?.email || '';
            document.getElementById('phone').value = data.user?.phone || '';
        } else {
            console.error('Помилка завантаження інформації про користувача');
        }
    } catch (error) {
        console.error('Помилка:', error);
    }
}

async function loadCertificates() {
    try {
        const response = await apiClient.get('/api/Tutors/certificates');
        if (response.ok) {
            const certificatesText = await response.text();
            const certificatesList = document.getElementById('certificatesList');
            certificatesList.innerHTML = '';

            const certificates = certificatesText
                .split('\n')
                .map(c => c.trim())
                .filter(c => c.length > 0 && c.replace(/"/g, '').trim().length > 0);

            if (certificates.length === 0) {
                certificatesList.innerHTML = '<div class="text-center text-muted">У вас ще немає сертифікатів</div>';
                return;
            }

            certificates.forEach((certificate, index) => {
                const certificateElement = document.createElement('div');
                certificateElement.className = 'list-group-item';
                certificateElement.innerHTML = `
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <p class="mb-1">${certificate}</p>
                        </div>
                        <div>
                            <button class="btn btn-sm btn-outline-danger" onclick="deleteCertificate(${index})">
                                <i class="fas fa-trash"></i>
                            </button>
                        </div>
                    </div>
                `;
                certificatesList.appendChild(certificateElement);
            });
        } else {
            console.error('Помилка завантаження сертифікатів');
        }
    } catch (error) {
        console.error('Помилка:', error);
    }
}

async function loadReviews() {
    try {
        const response = await apiClient.get('/api/Tutors/reviews');
        if (response.ok) {
            const reviews = await response.json();
            const reviewsList = document.getElementById('reviewsList');
            reviewsList.innerHTML = '';

            if (reviews.length === 0) {
                reviewsList.innerHTML = '<div class="text-center text-muted">У вас ще немає відгуків</div>';
                return;
            }

            reviews.forEach(review => {
                const reviewElement = document.createElement('div');
                reviewElement.className = 'list-group-item';
                reviewElement.innerHTML = `
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <h6 class="mb-0">${review.studentName}</h6>
                        <div class="text-warning">
                            ${generateStars(review.rating)}
                        </div>
                    </div>
                    <p class="mb-2">${review.comment}</p>
                    <small class="text-muted">${new Date(review.date).toLocaleDateString()}</small>
                `;
                reviewsList.appendChild(reviewElement);
            });
        } else {
            console.error('Помилка завантаження відгуків');
        }
    } catch (error) {
        console.error('Помилка:', error);
    }
}

function setupCertificateForm() {
    const saveButton = document.getElementById('saveCertificate');
    saveButton.addEventListener('click', async () => {
        const form = document.getElementById('addCertificateForm');
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        const name = document.getElementById('certificateName').value;
        const certificate = { name };

        try {
            const response = await apiClient.post('/api/Tutors/certificates', certificate);
            if (response.ok) {
                const modal = bootstrap.Modal.getInstance(document.getElementById('addCertificateModal'));
                modal.hide();
                form.reset();
                loadCertificates();
            } else {
                console.error('Помилка додавання сертифіката');
            }
        } catch (error) {
            console.error('Помилка:', error);
        }
    });
}

async function deleteCertificate(index) {
    if (!confirm('Ви впевнені, що хочете видалити цей сертифікат?')) {
        return;
    }

    try {
        const response = await apiClient.delete(`/api/Tutors/certificates/${index}`);
        if (response.ok) {
            loadCertificates();
        } else {
            console.error('Помилка видалення сертифіката');
        }
    } catch (error) {
        console.error('Помилка:', error);
    }
}

function generateStars(rating) {
    let stars = '';
    for (let i = 1; i <= 5; i++) {
        if (i <= rating) {
            stars += '<i class="fas fa-star"></i>';
        } else {
            stars += '<i class="far fa-star"></i>';
        }
    }
    return stars;
} 