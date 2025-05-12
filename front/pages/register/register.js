document.addEventListener('DOMContentLoaded', function() {
    const apiClient = new ApiClient('https://localhost:7274');
    const form = document.getElementById('registerForm');
    const phoneInput = document.getElementById('phone');
    const subjectsSelect = document.getElementById('subjects');
    const loadingSpinner = document.getElementById('loadingSpinner');
    const errorAlert = document.getElementById('errorAlert');
    const errorMessage = document.getElementById('errorMessage');
    const priceInput = document.getElementById('price');

    // Валидация цены при вводе
    priceInput.addEventListener('input', function(e) {
        let value = e.target.value;
        
        // Удаляем все нецифровые символы
        value = value.replace(/[^\d]/g, '');
        
        // Преобразуем в число
        let numValue = parseInt(value);
        
        // Проверяем на NaN
        if (isNaN(numValue)) {
            numValue = 0;
        }
        
        // Ограничиваем значение
        if (numValue > 10000) {
            numValue = 10000;
        }
        
        // Обновляем значение поля
        e.target.value = numValue;
    });

    // Валидация цены при потере фокуса
    priceInput.addEventListener('blur', function(e) {
        let value = parseInt(e.target.value);
        if (isNaN(value) || value < 0) {
            e.target.value = 0;
        }
    });

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

    // Загрузка предметов
    async function loadSubjects() {
        try {
            const subjects = await apiClient.getSubjects();
            if (subjects && subjects.length > 0) {
                subjects.forEach(subject => {
                    const option = document.createElement('option');
                    option.value = subject.id;
                    option.textContent = subject.name;
                    subjectsSelect.appendChild(option);
                });
            } else {
                console.warn('Список предметов пуст');
                errorMessage.textContent = 'Не вдалося завантажити список предметів. Спробуйте оновити сторінку.';
                errorAlert.classList.remove('d-none');
            }
        } catch (error) {
            console.error('Ошибка при загрузке предметов:', error);
            errorMessage.textContent = 'Помилка при завантаженні предметів. Перевірте підключення до сервера.';
            errorAlert.classList.remove('d-none');
        }
    }

    loadSubjects();

    // Обработка отправки формы
    form.addEventListener('submit', async function(e) {
        e.preventDefault();
        
        // Скрываем предыдущие ошибки
        errorAlert.classList.add('d-none');
        
        // Показываем спиннер
        loadingSpinner.classList.remove('d-none');
        
        // Собираем данные формы
        const formData = {
            email: document.getElementById('email')?.value || '',
            password: document.getElementById('password')?.value || '',
            confirmPassword: document.getElementById('confirmPassword')?.value || '',
            firstName: document.getElementById('firstName')?.value || '',
            lastName: document.getElementById('lastName')?.value || '',
            phone: document.getElementById('phone')?.value || '',
            role: 'tutor',
            // Данные репетитора
            bio: document.getElementById('description')?.value || '',
            education: document.getElementById('education')?.value || '',
            yearsOfExperience: parseInt(document.getElementById('experience')?.value || '0'),
            hourlyRate: parseInt(priceInput.value || '0'),
            teachingStyle: document.getElementById('availability')?.value || 'online',
            subjectIds: Array.from(subjectsSelect?.selectedOptions || []).map(option => parseInt(option.value))
        };

        // Дополнительная валидация цены
        if (formData.hourlyRate < 0 || formData.hourlyRate > 10000) {
            errorMessage.textContent = 'Ціна повинна бути від 0 до 10000 грн';
            errorAlert.classList.remove('d-none');
            loadingSpinner.classList.add('d-none');
            return;
        }

        // Валидация формы
        const errors = FormValidator.validateForm(formData);
        if (Object.keys(errors).length > 0) {
            // Відображаємо всі помилки
            Object.entries(errors).forEach(([field, message]) => {
                const element = document.getElementById(
                    field === 'hourlyRate' ? 'price' :
                    field === 'yearsOfExperience' ? 'experience' :
                    field === 'bio' ? 'description' :
                    field
                );
                if (element) {
                    FormValidator.showError(element, message);
                }
            });
            // Показываем першу помилку
            const firstError = Object.values(errors)[0];
            errorMessage.textContent = firstError;
            errorAlert.classList.remove('d-none');
            loadingSpinner.classList.add('d-none');
            return;
        }

        try {
            // Подготовка данных для отправки
            const userData = {
                email: formData.email,
                password: formData.password,
                firstName: formData.firstName,
                lastName: formData.lastName,
                phone: formData.phone,
                role: formData.role
            };

            const tutorData = {
                userId: 0, // Будет установлено после создания пользователя
                bio: formData.bio,
                education: formData.education,
                yearsOfExperience: formData.yearsOfExperience,
                hourlyRate: formData.hourlyRate,
                teachingStyle: formData.teachingStyle,
                subjectIds: formData.subjectIds
            };

            // Логируем данные перед отправкой
            console.log('Дані для реєстрації користувача:', userData);
            console.log('Дані для реєстрації репетитора:', tutorData);

            // Регистрация пользователя
            const user = await apiClient.registerUser(userData);
            console.log('Відповідь сервера (користувач):', user);

            // Регистрация репетитора
            tutorData.userId = user.id;
            const tutor = await apiClient.registerTutor(tutorData);
            console.log('Відповідь сервера (репетитор):', tutor);

            // Сохраняем данные пользователя
            localStorage.setItem('user', JSON.stringify(user));
            localStorage.setItem('tutor', JSON.stringify(tutor));

            // Показываем сообщение об успехе
            alert('Реєстрація успішна!');
            
            // Перенаправляем на главную страницу
            window.location.href = '../main/index.html';
        } catch (error) {
            console.error('Ошибка при регистрации:', error);
            errorMessage.textContent = error.message || 'Помилка при реєстрації. Спробуйте ще раз.';
            errorAlert.classList.remove('d-none');
        } finally {
            loadingSpinner.classList.add('d-none');
        }
    });
}); 