import { superHeroe } from "./04-homework-types";

function suma (a: number, b: number) : string{
    return `${a + b}`;
}

function multiplicacion (first: number, second?: number, base: number = 2){
    return first * base;
}

const resultado1: string = suma(5, 4);
const resultado2: string = suma(5, 1);
const resultado3: number = multiplicacion(3)

console.log({resultado1, resultado2, resultado3});

//Curar

interface Character{
    name: string;
    hp: number;
    show: () => void;
}
const curar = (character : Character, amount: number) => {
    character.hp += amount;
}

const character = {
    name: "dany",
    hp: 10,
    show(){
        console.log(`Su vida actual es de ${this.hp}`)
    }
}

curar(character, 50);
curar(character, 20);

console.log(character);

export {};
export const address = superHeroe.showAddress();
