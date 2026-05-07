using System;
using System.Collections.Generic;
using System.Text;

namespace SwishApp.Modelo
{
    internal class Equipo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Origen { get; set; }
        public int IdTorneo { get; set; }

    }
}
