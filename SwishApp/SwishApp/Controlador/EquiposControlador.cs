using System;
using System.Collections.Generic;
using System.Text;
using SwishApp.Dao;
using SwishApp.Modelo;

namespace SwishApp.Controlador
{
    internal class EquiposControlador
    {
        RegistroDao dao = new RegistroDao();
        public void RegistrarEquipo(string nombre, string categoria, string origen, int idTorneo)
        {
            // Validar los datos del equipo
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(categoria) || string.IsNullOrEmpty(origen))
            {
                throw new ArgumentException("Todos los campos son obligatorios.");
            }
            // Crear una instancia del equipo
            Equipo equipo = new Equipo()
            {
                Nombre = nombre,
                Categoria = categoria,
                Origen = origen,
                IdTorneo = idTorneo
            };
            // Registrar el equipo en la base de datos
            dao.RegistrarDatosDeEquipo(equipo);
        }
    }
}
