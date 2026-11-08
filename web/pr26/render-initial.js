'use strict';

// Початковий варіант для порівняння з відображенням за метаданими.
function render_initial_table(data) {
    const body = document.getElementById('postings');
    body.replaceChildren();

    for (const model of data) {
        const row = document.createElement('tr');

        const content = document.createElement('td');
        content.textContent = model.content;
        row.appendChild(content);

        const from = document.createElement('td');
        from.textContent = model.from;
        row.appendChild(from);

        const to = document.createElement('td');
        to.textContent = model.to;
        row.appendChild(to);

        const weight = document.createElement('td');
        weight.textContent = model.weight;
        row.appendChild(weight);

        const price = document.createElement('td');
        price.textContent = model.price;
        row.appendChild(price);

        body.appendChild(row);
    }
}
