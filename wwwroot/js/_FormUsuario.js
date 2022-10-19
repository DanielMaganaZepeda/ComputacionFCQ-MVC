function _FormUsuario_BuscarUsuario(id) {
    $.ajax({
        url: '/_FormUsuario/BuscarUsuario',
        data: { matricula: id },
        type: "GET",
        success: function (response) {
            if (response["responseText"] == null) {
                $('#nombre').val(response["nombre"])
                $('#apellidos').val(response["apellidos"])
                $('#correo').val(response["correo"])
                $('#carrera').prop("selectedIndex", response["carrera_id"] - 1)

                if (response['es_alumno'] == true)
                    $('#rad_alumno').prop("checked", true);
                else
                    $('#rad_docente').prop("checked", true);
            }
            else {
                alert(response["responseText"]);
            } 
        }
    });
}

function _FormUsuario_LimpiarForm() {
    $('#matricula').val('');
    $('#nombre').val('');
    $('#apellidos').val('');
    $('#correo').val('');
    $('#carrera').prop("selectedIndex", 0);
    $('#rad_alumno').prop("checked", true);
}

//ESTA FUNCION ESTA PENDIENTE EN EL CONTROLLER Y AQUI
function _FormUsuario_ValidarDatos(){
    return true;
}