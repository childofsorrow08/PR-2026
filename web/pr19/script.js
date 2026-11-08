'use strict';

const form = document.getElementById('grades-form');
const result = document.getElementById('result');

function calculateAverage(grades) {
    if (!Array.isArray(grades) || grades.length === 0) {
        throw new Error('Введіть непорожній масив оцінок.');
    }

    let sum = 0;

    for (const grade of grades) {
        if (typeof grade !== 'number' || !Number.isFinite(grade) || grade < 0 || grade > 100) {
            throw new Error('Кожна оцінка має бути числом від 0 до 100.');
        }
        sum += grade;
    }

    return sum / grades.length;
}

form.addEventListener('submit', event => {
    event.preventDefault();

    try {
        const average = calculateAverage(JSON.parse(document.getElementById('grades').value));
        result.textContent = `Середня оцінка: ${average.toFixed(2)}`;
    } catch (error) {
        result.textContent = error instanceof SyntaxError ? 'Некоректний JSON масиву.' : error.message;
    }
});
