﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Sesion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int ComputadoraId { get; set; }
        public int ProgramaId { get; set; }

        public virtual Computadora Computadora { get; set; } = null!;
        public virtual Programa Programa { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;

        //Retorna null si se cierra la sesion, retorna mensaje si el usuario no cuenta con una sesion activa
        public static string? FinalizarSesion(string matricula)
        {
            //Se valida la matricula
            if (!Validaciones.Entero(matricula)) return "Debe introducir una matricula válida";

            using (var db = new ComputacionFCQContext())
            {
                //Se busca que haya una sesion activa con esta matricula
                if (db.Sesions.Where(x => x.FechaFin.Value == null && x.UsuarioId == db.Usuarios.Where(y => y.Matricula == matricula).First().Id).FirstOrDefault() != null)
                {
                    db.Sesions.Where(x => x.FechaFin.Value == null && x.UsuarioId == db.Usuarios.Where(y => y.Matricula == matricula).First().Id).First().FechaFin = DateTime.Now;
                    db.SaveChanges();
                    return null;
                }
                //Si no tiene una sesion activa
                else
                {
                    return "El usuario no se encuentra en una sesion activa";
                }
            }
        }

        public static void IniciarSesion(string matricula, int sala, int computadora, string programa)
        {
            using (var db = new ComputacionFCQContext())
            {
                int usuario_id = db.Usuarios.Where(x => x.Matricula == matricula).First().Id;
                int computadora_id = db.Computadoras.Where(x => x.SalaId == sala && x.Numero == computadora).First().Id;
                int programa_id = db.Programas.Where(x => x.Nombre == programa).First().Id;

                db.Database.ExecuteSqlRaw("exec SP_IniciarSesion @p0, @p1, @p2", 
                    parameters: new[] { usuario_id.ToString(), computadora_id.ToString(), programa_id.ToString() });
            }
        }

        public static List<DatosSesion> GetSesiones()
        {
            List<DatosSesion> lista = new List<DatosSesion>();
            using (var db = new ComputacionFCQContext())
            {
                List<Sesion> sesiones = db.Sesions.Where(x => x.FechaFin.Value == null).ToList();

                foreach (Sesion sesion in sesiones)
                {
                    lista.Add(new DatosSesion
                        (db.Usuarios.Find(sesion.UsuarioId).Matricula,
                        db.Computadoras.Find(sesion.ComputadoraId).SalaId,
                        db.Computadoras.Find(sesion.ComputadoraId).Numero,
                        sesion.FechaInicio.Value));
                }
            }
            return lista;
        }
    }

    public class DatosSesion
    {
        public DatosSesion(string matricula, int sala, int computadora, DateTime fecha_inicio)
        {
            this.matricula = matricula;
            this.sala = sala;
            this.computadora = computadora;
            this.fecha_inicio = fecha_inicio;
        }

        public string matricula { set; get; }
        public int sala { get; set; }
        public int computadora { get; set; }
        public DateTime fecha_inicio { get; set; }
    }
}