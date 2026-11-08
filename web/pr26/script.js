'use strict';

const status = document.getElementById('status');
const body = document.getElementById('postings');

async function loadData() {
    const base_url = document.getElementById('api-url').value.replace(/\/+$/, '');
    const response = await fetch(`${base_url}/postings`);

    if (!response.ok) throw new Error(`Сервер повернув HTTP ${response.status}.`);

    const data = await response.json();
    if (!Array.isArray(data)) throw new Error('Сервер повернув дані не у форматі списку.');

    console.log('Відправлення:', data);
    return data;
}

function createCell(field_name, model) {
    const cell = document.createElement('td');
    const delivery_names = ['Відділення', 'Кур\'єр', 'Експрес-кур\'єр'];

    cell.textContent = field_name === 'deliveryType'
        ? (delivery_names[model[field_name]] ?? String(model[field_name] ?? ''))
        : String(model[field_name] ?? '');

    return cell;
}

function renderTable(data) {
    body.replaceChildren();
    const columns = document.getElementById('table-header').querySelectorAll('th[x-column]');

    for (const model of data) {
        const row = document.createElement('tr');

        for (const column of columns) {
            row.appendChild(createCell(column.getAttribute('x-column'), model));
        }

        body.appendChild(row);
    }
}

async function refresh() {
    const button = document.getElementById('refresh');
    button.disabled = true;
    status.textContent = 'Завантаження…';

    try {
        const data = await loadData();
        renderTable(data);

        status.textContent = data.length
            ? `Завантажено відправлень: ${data.length}`
            : 'Відправлень поки немає.';
    } catch (error) {
        body.replaceChildren();
        status.textContent = `Не вдалося завантажити список. ${error.message} ` +
            'Перевірте адресу сервера та CORS.';
    } finally {
        button.disabled = false;
    }
}

document.getElementById('refresh').addEventListener('click', refresh);
refresh();
