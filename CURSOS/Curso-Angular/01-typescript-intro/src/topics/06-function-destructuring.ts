export interface Products {
    price: number;
    nombre: string;
}

interface TaxCalculationOptions {
    tax: number;
    productos: Products[];
}

export function taxCalculation(options: TaxCalculationOptions) : [number, number]{
    const {tax, productos} = options;
    let total = 0;
     
    productos.forEach(({price}) =>{
        total += price;
    });
    
    return [total, total * tax];
}

const [total, subtotal] = taxCalculation({
    tax: 0.15,
    productos: [{price: 50, nombre: "camisa"}, {price: 50, nombre: "pantalon"}]
});

console.log({total, subtotal});
export {};
