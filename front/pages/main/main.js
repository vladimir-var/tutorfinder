// TutorFinder - Final Main JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Search form handling
    const searchForm = document.querySelector('.hero-section form');
    if (searchForm) {
        searchForm.addEventListener('submit', function(e) {
            e.preventDefault();
            window.location.href = 'search/search.html';
        });
    }

    if (window.bootstrap?.Tooltip) {
        document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(el => {
            new bootstrap.Tooltip(el);
        });
    }

    // Приховати пункти для учня
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
    const role = getUserRoleFromToken();
    if (role === 'student') {
        const loginNav = document.getElementById('loginNav');
        const registerStudentNav = document.getElementById('registerStudentNav');
        if (loginNav) loginNav.style.display = 'none';
        if (registerStudentNav) registerStudentNav.style.display = 'none';
    }
    if (role === 'tutor') {
        const loginNav = document.getElementById('loginNav');
        const registerStudentNav = document.getElementById('registerStudentNav');
        const findTutorNav = document.getElementById('findTutorNav');
        if (loginNav) loginNav.style.display = 'none';
        if (registerStudentNav) registerStudentNav.style.display = 'none';
        if (findTutorNav) findTutorNav.style.display = 'none';
    }
});