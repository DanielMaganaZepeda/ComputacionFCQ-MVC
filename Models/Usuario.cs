using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Reservacions = new HashSet<Reservacion>();
            Sesions = new HashSet<Sesion>();
            Carreras = Carrera.GetCarreras();
        }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public List<SelectListItem> Carreras { get; set; }

        public int Id { get; set; }
        public string Matricula { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public int CarreraId { get; set; }
        public string? Correo { get; set; }
        public bool EsAlumno { get; set; }

        public virtual Carrera Carrera { get; set; } = null!;
        public virtual ICollection<Reservacion> Reservacions { get; set; }
        public virtual ICollection<Sesion> Sesions { get; set; }
        
        //Valida que no este en una sesion activa
        public static bool EstaEnSesion(string matricula)
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    Usuario? usuario = db.Usuarios.Where(x => x.Matricula == matricula).FirstOrDefault();

                    if (usuario != null)
                    {
                        if (db.Sesions.Where(x => x.UsuarioId == usuario.Id && x.FechaFin.Value == null).FirstOrDefault() != null)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            catch { return true; }
        }

        //Valida que los parametros introducidos sean validos
        public static string? ValidarDatos(string matricula, string nombre, string apellidos, string correo)
        {
            if (!Validaciones.Entero(matricula)) return "Se debe introducir una matricula válida";
            if (!Validaciones.Nombre(nombre)) return "Se debe introducir un nombre válido";
            if (!Validaciones.Nombre(apellidos)) return "Se deben introducir apellidos válido";
            if (!Validaciones.Correo(correo)) return "Se debe introducir un correo válido";
            return null;
        }

        //Encuentra el usuario con la matricula indicada y le actualiza los campos que sean diferentes (Stored procedure)
        public static void GuardarCambios(string matricula, string nombre, string apellidos, string correo, string carrera, bool es_alumno)
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    int carrera_id = db.Carreras.Where(x => x.Nombre == carrera).First().Id;

                    db.Database.ExecuteSqlRaw("exec SP_Usuario @p0, @p1, @p2, @p3, @p4, @p5",
                        parameters: new[] { matricula, nombre, apellidos, carrera_id.ToString(), correo, Convert.ToInt32(es_alumno).ToString() });
                }
            }
            catch { }
        }

        //Recibe una matricula, retorna el usuario con esa matricula, de no existir retorna null
        public static Usuario? GetUsuarioPorMatricula(string matricula)
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    if (db.Usuarios.Where(x => x.Matricula == matricula).FirstOrDefault() != null)
                        return db.Usuarios.Where(x => x.Matricula == matricula).First();
                    else
                        return null;
                }
            }
            catch { return null; }
        }
    }
}
