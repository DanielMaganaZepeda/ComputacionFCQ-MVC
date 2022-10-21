function GenerarReporte() {
    location.href = `/Reportes/GenerarReporte?tipo=${$('#tipo').val()}&desde=${$('#desde_value').html()}&hasta=${$('#hasta_value').html()}`;
}