RecargarSalas();
setInterval(RecargarSalas, 60000);

RecargarSesiones();
setInterval(RecargarSalas, 30000);

function IniciarSesion() {
    console.log(_FormUsuario_ValidarDatos());
    if (_FormUsuario_ValidarDatos()) {
        $.ajax({
            url: '/Sesiones/IniciarSesion',
            data: { matricula: $('#matricula').val(), sala: $('#sala').val(), computadora: $('#computadora').val(), programa: $('#programa').val() },
            type: "POST",
            success: function (response) {
                if (response["success"] == true) {
                    RecargarSesiones();
                    alert('La sesión ha sido iniciada con exito');
                    $('#ModalIniciar').modal('hide');
                }
                else {
                    alert(response["responseText"]);
                }
            }
        });
    }
    else {
        console.log('?');
    }
}

function FinalizarSesion(matricula) {
    $.ajax({
        url: '/Sesiones/FinalizarSesion',
        data: { matricula: matricula },
        type: "POST",
        success: function (response) {
            if (response["success"] == true) {
                RecargarSesiones();
                alert('La sesión ha sido finalizada con exito');
                $('#ModalFinalizar').modal('hide');
            }
            else {
                alert(response["responseText"]);
            }
        }
    });
}

function FinalizarSesionTabla() {
    if ($('#no').length) {
        alert('No hay sesiones activas');
        return;
    }
    $('#ModalFinalizar').modal('show');
}

function RecargarFormUsuario() {
    $('#FormUsuario').load('/_FormUsuario/_FormUsuarioPartial');
}

function RecargarSalas() {
    $('#tabla_salas').load('/_EstadoSalas/EstadoSalasPartial')
}

function RecargarSesiones() {
    $('#tabla_sesiones').load('/Sesiones/TablaSesionesPartial')
}

function IniciarSesion() {
    $.ajax({
        url: '/Sesiones/IniciarSesion',
        data: {
            matricula: $('#matricula').val(), nombre: $('#nombre').val(), apellidos: $('#apellidos').val(), correo: $('#correo').val(),
            carrera: $('#carrera option:selected').text(), es_alumno: $('#rad_alumno').is(':checked'), sala: $('#sala option:selected').text(),
            computadora: $('#computadora option:selected').text(), programa: $('#programa option:selected').text()
        },
        type: "POST",
        success: function (response) {
            if (response["success"] == true) {
                RecargarSesiones();
                alert('La sesión se ha iniciado con exito');
                $('#ModalIniciar').modal('hide');
                $('#sala').prop('selectedIndex', 0);
            }
            else {
                alert(response["responseText"]);
            }
        }
    });
}

$('#ModalIniciar').on('show.bs.modal', function (e) {
    //Se vacian las salas
    $('#sala').empty();
    //Se obtienen las salas disponibles
    $.ajax({
        url: '/Sesiones/ActualizarSalas',
        type: "GET",
        success: function (response) {
            response.forEach(sala => {
                $('#sala').append($('<option>', {
                    value: sala,
                    text: sala
                }))
            })
        }
    });
})

$('#ModalIniciar').on('shown.bs.modal', function (e) {
    $('#sala').trigger('change');
})

$('#sala').change(function () {
    //Se vacian las computadoras
    $('#computadora').empty();
    //Se obtienen las computadoras disponibles
    $.ajax({
        url: '/Sesiones/ActualizarComputadoras',
        data: { sala: $('#sala').val() },
        type: "GET",
        success: function (response) {
            response.forEach(computadora => {
                $('#computadora').append($('<option>', {
                    value: computadora,
                    text: computadora
                }))
            })
        }
    });

    //Se vacian los programas
    var programa_nombre = $('#programa').val();
    $('#programa').empty();
    //Se obtienen las salas disponibles
    $.ajax({
        url: '/Sesiones/ActualizarProgramas',
        data: { sala: $('#sala').val() },
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
})

$('#ModalFinalizar').on('hidden.bs.modal', function (e) {
    $('#tx_matricula').val("")
})

$('#ModalFinalizar').on('shown.bs.modal', function (e) {
    $('#tx_matricula').focus()
})