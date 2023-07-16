/*let boxelements = document.getElementsByClassName("box")
document.getElementById("btnbiru").addEventListener("click", () => {
    Array.from(boxelements).forEach(elem => {
        elem.style.backgroundColor = "deepskyblue"
        elem.classList.remove("border-danger")
        elem.classList.remove("border-success")
        elem.classList.add("border-primary")
    })
})
document.getElementById("btnbiru").addEventListener("mouseenter", () => {
    Array.from(boxelements).forEach(elem => {
        elem.classList.remove("border-danger")
        elem.classList.remove("border-success")
        elem.classList.add("border-primary")
    })
})
document.getElementById("btnbiru").addEventListener("mouseleave", () => {
    Array.from(boxelements).forEach(elem => {
        elem.style.backgroundColor = "transparent"
        elem.classList.remove("border-danger")
        elem.classList.remove("border-success")
        elem.classList.remove("border-primary")
    })
})


document.getElementById("btnhijau").addEventListener("click", () => {
    Array.from(boxelements).forEach(elem => {
        elem.style.backgroundColor = "lightgreen"
        elem.classList.remove("border-danger")
        elem.classList.add("border-success")
        elem.classList.remove("border-primary")
    })
})
document.getElementById("btnhijau").addEventListener("mouseenter", () => {
    Array.from(boxelements).forEach(elem => {
        elem.classList.remove("border-danger")
        elem.classList.add("border-success")
        elem.classList.remove("border-primary")
    })
})
document.getElementById("btnhijau").addEventListener("mouseleave", () => {
    Array.from(boxelements).forEach(elem => {
        elem.style.backgroundColor = "transparent"
        elem.classList.remove("border-danger")
        elem.classList.remove("border-success")
        elem.classList.remove("border-primary")
    })
})



document.getElementById("btnmerah").addEventListener("click", () => {
    Array.from(boxelements).forEach(elem => {
        elem.style.backgroundColor = "pink"
        elem.classList.add("border-danger")
        elem.classList.remove("border-success")
        elem.classList.remove("border-primary")
    })
})
document.getElementById("btnmerah").addEventListener("mouseenter", () => {
    Array.from(boxelements).forEach(elem => {
        elem.classList.add("border-danger")
        elem.classList.remove("border-success")
        elem.classList.remove("border-primary")
    })
})
document.getElementById("btnmerah").addEventListener("mouseleave", () => {
    Array.from(boxelements).forEach(elem => {
        elem.style.backgroundColor = "transparent"
        elem.classList.remove("border-danger")
        elem.classList.remove("border-success")
        elem.classList.remove("border-primary")
    })
})


let arrayMhsObj = 
    { nama: "budi", nim: "a112015", umur: 20, isActive: true, fakultas: { name: "komputer" } },
    { nama: "joko", nim: "a112035", umur: 22, isActive: false, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112020", umur: 21, isActive: true, fakultas: { name: "komputer" } },
    { nama: "herul", nim: "a112032", umur: 25, isActive: true, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112040", umur: 21, isActive: true, fakultas: { name: "komputer" } },
]

let fakultas = []
for (var i = 0; i < arrayMhsObj.length; i++) {
    if (arrayMhsObj[i].fakultas.name === 'komputer') {
        fakultas.push(arrayMhsObj[i])
    }
}

console.log(fakultas)

for (var i = 0; i < arrayMhsObj.length; i++) {
    if (arrayMhsObj[i].nim.substring(5) >= '30')
    {
        arrayMhsObj[i].isActive = false
    }

    else
    {
        arrayMhsObj[i].isActive = true
    }
}

    console.log(arrayMhsObj)


console.log("Latihan Javascript")

let judul = document.getElementById("judul");
let p1 = document.getElementsByTagName("p");

judul.onclick = () => {
judul.style.backgroundColor = "cyan";
judul.innerHTML = "Saya ubah dari JS";
}

let query = document.querySelector("li:nth-child(2)");


//query.onclick = () => {
//    p1[0].style.backgroundColor = "pink";
//    query.style.backgroundColor = "pink";
//}

//query.onclick = () => {
//    p1[0].innerHTML = "Berubahhh!";
//    query.innerHTML = "Berubahhhhhh!!!";
//}

query.addEventListener('click', () => {
p1[0].style.backgroundColor = "pink";
query.style.backgroundColor = "pink";
});

query.addEventListener('click', () => {
p1[0].innerHTML = "Berubahhh!";
query.innerHTML = "Berubahhhhhh!!!";
});

//array => sekumpulan variable dengan tipe data yang sama dan nama yang sama,
//int nilai[5] => [50,60,80,90,90]

let array = [1, 2, 3, 4, 5];
//console.log(array);
//array.push("testing")
//console.log(array);
//array.pop();
//console.log(array);
//array.unshift("haloo");
//console.log(array[0]);
//array.shift();
//console.log(array);

//array multidimensi
let arrayMulti = [1, 2, 3, ['a', 'b', 'c', true], false];
//console.log(arrayMulti);
//console.log(arrayMulti[3][3]);

//object vs array ?? => object = key and value, array = index
// {} vs []
let obj = {};
obj.testing = "halo";
obj.number = 50;
console.log(obj);

//cara manual
let user = {
test: obj.testing,
testnumber: obj.number,
username: "iniusername",
password: "inipasword"
}

console.log(user)

//dengan spread operator
let spread = {
...obj,
spread: "test",
spreadnumber: 90
}

console.log(spread);

//arrow function
let hitung = (x, y) => x + y;

//function hitung(x, y) {
//    return x+y
//}

console.log(hitung(5, 9));

//array of object
let arrayMhsObj = [
{ nama: "budi", nim: "a112015", umur: 20, isActive: true, fakultas: { name: "komputer" } },
{ nama: "joko", nim: "a112035", umur: 22, isActive: false, fakultas: { name: "ekonomi" } },
{ nama: "herul", nim: "a112020", umur: 21, isActive: true, fakultas: { name: "komputer" } },
{ nama: "herul", nim: "a112032", umur: 25, isActive: true, fakultas: { name: "ekonomi" } },
{ nama: "herul", nim: "a112040", umur: 21, isActive: true, fakultas: { name: "komputer" } },
]

let FakultasKomp = [];

FakultasKomp = arrayMhsObj.filter(mhs => mhs.fakultas.name == "komputer");

console.log(arrayMhsObj);
console.log(FakultasKomp);
*/

