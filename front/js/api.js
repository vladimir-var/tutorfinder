class ApiClient {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
    }

    async registerUser(userData) {
        const response = await fetch(`${this.baseUrl}/api/users`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userData)
        });

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || 'Ошибка при регистрации пользователя');
        }

        return response.json();
    }

    async registerTutor(tutorData) {
        const response = await fetch(`${this.baseUrl}/api/tutors`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(tutorData)
        });

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || 'Ошибка при регистрации репетитора');
        }

        return response.json();
    }

    async getSubjects() {
        const response = await fetch(`${this.baseUrl}/api/subjects`);
        if (!response.ok) {
            throw new Error('Ошибка при получении списка предметов');
        }
        return response.json();
    }
} 