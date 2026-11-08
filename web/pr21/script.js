'use strict';

{
    const expressions = [
        ['"10" + 5', '10' + 5],
        ['"10" - 5', '10' - 5],
        ['"10" * "2"', '10' * '2'],
        ['10 + true', 10 + true],
        ['10 + false', 10 + false],
        ['Number("125 грн")', Number('125 грн')],
        ['Number("")', Number('')],
        ['Boolean("false")', Boolean('false')],
        ['Boolean("")', Boolean('')]
    ];

    const lines = [];

    for (const [expression, value] of expressions) {
        console.log(expression, value, typeof value);
        lines.push(`${expression} → ${String(value)} (${typeof value})`);
    }

    const products = [
        { name: 'Зошит', price: 40, quantity: 3 },
        { name: 'Ручка', price: 15, quantity: 2 },
        { name: 'Книга', price: 250, quantity: 1 }
    ];

    const vat_rate = 0.2;
    const vat_results = [];

    function ItemCostCalculator(price, quantity) {
        return Math.round(price * quantity * 100) / 100;
    }

    function OrderSumCalculator(items, rate, taxes) {
        let total = 0;
        taxes.length = 0;

        for (const item of items) {
            const cost = ItemCostCalculator(item.price, item.quantity);
            const vat = Math.round(cost * rate * 100) / 100;

            taxes.push({ name: item.name, vat });
            total += cost + vat;
        }

        return Math.round(total * 100) / 100;
    }

    const total = OrderSumCalculator(products, vat_rate, vat_results);

    console.log('Загальна вартість замовлення з ПДВ:', total);
    console.log('ПДВ за позиціями:', vat_results);

    lines.push(
        '',
        'Ціни товарів задано без ПДВ; ставка — 20%.',
        `Загальна вартість із ПДВ: ${total.toFixed(2)} грн`,
        JSON.stringify(vat_results, null, 2)
    );

    document.getElementById('output').textContent = lines.join('\n');
}
