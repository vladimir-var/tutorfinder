class FormValidator {
    static validateEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    static validatePassword(password) {
        // Минимум 8 символов, минимум 1 цифра, минимум 1 буква
        const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;
        return passwordRegex.test(password);
    }

    static validatePhone(phone) {
        // Поддерживает форматы: +380XXXXXXXXX, 0XXXXXXXXX, XXXXXXXXX
        const phoneRegex = /^(\+380|0)?\d{9}$/;
        return phoneRegex.test(phone);
    }

    static validatePrice(price) {
        return price > 0 && price <= 10000;
    }

    static validateRequired(value) {
        return value.trim() !== '';
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

    static formatPhoneNumber(input) {
        let value = input.value.replace(/\D/g, '');
        if (value.length > 0) {
            if (value.length <= 2) {
                value = '+380' + value;
            } else if (value.length <= 5) {
                value = '+380' + value.slice(0, 2) + ' ' + value.slice(2);
            } else if (value.length <= 8) {
                value = '+380' + value.slice(0, 2) + ' ' + value.slice(2, 5) + ' ' + value.slice(5);
            } else {
                value = '+380' + value.slice(0, 2) + ' ' + value.slice(2, 5) + ' ' + value.slice(5, 8) + ' ' + value.slice(8, 10);
            }
        }
        input.value = value;
    }
} 