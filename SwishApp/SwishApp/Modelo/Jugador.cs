using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwishApp.Modelo
{
    public class Jugador
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Numero { get; set; }
        public string Posicion { get; set; }
        public int IdEquipo { get; set; }
    }
}