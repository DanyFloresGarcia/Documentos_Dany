const skills: (string)[] = ['jojo', '9', 'true'];


interface Character {
    name: string;
    edad: number;
    soltero: boolean;
    fecha: Date;
    talla: object[];
    carro: string[];
}

const objeto : Character = {
    name: "dany",
    edad: 27,
    soltero: true,
    fecha: new Date(),
    talla: [{numero: 1},{numero: 2}],
    carro: skills
}

console.table(objeto);
export {};