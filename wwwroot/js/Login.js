$('#btn_login').on('click', function () {
    console.log(path_destino)
    $.ajax({
        url: '/Login/Acceder',
        data: { usuario: $('#usuario').val(), contrasena: $('#contrasena').val()},
        type: "GET",
        success: function (response) {
            if (response["success"] == false) {
                alert('Usuario o contraseña equivocada');
            }
            else {
                location.href = `/${path_destino}/${path_destino}`;
            }
        }
    });
})