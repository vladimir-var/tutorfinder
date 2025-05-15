document.addEventListener('DOMContentLoaded', function() {
    const apiClient = new ApiClient('https://localhost:7274');
    const form = document.getElementById('registerForm');
    const phoneInput = document.getElementById('phone');
    const loadingSpinner = document.getElementById('loadingSpinner');
    const errorAlert = document.getElementById('errorAlert');
    const errorMessage = document.getElementById('errorMessage');
    const profileImageInput = document.getElementById('profileImage');
    const imagePreview = document.getElementById('imagePreview');

    localStorage.clear();
    sessionStorage.clear();

    // Маска для телефона
    phoneInput.addEventListener('input', function(e) {
        let value = e.target.value.replace(/\D/g, '');
        if (value.length > 0) {
            if (!value.startsWith('380')) {
                value = '380' + value;
            }
            e.target.value = '+' + value;
        }
    });

    // Показать/скрыть пароль
    document.getElementById('togglePassword').addEventListener('click', function() {
        const passwordInput = document.getElementById('password');
        if (passwordInput.type === 'password') {
            passwordInput.type = 'text';
            this.innerHTML = '<i class="fas fa-eye-slash"></i>';
        } else {
            passwordInput.type = 'password';
            this.innerHTML = '<i class="fas fa-eye"></i>';
        }
    });

    // Обробка завантаження фото
    profileImageInput.addEventListener('change', function(e) {
        const file = e.target.files[0];
        if (file) {
            // Перевірка розміру файлу (5MB)
            if (file.size > 5 * 1024 * 1024) {
                showError('Розмір файлу не повинен перевищувати 5MB');
                profileImageInput.value = '';
                return;
            }
            // Перевірка типу файлу
            if (!file.type.startsWith('image/')) {
                showError('Будь ласка, виберіть зображення');
                profileImageInput.value = '';
                return;
            }
            // Створюємо посилання на зображення
            const reader = new FileReader();
            reader.onload = function(e) {
                const imageUrl = e.target.result;
                imagePreview.src = imageUrl;
                profileImageInput.setAttribute('data-image-url', imageUrl);
            };
            reader.readAsDataURL(file);
        }
    });

    function showError(message) {
        errorMessage.textContent = message;
        errorAlert.classList.remove('d-none');
        setTimeout(() => {
            errorAlert.classList.add('d-none');
        }, 5000);
    }

    // Валидация формы
    function validateForm(data) {
        const errors = {};
        if (!data.firstName.trim()) errors.firstName = "Вкажіть ім'я";
        if (!data.lastName.trim()) errors.lastName = "Вкажіть прізвище";
        if (!data.email.trim() || !/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(data.email)) errors.email = 'Вкажіть коректну електронну пошту';
        if (!data.phone.trim() || data.phone.length < 10) errors.phone = 'Вкажіть коректний телефон';
        if (!data.password || data.password.length < 8) errors.password = 'Пароль має містити мінімум 8 символів';
        if (data.password !== data.confirmPassword) errors.confirmPassword = 'Паролі не співпадають';
        if (!data.terms) errors.terms = 'Потрібно погодитися з умовами';
        return errors;
    }

    // Обработка отправки формы
    form.addEventListener('submit', async function(e) {
        e.preventDefault();
        errorAlert.classList.add('d-none');
        loadingSpinner.classList.remove('d-none');

        const formData = {
            email: document.getElementById('email')?.value || '',
            password: document.getElementById('password')?.value || '',
            confirmPassword: document.getElementById('confirmPassword')?.value || '',
            firstName: document.getElementById('firstName')?.value || '',
            lastName: document.getElementById('lastName')?.value || '',
            phone: document.getElementById('phone')?.value || '',
            terms: document.getElementById('terms')?.checked,
            role: 'student',
            profileImage: profileImageInput.getAttribute('data-image-url') || null
        };

        // Валидация
        const errors = validateForm(formData);
        if (Object.keys(errors).length > 0) {
            const firstError = Object.values(errors)[0];
            errorMessage.textContent = firstError;
            errorAlert.classList.remove('d-none');
            loadingSpinner.classList.add('d-none');
            return;
        }

        try {
            const userData = {
                email: formData.email,
                password: formData.password,
                firstName: formData.firstName,
                lastName: formData.lastName,
                phone: formData.phone,
                role: formData.role,
                profileImage: formData.profileImage
            };
            const user = await apiClient.registerUser(userData);
            localStorage.setItem('user', JSON.stringify(user));
            alert('Реєстрація успішна! Тепер увійдіть у свій акаунт.');
            window.location.href = '../../login/login.html';
        } catch (error) {
            errorMessage.textContent = error.message || 'Помилка при реєстрації. Спробуйте ще раз.';
            errorAlert.classList.remove('d-none');
        } finally {
            loadingSpinner.classList.add('d-none');
        }
    });
}); 