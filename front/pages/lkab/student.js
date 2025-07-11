document.addEventListener('DOMContentLoaded', function() {
    const token = localStorage.getItem('token');
    if (!token) {
        window.location.href = '../login/login.html';
        return;
    }
    loadUserInfo();
    loadUserReviews();
});

async function loadUserInfo() {
    try {
        const response = await apiClient.get('/api/Students/profile');
        if (response.ok) {
            const data = await response.json();
            document.getElementById('firstName').value = data.firstName;
            document.getElementById('lastName').value = data.lastName;
            document.getElementById('email').value = data.email;
            document.getElementById('phone').value = data.phone;
            const profileImage = document.getElementById('studentProfileImage');
            if (data.profileImage) {
                profileImage.src = data.profileImage;
            } else {
                profileImage.src = '../assets/default-avatar.png';
            }
        } else {
            console.error('Помилка завантаження інформації про користувача');
        }
    } catch (error) {
        console.error('Помилка:', error);
    }
}

async function loadUserReviews() {
    try {
        const response = await apiClient.get('/api/Students/reviews');
        if (response.ok) {
            const reviews = await response.json();
            const reviewsList = document.getElementById('reviewsList');
            reviewsList.innerHTML = '';

            if (reviews.length === 0) {
                reviewsList.innerHTML = '<div class="text-center text-muted">У вас ще немає відгуків</div>';
                return;
            }

            reviews.forEach(review => {
                console.log('Review:', review);
                const tutor = review.Tutor || review.tutor;
                const user = tutor && (tutor.User || tutor.user);
                const tutorName = user ? `${user.firstName} ${user.lastName}` : 'Без імені';
                const reviewElement = document.createElement('div');
                reviewElement.className = 'list-group-item';
                reviewElement.innerHTML = `
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <h6 class="mb-0">${tutorName}</h6>
                        <div class="text-warning">
                            ${generateStars(review.rating)}
                        </div>
                    </div>
                    <p class="mb-2">${review.comment}</p>
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