$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon/"
}).done((result) => {
    console.log(result.results);
    let temp = "";
    $.each(result.results, (key, val) => {
        temp += `<tr>
                    <td>${key + 1}</td>
                    <td>${val.name}</td>
                    <td>${val.url}</td>
                    <td><button onclick="detail('${val.url}')" data-bs-toggle="modal" data-bs-target="#modal" class="btn btn-primary">Detail</button></td>
                </tr>`;
    })
    $("#tbody").html(temp);

})

function detail(stringURL) {
    $.ajax({
        url: stringURL
    }).done(res => {
        $(".modal-title").html(res.name);
        $("#firstimg").attr("src", res.sprites.other.dream_world.front_default);
        $("#secimg").attr("src", res.sprites.other.home.front_shiny);
        $("#thirdimg").attr("src", res.sprites.other.home.front_default);
        $(".card-title").html(res.name);
        let temp = "";
        $.each(res.types, (key, val) => {
            temp += `<div class="badge bg-primary"> Type : ${val.type.name}
            </div> `;
        });
        let tempAbility = "";
        $.each(res.abilities, (key, val) => {
            tempAbility += `<div class="badge bg-success"> Ability : ${val.ability.name}</div> `;
        })
        $(".card-text").html(temp);
        $("#listpoke").html(tempAbility);
        $("#listpoke1").html(`<div class="badge bg-danger"> Weight : ${res.weight} KG</div> <div class="badge bg-danger"> Height : ${res.height} M</div>`);
        let tempForm = "";
        $.each(res.forms, (key, val) => {
            tempForm += `<div class="badge bg-warning"> Form : ${val.name} </div> <div class="badge bg-warning"> Species : ${res.species.name} </div>  `;
        })
        $("#listpoke2").html(tempForm);
        let tempSpec = "";
        $.each(res.species, (key, val) => {
            $.each(val.url, (key, val2) => {
                tempSpec += `<ul>
            <li>
            ${val.generation.name}
            <li/>
            </ul>`;
            })
        })
           $("#listpoke3").html(res.species.url.description);
        console.log(tempSpec);
        $("#title").html(res.name);
        $("#hp").css("width", res.stats[0].base_stat + "%").html("HP : " + res.stats[0].base_stat);
        $("#attack").css("width", res.stats[1].base_stat + "%").html("Attack : " + res.stats[1].base_stat);
        $("#defense").css("width", res.stats[2].base_stat + "%").html("Defense : " + res.stats[2].base_stat);
        $("#special-attack").css("width", res.stats[3].base_stat + "%").html("Spesial Attack : " + res.stats[3].base_stat);
        $("#special-defense").css("width", res.stats[4].base_stat + "%").html("Spesial Defense : " + res.stats[4].base_stat);
        $("#speed").css("width", res.stats[5].base_stat + "%").html("Speed : " + res.stats[5].base_stat);
        $
    })

}
$(document).ready(function () {
    $('#myTabel').DataTable({
        ajax: {
            url: "https://pokeapi.co/api/v2/pokemon/",
            dataType: "JSON",
            dataSrc: "results" //data source -> butuh array of object
        },
        columns: [
            {
                data: 'url',

                render: function (data, type, row) {
                    let number = data.split('/')[6];
                    return number;
                }
            },
            {data: "name"},
            { data: "url" },
            {
                data: null,
                render: function (data, type, row) {
                    return `<button onclick="detail('${data.url}')" data-bs-toggle="modal" data-bs-target="#modal" class="btn btn-primary">Detail</button>`;
                }
            }

        ]
    });
});