const meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];
const dias = ['Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado', 'Domingo'];
const abr = ['Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa']
var dt = InicializarFecha();

ActualizarSemana();
ActualizarBarra();
var timer = setInterval(ActualizarBarra, 60000);

function InicializarFecha() {
    var fecha = new Date();
    fecha.setHours(0, 0, 0, 0);

    if (fecha.getDay()==0)
        fecha.setDate(fecha.getDate() + 1)

    while (fecha.getDay() != 1)
        fecha.setDate(fecha.getDate() - 1);

    return fecha;
}

function ActualizarBarra() {
    var fecha_lim = new Date(dt)
    if (!((dt <= new Date() < fecha_lim) && new Date().getDay() != 0)) return;

    if ($('#barra')) $('#barra').remove();

    if (dt.getHours() < 7 || dt.getHours() > 21) {
        if (dt.getHours() < 7)
            var template = `<div id="barra" style="display:block; position: absolute; background-color:red; height:2px; width:100%; top: 0%"></div>`;
        else
            var template = `<div id="barra" style="display:block; position: absolute; background-color:red; height:2px; width:100%; top: 100%;"></div>`;
    }
    else
        var template = `<div id="barra" style="display:block; position: absolute; background-color:red; height:2px; width:100%; top: calc(${(new Date().getMinutes() / 0.6)}%);"></div>`;

    $(`#${abr[new Date().getDay() - 1] + new Date().getHours()}`).append(template);
}

function ActualizarSemana() {
    var sala_id = $('#sala').prop('selectedIndex') == undefined ? 1 : $('#sala').prop('selectedIndex') + 1;

    $.ajax({
        url: '/_Calendario/GetReservaciones',
        data: { sala: sala_id, dt: dt.toDateString() },
        type: "GET",
        success: function (response) {
            $('button.' + 'reserv').remove();
            for (reservacion of response) {
                for (id of reservacion["targetIds"]) {

                    var template = `<button class="reserv" id="${reservacion["id"]}" onclick="ReservacionDetalle(this.id);" style="background-color:${reservacion["backgroundColor"]};">
                                <div style="margin-bottom:10px;">${reservacion["nombre"]}</div>
                                <div style="font-weight: 500;">${reservacion["curso"]}</div>
                                </button>`

                    $(`#${id}`).append(template)
                }
            }
        }
    });

    $('#ano').html(dt.getFullYear());

    var fecha_inicio = new Date(dt);
    for (let i = 0; i <= 6; i++) {
        $(`#${abr[i]}`).html(dias[i] + ' ' + fecha_inicio.getDate());
        fecha_inicio.setDate(fecha_inicio.getDate() + 1);
    }
    
    if (dt.getMonth() == fecha_inicio.getMonth())
        $('#mes').html(meses[dt.getMonth()]);
    else
        $('#mes').html(meses[dt.getMonth()] + '/' + meses[fecha_inicio.getMonth()]);

    ActualizarBarra();
}