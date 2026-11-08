'use strict';

const api_url = 'http://localhost:5000';

class ApiError extends Error {
    constructor(status, message, errors = {}) {
        super(message);
        this.status = status;
        this.errors = errors;
    }
}

async function request(path, method = 'GET', model) {
    const options = { method };

    if (model !== undefined) {
        options.headers = { 'Content-Type': 'application/json' };
        options.body = JSON.stringify(model);
    }

    let response;
    try {
        response = await fetch(api_url + path, options);
    } catch {
        throw new Error('Не вдалося з’єднатися із сервером. Перевірте підключення й повторіть дію.');
    }

    if (!response.ok) {
        const problem = await response.json().catch(() => ({}));
        const message = response.status === 404
            ? 'Відправлення вже відсутнє. Оновіть список.'
            : problem.message ?? problem.title ?? `Помилка сервера: ${response.status}.`;
        throw new ApiError(response.status, message, problem.errors);
    }

    return response.status === 204 ? null : response.json();
}

function delivery_name(value) {
    return ['Відділення', 'Кур’єр', 'Експрес-кур’єр'][value] ?? 'Невідомо';
}
