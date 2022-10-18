$('#FormReservacion').load('/_FormReservacion/_FormReservacionPartial');

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