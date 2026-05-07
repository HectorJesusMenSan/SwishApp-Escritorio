using System;
using System.Collections.Generic;
using System.Text;

namespace SwishApp.Modelo
{
    internal class EstadisticasPorPartido
    {
        public int Id { get; set; }
        public int IdPartido { get; set; }  
        public int IdJugador { get; set; }
        public int Puntos { get; set; }
        public int Faltas { get; set; }

        
    }
}
