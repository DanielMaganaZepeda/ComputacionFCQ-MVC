$('#FormReservacion').load('/_FormReservacion/_FormReservacionPartial');
$(document).ready(function () { AplicarFiltros(); })

function AbrirModal() {
    _FormReservacion_ComponentesDisabled(false);
    _FormReservacion_LimpiarForm();
    _FormUsuario_LimpiarForm();
    $('#programa').val('');
    ActualizarProgramas();

    $('#ModalReservacion').modal('show');
}

function ReservacionDetalle(id) {
    $.ajax({
        url: '/Reservaciones/ReservacionDetalle',
        data: { id: id },
        type: "GET",
        success: function (response) {
            _FormReservacion_CargarDatos(response);
        }
    });
}

function AplicarFiltros() {
    var salas = [], tipos=[], estados=[];

    for (let i = 1; i <= 5; i++) {
        if ($(`#cb_${i}`).prop('checked') == true)
            salas.push(i);
    }
    if (salas.length == 0) {
        alert('Se debe seleccionar al menos una sala');
        return;
    }

    if ($('#cb_Unicas').prop('checked') == true)
        tipos.push('Unicas');
    if ($('#cb_Frecuencias').prop('checked') == true)
        tipos.push('Frecuencias');
    if (tipos.length == 0) {
        alert('Se debe seleccionar al menos un tipo de reservaciones');
        return;
    }

    if ($('#cb_Activas').prop('checked') == true)
        estados.push('Activas');
    if ($('#cb_Canceladas').prop('checked') == true)
        estados.push('Canceladas');
    if (estados.length == 0) {
        alert('Se debe seleccionar al menos un estado de reservaciones');
        return;
    }

    $.ajax({
        url: '/Reservaciones/AplicarFiltros',
        data: { desde: $('#table_desde_value').html(), hasta: $('#table_hasta_value').html(), salas: salas, tipos: tipos, estados: estados },
        type: "POST",
        success: function (response) {
            ActualizarTablaReservaciones(response);
        }
    });
}

function ActualizarTablaReservaciones(response) {
    //Se vacian los elementos que haya en la tabla
    $('#body').html('');
    var template;
    var button = `class="btn btn-outline-primary flex" style="height: 35px; margin-left:auto;" onclick="ReservacionDetalle(this.id)">
                  <i class="bi bi-gear-wide-connected" style="margin-right:10px; font-size:14px;"></i>Ver detalles</button>`;

    for (reservacion of response) {
        template = '<tr style="vertical-align: middle;">'
        template += `<td>${reservacion['sala_id']}</td>`
        template += `<td>${reservacion['curso']}</td>`
        template += `<td>${reservacion['nombre']}</td>`
        template += `<td>${reservacion['tipo']}</td>`

        if (reservacion['tipo'] == 'Unica') {
            template += `<td><a style="margin-right: 10px;">${reservacion['fecha']}</a> ${reservacion['hi']}:00-${reservacion['hf']}:00</td>`
        }
        else {
            template += `<td> ${reservacion['periodo_inicio']} - ${reservacion['periodo_fin']}</td>`
        }
        template += `<td>${reservacion['estado']}</td>`
        template += `<td><button id="${reservacion['id']}" ` + button + `</tr>`; 
        $('#body').append(template);
    }
}

function AdministrarFechas() {
    $.ajax({
        url: '/Reservaciones/GetFechas',
        type: "GET",
        success: function (response) {
            $('#semestre_desde_value').html(response['inicio']);
            $('#semestre_hasta_value').html(response['fin']);
            ActualizarTabla('semestre_desde', null);
            ActualizarTabla('semestre_hasta', null);
        }
    });
    $('#ModalFechas').modal('show');
}

function AplicarCambiosFechas() {
    if (confirm('¿Está seguro de que desea guardar los cambios?')) {
        $.ajax({
            url: '/Reservaciones/ActualizarFechas',
            data: {inicio: $('#semestre_desde_value').html(), fin: $('#semestre_hasta_value').html()},
            type: "POST",
            success: function (response) {
                if (response['success'] == true) {
                    alert('Los cambios han sido aplicados con éxito');
                    $('#ModalFechas').modal('hide');
                }
                else {
                    alert(response['responseText']);
                }
            }
        });
    }
}