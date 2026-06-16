using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwishApp.Modelo
{
    public class Equipo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Origen { get; set; }
        public int IdTorneo { get; set; }
        public int Derrotas { get; set; }
        public string Estado { get; set; }
    }
}