$('#nav-page').html('Programas')
$('#nav-icon').addClass('bi-grid-3x3-gap-fill')

var cambios = [];
var editable = false;

$('#div_editando').hide();

function Editar() {
   editable = true;
   cambios = [];
   $(".no-disponible").map(function () {
        $(this).removeClass("no-disponible");
   });
    $('#div_default').hide();
    $('#div_editando').show();
}

function cb_Click(id) {
    if (editable == false) {
        $(`#${id}`).prop('checked', !$(`#${id}`).prop('checked'));
        alert('Para hacer modificaciones debe habilitar la edición');
    }
    else {
        if (cambios.includes(id))
            cambios.splice(cambios.indexOf(id),1);
        else
            cambios.push(id);
    }
}

function Cancelar() {
    if (confirm('¿Está seguro de que cancelar la edición? (los cambios no serán aplicados)')) {
        cambios = [];
        $('#div_default').show();
        $('#div_editando').hide();
        $('input[type="checkbox"]').map(function () {
            $(this).addClass("no-disponible");
        });
    }
}

function AplicarCambios() {
    if (cambios.length == 0) {
        alert('No se ha seleccionado ningun cambio');
        return;
    }
    if (confirm('¿Está seguro de que desea guardar los cambios?')) {
        $.ajax({
            url: '/Programas/ActualizarProgramas',
            data: { ids: cambios },
            type: "POST",
            success: function (response) {
                alert(cambios.length + ' cambios han sido aplicados con exito');
                $('#div_editando').hide();
                $('#div_default').show();
            }
        });
    }
}

function Programa_AbrirModal() {
    $('#tx_nombre').val('');
    $('#ModalPrograma').modal('show');
}

function AgregarPrograma() {
    $.ajax({
        url: '/Programas/AgregarPrograma',
        data: { nombre: $('#tx_nombre').val() },
        type: "POST",
        success: function (response) {
            if (response['success'] == true) {
                alert('El programa ha sido agregado con éxito');
                location.reload();
            }
            else {
                alert(response['responseText']);
            }
        }
    });
}

function EliminarPrograma(id) {
    if (confirm('¿Está seguro de que desea eliminar este programa?')) {
        $.ajax({
            url: '/Programas/EliminarPrograma',
            data: { id: id },
            type: "DELETE",
            success: function (response) {
                alert('El programa ha sido eliminado con éxito');
                location.reload();
            }
        });
    }
}