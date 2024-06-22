import {Products, taxCalculation} from './06-function-destructuring';

const products: Products[] = [{
    price: 200,
    nombre: "parlante"
},
{
    price: 60,
    nombre: "micro"
}
]

const [resultado1, resultado2] = taxCalculation({tax: 0.2, productos: products});

console.log({resultado1, resultado2});
export {};