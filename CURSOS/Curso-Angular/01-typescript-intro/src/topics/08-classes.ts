export class Person{
    constructor(public nameReal: string, public lastName: string, public edad?:number){
    }
}
/*
export class Client extends Person {
    constructor(
        public name: string, 
        public ruc:string = "34343454545", 
        private address: string = "Av el muro", 
        private city:string = "Lima", 
        public site: string = name+": " + city + " " + address)
        {
            super(name, 19)
        }
}*/

export class Client {
    constructor(
        public nameClient: string, 
        public ruc:string,
        public Person: Person
    ){}
}

const Dany = new Person("Dany", "Flores", 10);

const client = new Client("Dany", "34343454545", Dany);

console.log(client);