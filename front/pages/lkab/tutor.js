document.addEventListener('DOMContentLoaded', function() {
    loadUserInfo();
    loadCertificates();
    loadReviews();
    setupCertificateForm();
});

async function loadUserInfo() {
    try {
        const response = await apiClient.get('/api/tutorprofile/profile');
        if (response.ok) {
            const data = await response.json();
            document.getElementById('firstName').value = data.firstName;
            document.getElementById('lastName').value = data.lastName;
            document.getElementById('email').value = data.email;
            document.getElementById('phone').value = data.phone;
        } else {
            console.error('Помилка завантаження інформації про користувача');
        }
    } catch (error) {
        console.error('Помилка:', error);
    }
}

async function loadCertificates() {
    try {
        const response = await apiClient.get('/api/tutorprofile/certificates');
        if (response.ok) {
            const certificates = await response.json();
            const certificatesList = document.getElementById('certificatesList');
            certificatesList.innerHTML = '';

            if (certificates.length === 0) {
                certificatesList.innerHTML = '<div class="text-center text-muted">У вас ще немає сертифікатів</div>';
                return;
            }

            certificates.forEach(certificate => {
                const certificateElement = document.createElement('div');
                certificateElement.className = 'list-group-item';
                certificateElement.innerHTML = `
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h6 class="mb-1">${certificate.name}</h6>
                            <p class="mb-1 text-muted">${certificate.organization}</p>
                            <small class="text-muted">Отримано: ${new Date(certificate.date).toLocaleDateString()}</small>
                        </div>
                        <div>
                            <a href="${certificate.fileUrl}" class="btn btn-sm btn-outline-primary me-2" target="_blank">
                                <i class="fas fa-eye"></i>
                            </a>
                            <button class="btn btn-sm btn-outline-danger" onclick="deleteCertificate(${certificate.id})">
                                <i class="fas fa-trash"></i>
                            </button>
                        </div>
                    </div>
                `;
                certificatesList.appendChild(certificateElement);
            });
        }
    } catch (error) {
        console.error('Помилка:', error);
    }
}

async function loadReviews() {
    try {
        const response = await apiClient.get('/api/tutorprofile/reviews');
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