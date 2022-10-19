function AgregarReservacion() {
    if ($('#rad_unica').prop('checked') == true) {
        $.ajax({
            url: '/_FormReservacion/AgregarReservacionUnica',
            data: {
                matricula: $('#matricula').val(), nombre: $('#nombre').val(), apellidos: $('#apellidos').val(), correo: $('#correo').val(), carrera: $('#carrera option:selected').text(),
                curso: $('#curso').val(), cantidad: $('#cantidad').val(), sala_id: $('#sala').val(),
                fecha: $('#desde_value').html(), hi: $('#hora_inicio').val(), hf: $('#hora_fin').val()
            },
            type: "POST",
            success: function (response) {
                if (response["success"] == true) {
                    alert('valida');
                }
                else {
                    alert(response['responseText']);
                }
            }
        });
    }
    else {
        var dias = [];
        var DayOfWeek = 1;
        for (dia of abr) {
            if ($(`#cb_${dia}`).prop('checked') == true) {
                dias.push(`${DayOfWeek}-${$(`#hi_${dia}`).val()}-${$(`#hf_${dia}`).val()}`);
            }
            DayOfWeek++;
        }

        $.ajax({
            url: '/_FormReservacion/AgregarReservacionFrecuencial',
            data: {
                matricula: $('#matricula').val(), nombre: $('#nombre').val(), apellidos: $('#apellidos').val(), correo: $('#correo').val(), carrera: $('#carrera option:selected').text(),
                curso: $('#curso').val(), cantidad: $('#cantidad').val(), sala_id: $('#sala').val(),
                periodo_inicio: $('#desde_value').html(), periodo_fin: $('#hasta_value').html(), dias
            },
            type: "POST",
            success: function (response) {
                if (response["success"] == true) {
                    alert('valida');
                }
                else {
                    alert(response['responseText']);
                }
            }
        });
    }
}

function _FormReservacion_LimpiarForm() {
    $('#curso').val('');
    $('#cantidad').val('');
    $('#sala').prop('selectedIndex', 0);
    $('#rad_unica').prop('checked', true);
    RadUnica_Checked();

    $('#desde_value').html('Seleccionar fecha');
    $('#hasta_value').html('Seleccionar fecha');
    ActualizarTabla('desde');
    ActualizarTabla('hasta');
    $(`#hora_inicio`).prop('selectedIndex', 0);
    $(`#hora_fin`).prop('selectedIndex', 0);

    $('#btn_evento').hide();
    $('#btn_frecuencia').hide();
    $('#btn_agregar').show();

    for (dia of abr) {
        $(`#cb_${dia}`).prop('checked', false);
        $(`#hi_${dia}`).prop('selectedIndex', 0);
        $(`#hf_${dia}`).prop('selectedIndex', 0);
        $(`#hi_${dia}`).val('');
        $(`#hf_${dia}`).val('');
    }

    $('#titulo').html(`Crear una reservación`);
}

function Cb_Checked(id) {
    var dia = id.substring(3);
    var disabled = !($(`#${id}`).prop('checked'));

    $(`#hi_${dia}`).prop('disabled', disabled);
    $(`#hf_${dia}`).prop('disabled', disabled);

    if (disabled) {
        $(`#hi_${dia}`).val('');
        $(`#hf_${dia}`).val('');
    }
    else {
        $(`#hi_${dia}`).val('7')
        $(`#hf_${dia}`).val('8');
    }
}

function RadFrecuencial_Checked() {
    $('#div_horas').hide();
    $('#div_hasta').show();
    $('#div_dias').show();
}

function RadUnica_Checked(){
    $('#div_hasta').hide();
    $('#div_dias').hide();
    $('#div_horas').show();
}

function _FormReservacion_ComponentesDisabled(bool) {
    //Primero se desactivan todos los componentes para que no se pueda mover nada
    $('#div_tipo_usuario').hide();
    $('#matricula').prop('disabled', bool);
    $('#buscar').prop('disabled', bool);
    $('#nombre').prop('disabled', bool);
    $('#apellidos').prop('disabled', bool);
    $('#carrera').prop('disabled', bool);
    $('#correo').prop('disabled', bool);
    $('#curso').prop('disabled', bool);
    $('#sala').prop('disabled', bool);
    $('#cantidad').prop('disabled', bool);
    $('#programa').prop('disabled', bool);
    $('#rad_unica').prop('disabled', bool);
    $('#rad_frecuencial').prop('disabled', bool);
    $('#desde_btn').prop('disabled', bool);
    $('#hasta_btn').prop('disabled', bool);
    $('#hora_inicio').prop('disabled', bool);
    $('#hora_fin').prop('disabled', bool);

    for (dia of abr) {
        $(`#cb_${dia}`).prop('disabled', bool);
    }
}

function ActualizarProgramasLimpio() {
    ActualizarProgramas();
    $('#programa').prop('selectedIndex', 0);
}


function ActualizarProgramas() {
    var sala_id = $('#sala').prop('selectedIndex') == undefined ? 1 : $('#sala').val();

    //Se vacian los programas
    var programa_nombre = $('#programa').val();
    $('#programa').empty();
    //Se obtienen los programas disponibles
    $.ajax({
        url: '/_FormReservacion/GetProgramas',
        data: { sala: sala_id },
        type: "GET",
        success: function (response) {
            for (var programa of response) {
                if (programa == programa_nombre) {
                    $('#programa').append($('<option>', {
                        value: programa,
                        text: programa,
                        selected: true
                    }))
                }
                else {
                    $('#programa').append($('<option>', {
                        value: programa,
                        text: programa
                    }))
                }
            }
        }
    });
}

function _FormReservacion_CargarDatos(response) {
    _FormReservacion_ComponentesDisabled(true);
    _FormUsuario_BuscarUsuario(response['matricula']);

    $('#btn_evento').show();
    $('#btn_frecuenia').show();

    $('#matricula').val(response['matricula']);
    $('#curso').val(response['curso']);
    $('#sala').prop('selectedIndex', response['sala_id'] - 1);
    $('#cantidad').val(response['cantidad_alumnos']);
    $('#btn_agregar').hide();

    $('#programa').empty();
    $('#programa').append($('<option>', {
        text: response["programa"],
        selected: true
    }))

    for (dia of abr) {
        $(`#hi_${dia}`).val('');
        $(`#hf_${dia}`).val('');
    }

    var dt = new Date(response['fecha'].split('/')[2] + '-' +
        parseInt(getMes(response['fecha'].split('/')[1])).toLocaleString('en-US', { minimumIntegerDigits: 2 }) + '-' +
        parseInt(response['fecha'].split('/')[0]).toLocaleString('en-US', { minimumIntegerDigits: 2 }) + 'T' +
        parseInt(response['hi']).toLocaleString('en-US', { minimumIntegerDigits: 2 }) + ':00:00');


    if (dt.getTime() <= new Date().getTime())
        $('#btn_evento').hide();

    if (response['es_unica'] == true) {
        $('#titulo').html(`Detalles de la reservación "${response['curso']}"`);

        $('#rad_unica').prop('checked', true);
        RadUnica_Checked();

        $('#desde_value').html(response['fecha']);

        $(`#hora_inicio`).val(response['hi']);
        $(`#hora_fin`).val(response['hf']);

        $('#btn_frecuencia').hide();

    }
    else {
        $('#titulo').html(`Detalles de la reservación "${response["curso"]}" - ${dias[dt.getDay()]} ${response['fecha']} - \n 
        (${response['totales']} totales, ${response['restantes']} futuras)`);

        $('#rad_frecuencial').prop('checked', true);
        RadFrecuencial_Checked();

        $('#desde_value').html(response['periodo_inicio']);
        $('#hasta_value').html(response['periodo_fin']);

        if (parseInt(response['restantes']) == 0)
            $('#btn_frecuenia').hide();

        for (var dia of response["dias"]) {
            $(`#cb_${dia['id']}`).prop('checked', true);
            $(`#hi_${dia['id']}`).val(dia['hi']);
            $(`#hf_${dia['id']}`).val(dia['hf']);
        }
    }

    for (dia of abr) {
        console.log();
        $(`#hi_${dia}`).prop('disabled', true);
        $(`#hf_${dia}`).prop('disabled', true);
    }

    $('#ModalReservacion').modal('show');
}