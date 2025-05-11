class ApiClient {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
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