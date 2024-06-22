import { address } from "./03-functions";

interface Address {
    street: string;
    country: string;
    city: string;    
}

interface SuperHero {
    name: string;
    age: number;
    address: Address;
    showAddress: () => string;
    
}

export const superHeroe: SuperHero = {
    name: 'Spiderman',
    age: 30,
    address: {
        street: 'Main St',
        country: 'USA',
        city: 'NY'
    },
    showAddress() {
        return this.name + ', ' + this.address.city + ', ' + this.address.country;
    }
}

const {age:agejojo, name: nombre, address: {city}} = superHeroe;

//destructuracion de arreglos

const personajes: string[] = ['Goku', 'Vegeta', 'Gohan'];
const Piccolo = personajes[5] || 'No existe Piccolo';
const Goku = personajes[0] || 'No existe Goku';

console.log({agejojo,nombre, city, Goku, Piccolo});

export {};