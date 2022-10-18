function Inicializar(id) {
    for (let i = 1; i <= 42; i++) {
        $(`#c${i}`).attr('id', id+'_c_' + i);
    }

    //Para que cuando de click dentro del menu no se cierre
    $(`#${id} .dropdown-menu`).on({
        "click": function (e) {
            e.stopPropagation();
        }
    });

    //La accion que realizara cuando salga del menu
    $(`#${id}`).on('hidden.bs.dropdown', function () {
        ActualizarTabla(id, null);
    })

    ActualizarTabla(id, null);
}

function SetFecha(id, fecha) {
    var dt = new Date(fecha)
    $(`#${id}_value`).html(`${dt.getDate()}/${getMes(dt.getMonth() + 1)}/${dt.getFullYear()}`);
    $(`#${id}_btn`).click();
}

//Accion puede ser 'next','prev', o null
function ActualizarTabla(id, accion) {

    //Se borra todo el contenido de las celdas
    for (var i = 1; i <= 42; i++) {
        $(`#${id}_c_${i}`).html('')
        $(`#${id}_c_${i}`).attr('class','')
    }

    //Obtenemos el valor del mes y del ano que se mostraran
    var mes, ano;

    //Si se recarga la tabla sin accion (cuando se sale del modal en un mes distinto pero no se modifica value)
    if (accion == null) {
        if ($(`#${id}_value`).html()=='Seleccionar fecha') {
            mes = new Date().getMonth() + 1;
            ano = new Date().getFullYear();
        }
        else {
            mes = getMes($(`#${id}_value`).html().split('/')[1]);
            ano = parseInt($(`#${id}_value`).html().split('/')[2]);
        }
    }
    //Cuando se quiere cambiar mes
    else {
        mes = getMes($(`#${id}_mes`).html());
        ano = parseInt($(`#${id}_ano`).html());
        if (accion == 'prev') {
            ano = mes > 1 ? ano : ano - 1;
            mes = mes > 1 ? mes-1 : 12 ;
        }
        if (accion == 'next') {
            ano = mes < 12 ? ano : ano + 1;
            mes = mes < 12 ? mes + 1 : 1;
        }
    }

    //Se pone el header
    $(`#${id}_mes`).html(getMes(mes));
    $(`#${id}_ano`).html(ano);

    //Posicionamos a la fecha en el primer dia del mes
    var dt = new Date(ano + '-' + mes + '-1');

    //Posicionamos en la primera celda que tendra contenido (Domingo==0)
    var c = dt.getDay() + 1;

    while ((dt.getMonth() + 1) == mes) {
        var template = `<button class="flex calendario-fecha" id="${id}_${dt.getDate()}-${getMes(dt.getMonth() + 1)}-${dt.getFullYear()}"
            onclick="$('#${id}_value').html('${dt.getDate()}/${getMes(dt.getMonth() + 1)}/${dt.getFullYear()}'); $('#${id}_btn').click();">${dt.getDate()}</button>`;

        $(`#${id}_c_${c}`).html(template);
        $(`#${id}_c_${c}`).addClass('calendario-cell');

        c++;
        dt.setDate(dt.getDate() + 1);
    }

    //Verificando si dentro de la tabla esta el dia seleccionado
    if ($(`#${id}_value`).html() != 'Seleccionar fecha') {
        if (document.getElementById(id + '_' + document.getElementById(id + '_value').innerHTML.replaceAll('/', '-')) != null) {
            document.getElementById(id + '_' + document.getElementById(id + '_value').innerHTML.replaceAll('/', '-')).className = 'flex calendario-seleccionada';
        }
    }

    //Veriricando si existe Hoy esta en el mes seleccionado
    if (document.getElementById(id + '_' + new Date().getDate() + '-' + getMes(new Date().getMonth()+1) + '-' + new Date().getFullYear()) != null) {
        document.getElementById(id + '_' + new Date().getDate() + '-' + getMes(new Date().getMonth()+1) + '-' + new Date().getFullYear()).style.border = '2px solid dodgerblue'
    }
    
}

function Borrar(id) {
    $(`#${id}_value`).html('Seleccionar fecha');
    $(`#${id}_btn`).click();
}

//Obtiene el nombre del mes dependiendo o el numero del mes dependiendo de la entrada
function getMes(mes) {
    const meses = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
    if (typeof mes == "string") {
        return (meses.indexOf(mes) + 1);
    }
    else {
        return meses[mes-1];
    }
}