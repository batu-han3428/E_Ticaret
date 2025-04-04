import { ensureCsrfToken } from './security.js';

async function apiRequest(url, method = 'GET', body = null) {

    await ensureCsrfToken();

    const options = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include' 
    };

    if (body) {
        options.body = JSON.stringify(body);
    }

    try {
        const response = await fetch(url, options);

        if (!response.ok) {
            throw new Error(`API isteği başarısız: ${response.statusText}`);
        }

        return await response.json(); 
    } catch (error) {
        console.error("API isteği sırasında hata:", error);
        throw error; 
    }
}

export { apiRequest };
