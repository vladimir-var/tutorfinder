document.addEventListener('DOMContentLoaded', function() {
    setupLoginForm();
    setupPasswordToggle();
});

function setupLoginForm() {
    const form = document.getElementById('loginForm');
    form.addEventListener('submit', async function(e) {
        e.preventDefault();
        
        if (!form.checkValidity()) {
            form.classList.add('was-validated');
            return;
        }

        const email = document.getElementById('email').value;
        const password = document.getElementById('password').value;

        try {
            const response = await apiClient.post('/api/auth/login', {
                email: email,
                password: password
            });

            const data = await response.json();
            // Сохраняем токен в localStorage
            localStorage.setItem('token', data.token);
            localStorage.setItem('userType', data.userType);

            // Перенаправляем в соответствующий личный кабинет
            if (data.userType === 'tutor') {
                window.location.href = '../lkab/tutor.html';
            } else {
                window.location.href = '../lkab/student.html';
            }
        } catch (error) {
            console.error('Помилка:', error);
            showError(error.message || 'Помилка входу. Перевірте ваші дані.');
        }
    });
}

function setupPasswordToggle() {
    const toggleButton = document.getElementById('togglePassword');
    const passwordInput = document.getElementById('password');

    toggleButton.addEventListener('click', function() {
        const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordInput.setAttribute('type', type);
        
        // Меняем иконку
        const icon = toggleButton.querySelector('i');
        icon.classList.toggle('fa-eye');
        icon.classList.toggle('fa-eye-slash');
    });
}

function showError(message) {
    // Создаем элемент для отображения ошибки
    const errorDiv = document.createElement('div');
    errorDiv.className = 'alert alert-danger mt-3';
    errorDiv.textContent = message;

    // Добавляем ошибку после формы
    const form = document.getElementById('loginForm');
    const existingError = form.parentElement.querySelector('.alert-danger');
    if (existingError) {
        existingError.remove();
    }
    form.parentElement.appendChild(errorDiv);

    // Удаляем ошибку через 5 секунд
    setTimeout(() => {
        errorDiv.remove();
    }, 5000);
} 