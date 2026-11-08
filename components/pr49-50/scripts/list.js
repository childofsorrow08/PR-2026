'use strict';

const status = document.getElementById('status');
const body = document.getElementById('postings');
const refresh_button = document.getElementById('refresh');
let busy = false;

function action(label, handler) {
    const button = document.createElement('button');
    button.type = 'button';
    button.textContent = label;
    button.addEventListener('click', handler);
    return button;
}

function render(data) {
    body.replaceChildren();
    const columns = document.querySelectorAll('th[x-column]');

    for (const posting of data) {
        const row = document.createElement('tr');
        row.tabIndex = 0;
        row.addEventListener('click', () => {
            for (const item of body.children) item.classList.remove('selected');
            row.classList.add('selected');
        });

        for (const column of columns) {
            const field = column.getAttribute('x-column');
            const cell = document.createElement('td');
            cell.dataset.label = column.textContent;
            cell.textContent = field === 'deliveryType' ? delivery_name(posting[field]) : posting[field] ?? '—';
            row.appendChild(cell);
        }

        const actions = document.createElement('td');
        actions.dataset.label = 'Дії';
        actions.className = 'actions';
        actions.appendChild(action('Деталі', () => { location.href = `details.html?id=${posting.id}`; }));
        actions.appendChild(action('Редагувати', () => { location.href = `posting-edit.html?id=${posting.id}`; }));
        actions.appendChild(action('Видалити', () => remove(posting.id)));
        row.appendChild(actions);
        body.appendChild(row);
    }
}

async function refresh() {
    if (busy) return;
    busy = true;
    refresh_button.disabled = true;
    status.textContent = 'Завантаження…';

    try {
        const data = await request('/postings');
        if (!Array.isArray(data)) throw new Error('Сервер повернув некоректний список.');
        render(data);
        status.textContent = data.length ? `Відправлень: ${data.length}` : 'Відправлень поки немає.';
    } catch (error) {
        body.replaceChildren();
        status.textContent = error.message;
    } finally {
        busy = false;
        refresh_button.disabled = false;
    }
}

async function remove(id) {
    if (busy || !confirm(`Видалити відправлення №${id}? Цю дію неможливо скасувати.`)) return;
    busy = true;
    for (const button of document.querySelectorAll('button')) button.disabled = true;
    let message;

    try {
        await request(`/postings/${id}`, 'DELETE');
        message = 'Відправлення видалено.';
    } catch (error) {
        message = error.message;
    } finally {
        busy = false;
        for (const button of document.querySelectorAll('button')) button.disabled = false;
    }

    await refresh();
    status.textContent = message + ' ' + status.textContent;
}

refresh_button.addEventListener('click', refresh);
refresh();
