// Базовая навигация
document.addEventListener('DOMContentLoaded', function() {
    const searchFilters = document.getElementById('searchFilters');
    if (searchFilters) {
        searchFilters.addEventListener('submit', function(e) {
            e.preventDefault();
            window.location.href = 'profile/profile.html';
        });
    }
});

