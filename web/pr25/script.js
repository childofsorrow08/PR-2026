'use strict';

const value1 = document.getElementById('value1');
const value2 = document.getElementById('value2');
const result = document.getElementById('result');
const history = document.getElementById('history');

function calculate() {
    const a = Number(value1.value);
    const b = Number(value2.value);

    if (!value1.value.trim() || !value2.value.trim() || !Number.isFinite(a) || !Number.isFinite(b)) {
        result.textContent = 'Введіть два скінченні числа.';
        return;
    }

    const sum = a + b;

    if (!Number.isFinite(sum)) {
        result.textContent = 'Сума виходить за допустимий діапазон.';
        return;
    }

    result.textContent = String(sum);

    const entry = document.createElement('li');
    entry.textContent = `${a} + ${b} = ${sum}`;
    history.appendChild(entry);
}
