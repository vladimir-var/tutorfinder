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
});