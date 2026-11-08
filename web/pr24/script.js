'use strict';

class Parcel {
    #weight;
    static #count = 0;

    constructor(recipient, weight, length, width, height) {
        if (typeof recipient !== 'string' || !recipient.trim()) {
            throw new Error('Вкажіть одержувача.');
        }

        for (const size of [length, width, height]) {
            if (typeof size !== 'number' || !Number.isFinite(size) || size <= 0) {
                throw new Error('Розміри мають бути додатними числами.');
            }
        }

        this.recipient = recipient;
        this.weight = weight;
        this.length = length;
        this.width = width;
        this.height = height;
        Parcel.#count++;
    }

    get weight() { return this.#weight; }

    set weight(value) {
        if (typeof value !== 'number' || !Number.isFinite(value) || value <= 0) {
            throw new Error('Вага має бути додатним числом.');
        }
        this.#weight = value;
    }

    get volumetricWeight() { return this.length * this.width * this.height / 5000; }
    get chargeableWeight() { return Math.max(this.weight, this.volumetricWeight); }

    describe() {
        return `${this.recipient}: ${this.length} × ${this.width} × ${this.height} см; ` +
            `фактична вага ${this.weight} кг, об'ємна ${this.volumetricWeight} кг, ` +
            `розрахункова ${this.chargeableWeight} кг`;
    }

    static getCount() { return Parcel.#count; }
}

class ParcelPriceCalculator {
    constructor(ratePerKg) { this.ratePerKg = ratePerKg; }
    calculate(parcel) { return parcel.chargeableWeight * this.ratePerKg; }
    getName() { return 'Базовий тариф'; }
}

class PostOfficeParcelPriceCalculator extends ParcelPriceCalculator {
    constructor() { super(15); }
    getName() { return 'Відділення пошти'; }
}

class CourierParcelPriceCalculator extends ParcelPriceCalculator {
    constructor() { super(30); }
    getName() { return 'Кур\'єр'; }
}

class ExpressParcelPriceCalculator extends ParcelPriceCalculator {
    constructor() { super(30); }
    calculate(parcel) { return super.calculate(parcel) + parcel.chargeableWeight * 25; }
    getName() { return 'Експрес'; }
}

const parcels = [
    new Parcel('Іван', 5, 10, 20, 30),
    new Parcel('Олена', 1, 40, 50, 30)
];

const calculators = [
    new PostOfficeParcelPriceCalculator(),
    new CourierParcelPriceCalculator(),
    new ExpressParcelPriceCalculator()
];

const lines = [];

for (const parcel of parcels) {
    console.log(parcel.describe());
    lines.push(parcel.describe());

    for (const calculator of calculators) {
        const line = `${calculator.getName()}: ${calculator.calculate(parcel).toFixed(2)} грн`;
        console.log(line);
        lines.push(line);
    }
}

console.log('Кількість відправлень:', Parcel.getCount());
lines.push(`Кількість відправлень: ${Parcel.getCount()}`);
document.getElementById('output').textContent = lines.join('\n');
