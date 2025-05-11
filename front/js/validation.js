class FormValidator {
    static validateEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    static validatePassword(password) {
        // Минимум 8 символов, хотя бы одна буква и одна цифра
        const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;
        return passwordRegex.test(password);
    }

    static validatePhone(phone) {
        // Проверяем, что номер начинается с +380 и имеет правильную длину
        return phone.startsWith('+380') && phone.length === 13;
    }

    static validatePrice(price) {
        const numPrice = parseFloat(price);
        return !isNaN(numPrice) && numPrice >= 0 && numPrice <= 10000 && Number.isInteger(numPrice);
    }

    static validateRequired(value) {
        return value !== null && value !== undefined && value.trim() !== '';
    }

    static validateExperience(years) {
        return !isNaN(years) && years >= 0 && years <= 50;
    }

    static showError(element, message) {
        const errorDiv = document.createElement('div');
        errorDiv.className = 'invalid-feedback';
        errorDiv.textContent = message;
        
        element.classList.add('is-invalid');
        element.parentNode.appendChild(errorDiv);
    }

    static clearError(element) {
        element.classList.remove('is-invalid');
        const errorDiv = element.parentNode.querySelector('.invalid-feedback');
        if (errorDiv) {
            errorDiv.remove();
        }
    }

    static validateForm(formData) {
        const errors = {};

        // Валидация email
        if (!this.validateEmail(formData.email)) {
            errors.email = 'Введите корректный email адрес';
        }

        // Валидация пароля
        if (!this.validatePassword(formData.password)) {
            errors.password = 'Пароль должен содержать минимум 8 символов, включая буквы и цифры';
        }

        // Проверка совпадения паролей
        if (formData.password !== formData.confirmPassword) {
            errors.confirmPassword = 'Пароли не совпадают';
        }

        // Валидация телефона
        if (!this.validatePhone(formData.phone)) {
            errors.phone = 'Номер телефона должен быть в формате +380XXXXXXXXX';
        }

        // Валідація ціни
        if (!this.validatePrice(formData.hourlyRate)) {
            errors.hourlyRate = 'Ціна повинна бути цілим числом від 0 до 10000';
        }

        // Валідація досвіду
        if (!this.validateExperience(formData.yearsOfExperience)) {
            errors.yearsOfExperience = 'Досвід повинен бути від 0 до 50 років';
        }

        // Валідація опису
        if (!this.validateRequired(formData.bio)) {
            errors.bio = 'Опис обовʼязковий для заповнення';
        }

        // Проверка обязательных полей
        const requiredFields = ['firstName', 'lastName', 'education', 'bio'];
        requiredFields.forEach(field => {
            if (!this.validateRequired(formData[field])) {
                errors[field] = 'Це поле обовʼязкове для заповнення';
            }
        });

        console.log('Значення опису (bio):', document.getElementById('description')?.value, '| formData.bio:', formData.bio);

        return errors;
    }
} 