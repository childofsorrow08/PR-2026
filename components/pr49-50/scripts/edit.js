'use strict';

const form = document.getElementById('posting-form');
const status = document.getElementById('status');
const save_button = document.getElementById('save');
const fields = [...form.querySelectorAll('input, select, textarea')];
const id_text = new URLSearchParams(location.search).get('id');
const id = id_text === null ? null : Number(id_text);

function clear_error(field) {
    field.classList.remove('invalid');
    field.removeAttribute('aria-invalid');
    document.getElementById(`${field.id}-error`).textContent = '';
}

function show_error(field, message) {
    field.classList.add('invalid');
    field.setAttribute('aria-invalid', 'true');
    document.getElementById(`${field.id}-error`).textContent = '⚠ ' + message;
}

function validate_field(field) {
    clear_error(field);
    const value = field.value.trim();
    let message = '';

    if (field.required && !value) message = 'Заповніть поле.';
    else if (['from', 'to'].includes(field.id) && value.length < 2) message = 'Введіть щонайменше 2 символи.';
    else if (field.type === 'number' && value !== '') {
        const number = Number(value);
        if (!Number.isFinite(number)) message = 'Введіть скінченне число.';
        else if (field.id === 'weight' && number <= 0) message = 'Вага має бути додатною.';
        else if (number < 0) message = 'Значення не може бути від’ємним.';
        else if (['width', 'height', 'depth'].includes(field.id) && !Number.isInteger(number))
            message = 'Розмір має бути цілим числом.';
    }

    if (!message && field.id === 'weight' && form.elements.deliveryType.value === '0' && Number(value) > 20)
        message = 'Максимальна вага для відділення — 20 кг.';
    if (!message && field.id === 'deliveryType' && !['0', '1', '2'].includes(value))
        message = 'Оберіть спосіб доставки.';

    if (message) show_error(field, message);
    return !message;
}

for (const field of fields) {
    field.addEventListener('input', () => clear_error(field));
    field.addEventListener('change', () => {
        validate_field(field);
        if (field.id === 'deliveryType') validate_field(form.elements.weight);
    });
}

function read_form() {
    const model = {};
    for (const field of fields) {
        model[field.name] = field.type === 'number' || field.tagName === 'SELECT'
            ? field.value === '' ? null : Number(field.value)
            : field.value.trim();
    }
    if (id !== null) model.id = id;
    return model;
}

async function save() {
    if (save_button.disabled) return;
    let valid = true;
    for (const field of fields) valid = validate_field(field) && valid;

    if (!valid) {
        status.textContent = 'Виправте виділені поля.';
        form.querySelector('.invalid')?.focus();
        return;
    }

    save_button.disabled = true;
    status.textContent = 'Збереження…';

    try {
        await request(id === null ? '/postings' : `/postings/${id}`, id === null ? 'POST' : 'PUT', read_form());
        location.href = 'posting-list.html';
    } catch (error) {
        for (const [name, messages] of Object.entries(error.errors ?? {})) {
            const field = fields.find(item => item.name.toLowerCase() === name.toLowerCase());
            if (field) show_error(field, messages.join(' '));
        }
        status.textContent = error.message;
        if (error.status === 404) status.textContent += ' Натисніть «Скасувати», щоб повернутися до списку.';
        save_button.disabled = false;
    }
}

async function load() {
    if (id === null) return;
    document.getElementById('page-title').textContent = 'Редагування відправлення';
    save_button.disabled = true;

    if (!/^[1-9]\d*$/.test(id_text) || !Number.isSafeInteger(id) || id > 2147483647) {
        status.textContent = 'Некоректний номер відправлення.';
        return;
    }

    try {
        const model = await request(`/postings/${id}`);
        for (const field of fields) field.value = model[field.name] ?? '';
        document.getElementById('price').textContent = `Вартість пересилання: ${model.price.toFixed(2)} грн`;
        save_button.disabled = false;
    } catch (error) {
        status.textContent = error.message;
    }
}

save_button.addEventListener('click', save);
form.addEventListener('submit', event => { event.preventDefault(); save(); });
document.getElementById('cancel').addEventListener('click', () => { location.href = 'posting-list.html'; });
load();
