export function WhatMyTypes<T>(argument: T): T{
    return argument;
}

let string = WhatMyTypes<string>("jojo jojo").split(' ');
let number = WhatMyTypes<number>(20);
let object = WhatMyTypes<object>({name: "dany"});

console.log({string, number, object});