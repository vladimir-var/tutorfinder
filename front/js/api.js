class ApiClient {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
    }

    getHeaders() {
        const headers = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };

        const token = localStorage.getItem('token');
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        return headers;
    }

    async get(endpoint) {
        try {
            const response = await fetch(`${this.baseUrl}${endpoint}`, {
                method: 'GET',
                headers: this.getHeaders()
            });

            if (!response.ok) {
                let errorText = await response.text();
                let errorMessage;
                try {
                    errorMessage = JSON.parse(errorText).message;
                } catch {
                    errorMessage = errorText;
                }
                throw new Error(errorMessage || 'Ошибка при выполнении запроса');
            }

            return response;
        } catch (error) {
            console.error('Ошибка при выполнении запроса:', error);
            throw error;
        }
    }

    async post(endpoint, data) {
        try {
            const isString = typeof data === 'string';
            const response = await fetch(`${this.baseUrl}${endpoint}`, {
                method: 'POST',
                headers: this.getHeaders(),
                body: isString ? data : JSON.stringify(data)
            });

            if (!response.ok) {
                let errorText = await response.text();
                let errorMessage;
                try {
                    errorMessage = JSON.parse(errorText).message;
                } catch {
                    errorMessage = errorText;
                }
                throw new Error(errorMessage || 'Ошибка при выполнении запроса');
            }

            return response;
        } catch (error) {
            console.error('Ошибка при выполнении запроса:', error);
            throw error;
        }
    }

    async delete(endpoint) {
        try {
            const response = await fetch(`${this.baseUrl}${endpoint}`, {
                method: 'DELETE',
                headers: this.getHeaders()
            });

            if (!response.ok) {
                let errorText = await response.text();
                let errorMessage;
                try {
                    errorMessage = JSON.parse(errorText).message;
                } catch {
                    errorMessage = errorText;
                }
                throw new Error(errorMessage || 'Ошибка при выполнении запроса');
            }

            return response;
        } catch (error) {
            console.error('Ошибка при выполнении запроса:', error);
            throw error;
        }
    }

    async registerUser(userData) {
        try {
            const response = await fetch(`${this.baseUrl}/api/users`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(userData)
            });

            if (!response.ok) {
                let errorText = await response.text();
                let errorMessage;
                try {
                    errorMessage = JSON.parse(errorText).message;
                } catch {
                    errorMessage = errorText;
                }
                throw new Error(errorMessage || 'Ошибка при регистрации пользователя');
            }

            return response.json();
        } catch (error) {
            console.error('Ошибка при регистрации пользователя:', error);
            throw error;
        }
    }

    async registerTutor(tutorData) {
        try {
            const response = await fetch(`${this.baseUrl}/api/tutors`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(tutorData)
            });

            if (!response.ok) {
                let errorText = await response.text();
                let errorMessage;
                try {
                    errorMessage = JSON.parse(errorText).message;
                } catch {
                    errorMessage = errorText;
                }
                throw new Error(errorMessage || 'Ошибка при регистрации репетитора');
            }

            return response.json();
        } catch (error) {
            console.error('Ошибка при регистрации репетитора:', error);
            throw error;
        }
    }

    async getSubjects() {
        try {
            const response = await fetch(`${this.baseUrl}/api/subjects`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            });

            if (!response.ok) {
                const error = await response.json();
                throw new Error(error.message || 'Ошибка при получении списка предметов');
            }

            return response.json();
        } catch (error) {
            console.error('Ошибка при получении списка предметов:', error);
            throw error;
        }
    }
}

// Создаем экземпляр ApiClient с базовым URL
const apiClient = new ApiClient('https://localhost:7274'); 