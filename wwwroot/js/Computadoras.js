$('#nav-page').html('Computadoras')
$('#nav-icon').addClass('bi-pc-display')

ActualizarComputadoras();

$('#ModalReporte').on('hide.bs.modal', function () {
    ComputadoraDetalle($('#numero').html());
});

ActualizarTablaReportes()

function ActualizarTablaReportes() {
    $.ajax({
        url: '/Computadoras/GetReportes',
        type: "GET",
        success: function (response) {
            $('#tabla_reportes').html('');

            if (response.length == 0) {
                $('#tabla_reportes').hide();
                $('#table_sin_reportes').show();
            }
            else {
                $('#tabla_reportes').show();
                $('#table_sin_reportes').hide();

                for (reporte of response) {
                    let template = '<tr>'
                    template += `<td>${reporte['sala']}</td>`
                    template += `<td>${reporte['numero']}</td>`
                    template += `<td>${reporte['fecha']}</td>`
                    template += `<td>${reporte['detalle']}</td>`
                    template += `<td><button class="btn btn-outline-primary flex" id="${reporte['numero']}" onclick="ComputadoraDetalle(this.id)" style="margin-left:auto;">
                    <i name="information-circle-outline" class="bi bi-gear-wide-connected align-middle" style="font-size: 16px;"></i>Ver detalles</button></td>`

                    $('#tabla_reportes').append(template + '</tr>');
                }
            }
        }
    });
}

function SolucionarReporte(id) {
    if (confirm('¿Está seguro de que desea marcar este reporte como solucionado?')) {
        let sala_id = $('#sala').val() == undefined ? 1 : $('#sala').val();
        $.ajax({
            url: '/Computadoras/SolucionarReporte',
            data: { reporte_id: id, sala_id: sala_id, numero: $('#numero').html() },
            type: "POST",
            success: function (response) {
                if (response['cantidad'] == 0) {
                    alert('El reporte ha sido marcado como solucionado con éxito y la computadora ha vuelto a estar disponible para su uso en sesiones');
                }
                else {
                    alert(`El reporte ha sido marcado como solucionado con éxito, la computadora volverá a estar disponible cuando los ${response['cantidad']} reportes restantes sean marcados como solucionados`);
                }
            }
        });
    }
}

function LevantarReporte() {
    $('#detalle').val('');
    $('#ModalComputadora').modal('hide');
    $('#ModalReporte').modal('show');
}

function AgregarReporte() {
    if (confirm('¿Está seguro de que desea agregar este reporte? (La computadora será inhabilitada para su uso en sesiones)')) {
        $.ajax({
            url: '/Computadoras/AgregarReporte',
            data: { sala_id: $('#div_sala').html(), numero: $('#div_numero').html(), detalle: $('#detalle').val() },
            type: "POST",
            success: function (response) {
                if (response['success'] == true) {
                    alert('El reporte ha sido generado con éxito, la computadora se in-habilitará y no podrá ser usada en sesiones hasta que el reporte sea marcado como solucionado')
                    location.reload();
                }
                else
                    alert(response['responseText']);
            }
        });
    }
}

function ActualizarComputadoras() {
    var sala_id = $('#sala').val() == undefined ? 1 : $('#sala').val();

    $.ajax({
        url: '/Computadoras/ActualizarComputadoras',
        data: { sala_id: sala_id },
        type: "GET",
        success: function (response) {
            for (com of response) {

                $(`#div_${com['numero']}`).removeClass();

                if (com['disponible'] == true) {
                    $(`#div_${com['numero']}`).addClass('div disponible flex');
                    $(`#div_${com['numero']}`).html(`Disponible`)

                }
                else {
                    $(`#div_${com['numero']}`).addClass('div no-disponible flex');
                    $(`#div_${com['numero']}`).html(`No disponible`)

                }
            }
        }
    });
}

function ComputadoraDetalle(numero) {

    $.ajax({
        url: '/Computadoras/ComputadoraDetalle',
        data: { sala_id: $('#sala').val(), numero: numero },
        type: "GET",
        success: function (response) {
            $('#div_sala').html($('#sala').val());
            $('#div_numero').html(numero);
            $('#div').removeClass();
            
            if (response.length == 0) {
                $('#div').html(`<i class="bi bi-check-circle" style="margin-right:5px; font-size:14px;"></i>Disponible`);
                $('#div').addClass('div disponible flex');

                $('#tabla').hide();
                $('#div_sin_reportes').show();
            }
            else {
                $('#div').addClass('div disponible flex');
                $('#div').html(`<i class="bi bi-check-circle" style="margin-right:5px; font-size:14px;"></i>Disponible`);

                for (reporte of response)
                    if (reporte['fecha_solucion'] == '') {
                        $('#div').removeClass('disponible');
                        $('#div').addClass('no-disponible');
                        $('#div').html(`<i class="bi bi-x-circle" style="margin-right:5px; font-size:14px;"></i>No disponible`)
                    }


                $('#tabla').show();
                $('#div_sin_reportes').hide();

                ActualizarTablaDetalle(response);
            }
        }
    });

    $('#ModalComputadora').modal('show');
}

function ActualizarTablaDetalle(response) {
    $('#body').html('');

    for (reporte of response) {
        var template = '<tr>';

        template += `<td>${reporte['fecha_creacion']}</td>`;
        template += `<td>${reporte['detalle']}</td>`;

        if (reporte['fecha_solucion'] != '') {
            template += `<td><div class="div disponible" style="height: max-content;">
            <i class="bi bi-check-circle" style="margin-right:10px; font-size:14px;"></i>Solucionado el ${reporte['fecha_solucion']}</div></td>
            <td></td>`;
        }
        else {
            template += `<td><div class="div no-disponible flex" style="height: 38px;">Pendiente</div></td>
            <td><button id="${reporte['id']}" class="btn btn-outline-primary flex" style="margin-left: auto" onclick="SolucionarReporte(this.id)">
            <i class="bi bi-check-circle" style="margin-right:5px; font-size:14px;"></i>Marcar solucionado</button></td>`;
        }

        $('#body').append(template + '</tr>');
    }
}