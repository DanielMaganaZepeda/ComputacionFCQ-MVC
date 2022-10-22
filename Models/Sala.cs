using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Sala
    {
        public Sala()
        {
            Computadoras = new HashSet<Computadora>();
            Reservacions = new HashSet<Reservacion>();
            Programas = new HashSet<Programa>();
        }

        public int Id { get; set; }

        public virtual ICollection<Computadora> Computadoras { get; set; }
        public virtual ICollection<Reservacion> Reservacions { get; set; }

        public virtual ICollection<Programa> Programas { get; set; }


        public static List<int> GetSalasDisponibles()
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    return db.Salas.Where(x =>
                        !(db.Reservacions.Where(y => y.FechaInicio.Value <= DateTime.Now && y.FechaFin.Value > DateTime.Now).Select(y => y.SalaId).Distinct().Contains(x.Id))
                        ).Select(x => x.Id).ToList();
                }
            }
            catch { return new List<int>(); }
        }

        public static List<int> GetComputadorasPorSala(int sala)
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    //Se encuentran las computadoras que esten en la sala y que no esten en una sesion activa
                    return db.Computadoras.Where(x => x.SalaId == sala && x.Funcional == true &&
                    !(db.Sesions.Where(y => y.FechaFin.Value == null).Select(y => y.ComputadoraId).ToList().Contains(x.Id))).Select(x => x.Numero).ToList();
                }
            }
            catch { return new List<int>(); }
        }

        public static List<string> GetProgramasPorSala(int sala)
        {
            try
            {
                if (sala != 0)
                {
                    using (var db = new ComputacionFCQContext())
                    {
                        return db.Salas.Find(sala).Programas.Select(x => x.Nombre).ToList();
                    }
                }
                else
                {
                    return new List<string>();
                }
            }
            catch { return new List<string>(); }
        }
    }
}
