using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace ComputacionFCQ_MVC.Models
{
    public class GeneracionReportes
    {
        public static ExcelPackage GenerarReporte(string tipo, string desde, string hasta)
        {
            DateTime fecha_desde;
            if(desde=="Seleccionar fecha")
                fecha_desde = new DateTime(2022, 1, 1);
            else
                fecha_desde = Fecha.ParseFecha(desde);
            
            DateTime fecha_hasta;
            if(hasta=="Seleccionar fecha")
                fecha_hasta = DateTime.Now;
            else
                fecha_hasta = Fecha.ParseFecha(desde);

            if(tipo=="Sesiones") return ReporteSesiones(fecha_desde, fecha_hasta);
            if(tipo == "Reservaciones") return ReporteReservaciones(fecha_desde, fecha_hasta);
            if (tipo == "Programas") return ReporteProgramas(fecha_desde, fecha_hasta);
            else return ReporteComputadoras(fecha_desde, fecha_hasta);
        }

        public static ExcelPackage ReporteComputadoras(DateTime desde, DateTime hasta)
        {
            try
            {
                ExcelPackage package = new ExcelPackage();
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Computadoras");
                sheet.Cells[1, 1].Value = $"Programas y su uso desde {desde.ToString("dd/MM/yyyy")} hasta {hasta.ToString("dd/MM/yyyy")}";

                using (var db = new ComputacionFCQContext())
                {
                    List<Sala> salas = db.Salas.ToList();

                    int max = 0;
                    foreach (var sala in salas)
                    {
                        sheet.Cells[2, 3 * (sala.Id - 1) + 1].Value = "Sala " + sala.Id;
                        sheet.Cells[2, 3 * (sala.Id - 1) + 2].Value = "Uso en reservaciones: " + sala.Reservacions.Where(x => x.FechaInicio >= desde && x.FechaFin <= hasta && x.FrecuenciaId != null)
                            .Select(x => x.FrecuenciaId).Distinct().Count();

                        sheet.Cells[3, 3 * (sala.Id - 1) + 1].Value = "Computadora";
                        sheet.Cells[3, 3 * (sala.Id - 1) + 2].Value = "Estado";
                        sheet.Cells[3, 3 * (sala.Id - 1) + 3].Value = "Uso en sesiones";

                        int fila = 4;
                        foreach (var computadora in sala.Computadoras)
                        {
                            sheet.Cells[fila, 3 * (sala.Id - 1) + 1].Value = computadora.Numero;
                            sheet.Cells[fila, 3 * (sala.Id - 1) + 2].Value = computadora.Funcional == true ? "Disponible" : "No disponible";
                            sheet.Cells[fila, 3 * (sala.Id - 1) + 3].Value = computadora.Sesions.Where(x => x.FechaInicio >= desde && (x.FechaFin <= hasta || x.FechaFin == null)).Count();
                            fila++;
                        }
                        if (fila > max) max = fila;

                        sheet.Cells[2, 3 * (sala.Id - 1) + 3, fila, 3 * (sala.Id - 1) + 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    for (int i = 1; i < salas.Count * 3; i++)
                        sheet.Column(i).Width = 15;

                    max--;
                    sheet.Cells[1, 1, 3, salas.Count * 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[1, 1, 3, salas.Count * 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    sheet.Cells[1, 1, max, salas.Count * 3].Style.Font.Name = "Arial";
                    sheet.Cells[1, 1, max, salas.Count * 3].Style.Font.Size = 11;
                    sheet.Cells[1, 1, max, salas.Count * 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                return package;
            }
            catch { return new ExcelPackage(); }
        }

        public static ExcelPackage ReporteProgramas(DateTime desde, DateTime hasta)
        {
            try
            {
                ExcelPackage package = new ExcelPackage();
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Programas");
                sheet.Cells[1, 1].Value = $"Programas y su uso desde {desde.ToString("dd/MM/yyyy")} hasta {hasta.ToString("dd/MM/yyyy")}";

                using (var db = new ComputacionFCQContext())
                {
                    List<Programa> programas = db.Programas.ToList();

                    int fila = 3;
                    foreach (var programa in programas)
                    {
                        sheet.Cells[fila, 1].Value = programa.Nombre;
                        sheet.Cells[fila, 2].Value = programa.Activo == true ? "Disponible" : "No disponible";
                        sheet.Cells[fila, 3].Value = programa.Sesions.Where(x => x.FechaInicio >= desde && x.FechaFin <= hasta).Count();
                        sheet.Cells[fila, 4].Value = programa.Reservacions.Where(x => x.FechaInicio >= desde && x.FechaFin <= hasta).Select(x => x.FrecuenciaId).Distinct().Count();
                        fila++;
                    }

                    sheet.Cells[1, 1].Style.Font.Italic = true;
                    sheet.Cells[1, 1].Style.Font.Bold = true;
                    sheet.Cells[2, 1].Value = "Nombre"; sheet.Column(1).Width = 30;
                    sheet.Cells[2, 2].Value = "Estado"; sheet.Column(2).Width = 20;
                    sheet.Cells[2, 3].Value = "Uso en sesiones"; sheet.Column(3).Width = 20;
                    sheet.Cells[2, 4].Value = "Uso en reservaciones"; sheet.Column(4).Width = 20;

                    sheet.Cells[1, 1, 2, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[1, 1, 2, 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    sheet.Cells[1, 1, fila, 4].Style.Font.Name = "Arial";
                    sheet.Cells[1, 1, fila, 4].Style.Font.Size = 11;
                    sheet.Cells[1, 1, fila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                return package;
            }
            catch { return new ExcelPackage(); }
        }

        public static ExcelPackage ReporteReservaciones(DateTime desde, DateTime hasta)
        {
            try
            {
                hasta = new DateTime(9999, 1, 1);
                ExcelPackage package = new ExcelPackage();
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Reservaciones");
                sheet.Cells[1, 1].Value = $"Reservaciones desde {desde.ToString("dd/MM/yyyy")} hasta {hasta.ToString("dd/MM/yyyy")}";

                using (var db = new ComputacionFCQContext())
                {
                    List<Reservacion> reservaciones = db.Reservacions.Where(x => x.FechaInicio >= desde && x.FechaInicio <= hasta).OrderBy(x => x.FechaInicio).ToList();

                    if (reservaciones.Count == 0)
                    {
                        sheet.Cells[1, 1].Value = $"Sin registros de reservaciones desde {desde.ToString("dd / MM / yyyy")} hasta {hasta.ToString("dd / MM / yyyy")}";
                        return package;
                    }

                    int fila = 3;
                    foreach (Reservacion rsv in reservaciones)
                    {
                        sheet.Cells[fila, 1].Value = Convert.ToInt32(rsv.Usuario.Matricula);
                        sheet.Cells[fila, 2].Value = rsv.Usuario.Nombre;
                        sheet.Cells[fila, 3].Value = rsv.Usuario.Apellidos;
                        sheet.Cells[fila, 4].Value = rsv.Usuario.Carrera.Nombre;
                        sheet.Cells[fila, 5].Value = rsv.Usuario.Correo;
                        sheet.Cells[fila, 6].Value = rsv.SalaId;
                        sheet.Cells[fila, 7].Value = rsv.FechaInicio.Value.ToString("dd/MM/yyyy");
                        sheet.Cells[fila, 8].Value = rsv.FechaInicio.Value.ToString("HH:mm");
                        sheet.Cells[fila, 9].Value = rsv.FechaFin.Value.ToString("HH:mm");
                        sheet.Cells[fila, 11].Value = rsv.Programa.Nombre;
                        sheet.Cells[fila, 12].Value = rsv.CantidadAlumnos;

                        if (rsv.FrecuenciaId == null)
                        {
                            sheet.Cells[fila, 10].Value = rsv.Curso;
                            sheet.Cells[fila, 13].Value = "Unica";
                            sheet.Cells[fila, 14, fila, 16].Value = "-";

                            if (rsv.Activa == false)
                                sheet.Cells[fila, 17].Value = "Cancelada";
                            else
                            {
                                if (rsv.FechaFin.Value <= DateTime.Now)
                                    sheet.Cells[fila, 17].Value = "Finalizada";
                                else
                                    sheet.Cells[fila, 17].Value = "Activa";
                            }
                        }
                        else
                        {
                            sheet.Cells[fila, 10].Value = rsv.Frecuencia.Curso;
                            sheet.Cells[fila, 13].Value = "Frecuencial";
                            sheet.Cells[fila, 14].Value = rsv.FrecuenciaId;
                            sheet.Cells[fila, 15].Value = rsv.Frecuencia.PeriodoInicio.Value.ToString("dd/MM/yyyy");
                            sheet.Cells[fila, 16].Value = rsv.Frecuencia.PeriodoFin.Value.ToString("dd/MM/yyyy");

                            if (db.Reservacions.Where(x => x.FrecuenciaId == rsv.FrecuenciaId && x.Activa == true).Count() == 0)
                                sheet.Cells[fila, 17].Value = "Cancelada";
                            else
                            {
                                if (rsv.FechaFin.Value <= DateTime.Now)
                                    sheet.Cells[fila, 17].Value = "Finalizada";
                                else
                                    sheet.Cells[fila, 17].Value = "Activa";
                            }
                        }

                        fila++;
                    }

                    sheet.Cells[1, 1].Style.Font.Italic = true;
                    sheet.Cells[1, 1].Style.Font.Bold = true;
                    sheet.Cells[2, 1].Value = "Matricula"; sheet.Column(1).Width = 10;
                    sheet.Cells[2, 2].Value = "Nombre"; sheet.Column(2).Width = 20;
                    sheet.Cells[2, 3].Value = "Apellidos"; sheet.Column(3).Width = 20;
                    sheet.Cells[2, 4].Value = "Carrera"; sheet.Column(4).Width = 20;
                    sheet.Cells[2, 5].Value = "Correo universitario"; sheet.Column(5).Width = 35;
                    sheet.Cells[2, 6].Value = "Sala"; sheet.Column(6).Width = 5;
                    sheet.Cells[2, 7].Value = "Fecha"; sheet.Column(7).Width = 15;
                    sheet.Cells[2, 8].Value = "Hora inicio"; sheet.Column(8).Width = 10;
                    sheet.Cells[2, 9].Value = "Hora fin"; sheet.Column(9).Width = 10;
                    sheet.Cells[2, 10].Value = "Curso"; sheet.Column(10).Width = 30;
                    sheet.Cells[2, 11].Value = "Programa"; sheet.Column(11).Width = 30;
                    sheet.Cells[2, 12].Value = "Cantidad alumnos"; sheet.Column(12).Width = 5;
                    sheet.Cells[2, 13].Value = "Tipo"; sheet.Column(13).Width = 15;
                    sheet.Cells[2, 14].Value = "Frecuencia"; sheet.Column(14).Width = 5;
                    sheet.Cells[2, 15].Value = "Desde"; sheet.Column(15).Width = 15;
                    sheet.Cells[2, 16].Value = "Hasta"; sheet.Column(16).Width = 15;
                    sheet.Cells[2, 17].Value = "Estado"; sheet.Column(17).Width = 10;

                    sheet.Cells[1, 1, 2, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[1, 1, 2, 17].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    sheet.Cells[1, 1, fila, 17].Style.Font.Name = "Arial";
                    sheet.Cells[1, 1, fila, 17].Style.Font.Size = 11;
                    sheet.Cells[1, 1, fila, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                return package;
            }
            catch { return new ExcelPackage(); }
        }

        public static ExcelPackage ReporteSesiones(DateTime desde, DateTime hasta)
        {
            try
            {
                ExcelPackage package = new ExcelPackage();
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Sesiones");

                sheet.Cells[1, 1].Value = $"Sesiones de uso desde {desde.ToString("dd/MM/yyyy")} hasta {hasta.ToString("dd/MM/yyyy")}";

                using (var db = new ComputacionFCQContext())
                {
                    List<Sesion> sesiones = db.Sesions.Where(x => x.FechaInicio >= desde && (x.FechaFin <= hasta || x.FechaFin == null)).ToList();

                    if (sesiones.Count == 0)
                    {
                        sheet.Cells[1, 1].Value = $"Sin registros de sesiones de uso desde {desde.ToString("dd / MM / yyyy")} hasta {hasta.ToString("dd / MM / yyyy")}";
                        return package;
                    }

                    int fila = 3;
                    foreach (var sesion in sesiones)
                    {
                        sheet.Cells[fila, 1].Value = Convert.ToInt32(sesion.Usuario.Matricula);
                        sheet.Cells[fila, 2].Value = sesion.Usuario.Nombre;
                        sheet.Cells[fila, 3].Value = sesion.Usuario.Apellidos;
                        sheet.Cells[fila, 4].Value = sesion.Usuario.Carrera.Nombre;
                        sheet.Cells[fila, 5].Value = sesion.Usuario.Correo;
                        sheet.Cells[fila, 6].Value = sesion.Usuario.EsAlumno == true ? "Alumno" : "Docente";
                        sheet.Cells[fila, 7].Value = sesion.Computadora.SalaId;
                        sheet.Cells[fila, 8].Value = sesion.Computadora.Numero;
                        sheet.Cells[fila, 9].Value = sesion.FechaInicio.Value.ToString("dd/MM/yyyy HH:mm");
                        sheet.Cells[fila, 10].Value = sesion.FechaFin != null ? sesion.FechaFin.Value.ToString("dd/MM/yyyy HH:mm") : "-";
                        sheet.Cells[fila, 11].Value = sesion.Programa.Nombre;
                        fila++;
                    }

                    sheet.Cells[1, 1].Style.Font.Italic = true;
                    sheet.Cells[1, 1].Style.Font.Bold = true;
                    sheet.Cells[2, 1].Value = "Matricula"; sheet.Column(1).Width = 10;
                    sheet.Cells[2, 2].Value = "Nombre"; sheet.Column(2).Width = 20;
                    sheet.Cells[2, 3].Value = "Apellidos"; sheet.Column(3).Width = 20;
                    sheet.Cells[2, 4].Value = "Carrera"; sheet.Column(4).Width = 20;
                    sheet.Cells[2, 5].Value = "Correo universitario"; sheet.Column(5).Width = 30;
                    sheet.Cells[2, 6].Value = "Tipo"; sheet.Column(6).Width = 10;
                    sheet.Cells[2, 7].Value = "Sala"; sheet.Column(7).Width = 5;
                    sheet.Cells[2, 8].Value = "Computadora"; sheet.Column(8).Width = 5;
                    sheet.Cells[2, 9].Value = "Fecha de inicio"; sheet.Column(9).Width = 20;
                    sheet.Cells[2, 10].Value = "Fecha de fin"; sheet.Column(10).Width = 20;
                    sheet.Cells[2, 11].Value = "Programa utilizado"; sheet.Column(11).Width = 30;

                    sheet.Cells[1, 1, 2, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[1, 1, 2, 11].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    sheet.Cells[1, 1, fila, 11].Style.Font.Name = "Arial";
                    sheet.Cells[1, 1, fila, 11].Style.Font.Size = 11;
                    sheet.Cells[1, 1, fila, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                return package;
            }
            catch { return new ExcelPackage(); }
        }
    }
}
