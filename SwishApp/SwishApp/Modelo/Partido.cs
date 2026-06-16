using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwishApp.Modelo
{
    public class Partido
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public int PuntosA { get; set; }
        public int PuntosB { get; set; }
        public string Fecha { get; set; }
        public int Ronda { get; set; }
        public string Bracket { get; set; }
        public int Ganador { get; set; }
        public int Perdedor { get; set; }
        public bool Bye { get; set; }
        public int IdEquipoA { get; set; }
        public int IdEquipoB { get; set; }
        public int IdTorneo { get; set; }
    }
}