using System;
using System.Collections.Generic;
using System.Text;

namespace SwishApp.Modelo
{
    internal class Partido
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public int PuntosEquipoA { get; set; }
        public int PuntosEquipoB { get; set; }
        public int IdEquipoA { get; set; }
        public int IdEquipoB { get; set; }
        public int IdTorneo { get; set; }


    }
}
