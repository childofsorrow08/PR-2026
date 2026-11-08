'use strict';

const status = document.getElementById('status');
const details = document.getElementById('details');
const id = new URLSearchParams(location.search).get('id');

async function load() {
    if (!id || !/^[1-9]\d*$/.test(id) || Number(id) > 2147483647) {
        status.textContent = 'Некоректний номер відправлення.';
        return;
    }

    try {
        const model = await request(`/postings/${id}`);
        const labels = {
            id: 'Номер', from: 'Відправник', to: 'Одержувач', content: 'Вміст',
            deliveryType: 'Доставка', weight: 'Вага, кг', width: 'Ширина, см',
            height: 'Висота, см', depth: 'Довжина, см', value: 'Оголошена вартість, грн',
            price: 'Вартість пересилання, грн', createdAt: 'Створено'
        };

        for (const [field, label] of Object.entries(labels)) {
            const name = document.createElement('dt');
            const value = document.createElement('dd');
            name.textContent = label;
            value.textContent = field === 'deliveryType' ? delivery_name(model[field]) : model[field] ?? '—';
            details.append(name, value);
        }
        status.textContent = '';
    } catch (error) {
        status.textContent = error.message;
    }
}

load();
