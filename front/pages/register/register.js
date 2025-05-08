// Проверяем, что все необходимые классы загружены
if (typeof ApiClient === 'undefined' || typeof FormValidator === 'undefined') {
    console.error('Не удалось загрузить необходимые классы');
} else {
    const api = new ApiClient('https://localhost:7274');

    document.addEventListener('DOMContentLoaded', function() {
        const registerForm = document.getElementById('registerForm');
        
        // Создаем индикатор загрузки
        const loadingSpinner = document.createElement('div');
        loadingSpinner.className = 'loading-overlay';
        loadingSpinner.innerHTML = `
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Загрузка...</span>
            </div>
            <div class="loading-text mt-2">Подождите, идет обработка...</div>
        `;
        document.body.appendChild(loadingSpinner);

        // Создаем контейнер для уведомлений
        const notificationContainer = document.createElement('div');
        notificationContainer.className = 'notification-container';
        document.body.appendChild(notificationContainer);

        // Инициализация маски для телефона
        const phoneInput = document.getElementById('phone');
        phoneInput.addEventListener('input', function() {
            FormValidator.formatPhoneNumber(this);
        });

        // Загрузка списка предметов
        loadSubjects();

        if (registerForm) {
            registerForm.addEventListener('submit', async function(e) {
                e.preventDefault();
                
                if (!validateForm()) {
                    return;
                }

                try {
                    showLoading('Регистрация пользователя...');
                    console.log('Начало регистрации пользователя...');
                    
                    // Сбор данных формы
                    const userData = {
                        firstName: document.getElementById('firstName').value,
                        lastName: document.getElementById('lastName').value,
                        email: document.getElementById('email').value,
                        phone: document.getElementById('phone').value,
                        password: document.getElementById('password').value,
                        role: 'Tutor'
                    };

                    console.log('Отправка данных пользователя:', { ...userData, password: '***' });

                    // Регистрация пользователя
                    const userResponse = await api.registerUser(userData);
                    console.log('Пользователь успешно зарегистрирован:', userResponse);
                    
                    showLoading('Регистрация репетитора...');
                    
                    const tutorData = {
                        bio: document.getElementById('description').value,
                        education: document.getElementById('education').value,
                        yearsOfExperience: parseInt(document.getElementById('experience').value),
                        hourlyRate: parseFloat(document.getElementById('price').value),
                        isAvailable: true,
                        teachingStyle: document.getElementById('availability').value,
                        subjects: Array.from(document.getElementById('subjects').selectedOptions).map(option => option.value)
                    };

                    console.log('Отправка данных репетитора:', tutorData);

                    // Регистрация репетитора
                    tutorData.userId = userResponse.id;
                    const tutorResponse = await api.registerTutor(tutorData);
                    console.log('Репетитор успешно зарегистрирован:', tutorResponse);

                    // Сохранение данных пользователя в localStorage
                    localStorage.setItem('user', JSON.stringify(userResponse));
                    
                    showNotification('Регистрация успешно завершена!', 'success');
                    
                    // Редирект на страницу профиля через 2 секунды
                    setTimeout(() => {
                        window.location.href = '../profile/profile.html';
                    }, 2000);

                } catch (error) {
                    console.error('Ошибка при регистрации:', error);
                    showError(error.message);
                } finally {
                    hideLoading();
                }
            });
        }

        // Валидация полей при вводе
        const inputs = registerForm.querySelectorAll('input, select, textarea');
        inputs.forEach(input => {
            input.addEventListener('input', function() {
                validateField(this);
            });
        });
    });

    async function loadSubjects() {
        try {
            showLoading('Загрузка списка предметов...');
            console.log('Загрузка списка предметов...');
            
            const subjects = await api.getSubjects();
            console.log('Предметы загружены:', subjects);
            
            const subjectsSelect = document.getElementById('subjects');
            subjectsSelect.innerHTML = subjects.map(subject => 
                `<option value="${subject.id}">${subject.name}</option>`
            ).join('');
            
            showNotification('Список предметов загружен', 'info');
        } catch (error) {
            console.error('Ошибка при загрузке предметов:', error);
            showError('Не удалось загрузить список предметов');
        } finally {
            hideLoading();
        }
    }

    function validateForm() {
        let isValid = true;
        const inputs = document.querySelectorAll('input, select, textarea');
        
        inputs.forEach(input => {
            if (!validateField(input)) {
                isValid = false;
            }
        });

        // Проверка совпадения паролей
        const password = document.getElementById('password');
        const confirmPassword = document.getElementById('confirmPassword');
        if (password.value !== confirmPassword.value) {
            FormValidator.showError(confirmPassword, 'Пароли не совпадают');
            isValid = false;
        }

        return isValid;
    }

    function validateField(field) {
        FormValidator.clearError(field);
        let isValid = true;
        let errorMessage = '';

        switch (field.id) {
            case 'email':
                if (!FormValidator.validateEmail(field.value)) {
                    errorMessage = 'Введите корректный email адрес';
                    isValid = false;
                }
                break;
            case 'password':
                if (!FormValidator.validatePassword(field.value)) {
                    errorMessage = 'Пароль должен содержать минимум 8 символов, включая буквы и цифры';
                    isValid = false;
                }
                break;
            case 'phone':
                if (!FormValidator.validatePhone(field.value)) {
                    errorMessage = 'Введите корректный номер телефона';
                    isValid = false;
                }
                break;
            case 'price':
                if (!FormValidator.validatePrice(field.value)) {
                    errorMessage = 'Цена должна быть от 1 до 10000 грн';
                    isValid = false;
                }
                break;
            default:
                if (field.required && !FormValidator.validateRequired(field.value)) {
                    errorMessage = 'Это поле обязательно для заполнения';
                    isValid = false;
                }
        }

        if (!isValid) {
            FormValidator.showError(field, errorMessage);
        }

        return isValid;
    }

    function showLoading(message = 'Загрузка...') {
        const spinner = document.querySelector('.loading-overlay');
        const loadingText = spinner.querySelector('.loading-text');
        loadingText.textContent = message;
        spinner.style.display = 'flex';
        document.querySelector('button[type="submit"]').disabled = true;
    }

    function hideLoading() {
        const spinner = document.querySelector('.loading-overlay');
        spinner.style.display = 'none';
        document.querySelector('button[type="submit"]').disabled = false;
    }

    function showError(message) {
        showNotification(message, 'error');
    }

    function showNotification(message, type = 'info') {
        const notification = document.createElement('div');
        notification.className = `notification notification-${type}`;
        notification.innerHTML = `
            <div class="notification-content">
                <i class="fas ${getNotificationIcon(type)}"></i>
                <span>${message}</span>
            </div>
            <button type="button" class="btn-close" onclick="this.parentElement.remove()"></button>
        `;
        
        const container = document.querySelector('.notification-container');
        container.appendChild(notification);
        
        // Автоматически удаляем уведомление через 5 секунд
        setTimeout(() => {
            notification.remove();
        }, 5000);
    }

    function getNotificationIcon(type) {
        switch (type) {
            case 'success':
                return 'fa-check-circle';
            case 'error':
                return 'fa-exclamation-circle';
            case 'info':
                return 'fa-info-circle';
            default:
                return 'fa-bell';
        }
    }
} 