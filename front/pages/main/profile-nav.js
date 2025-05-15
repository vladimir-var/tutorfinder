document.addEventListener('DOMContentLoaded', function() {
    let token = localStorage.getItem('token');
    let userType = localStorage.getItem('userType');
    const profileNav = document.getElementById('profileNav');
    const profileLink = document.getElementById('profileLink');
    const goToProfile = document.getElementById('goToProfile');
    const logoutBtn = document.getElementById('logoutBtn');

    // Вважаємо токен валідним лише якщо він не порожній, не null і не undefined
    const isAuthenticated = token && token !== 'null' && token !== 'undefined' && token.trim() !== '' && userType && userType !== 'null' && userType !== 'undefined' && userType.trim() !== '';

    if (isAuthenticated) {
        profileNav.style.display = 'block';
        if (goToProfile) {
            goToProfile.onclick = function(e) {
                e.preventDefault();
                if (userType === 'tutor') {
                    window.location.href = '../lkab/tutor.html';
                } else if (userType === 'student') {
                    window.location.href = '../lkab/student.html';
                }
            };
        }
        if (logoutBtn) {
            logoutBtn.onclick = function(e) {
                e.preventDefault();
                localStorage.removeItem('token');
                localStorage.removeItem('userType');
                localStorage.removeItem('user');
                localStorage.removeItem('tutor');
                window.location.href = '../main/index.html';
            };
        }
    } else {
        profileNav.style.display = 'none';
    }

    // Ховаємо пункт меню 'Стати Репетитором' для студентів
    var becomeTutorLink = document.querySelector('a[href*="register"], a[href*="register.html"]');
    if (becomeTutorLink && userType === 'student') {
        var navItem = becomeTutorLink.closest('.nav-item');
        if (navItem) navItem.style.display = 'none';
    }
}); 