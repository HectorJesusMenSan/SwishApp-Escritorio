using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using SwishApp.Modelo;

namespace SwishApp.Dao
{
    internal class RegistroDao
    {
        Conexion con = new Conexion();
        public void RegistrarDatosDeEquipo(Equipo E)
        {
            // Lógica para registrar los datos del equipo en la base de datos
            using (var conexion = con.ObtenerConexion())
            {
                conexion.Open();
                string query = "INSERT INTO equipo (nombre, categoria, origen, id_torneo) VALUES (@Nombre, @Categoria, @Origen, @IdTorneo)";
                MySqlCommand comando = new MySqlCommand(query, conexion);

                comando.Parameters.AddWithValue("@Nombre", E.Nombre);
                comando.Parameters.AddWithValue("@Categoria", E.Categoria);
                comando.Parameters.AddWithValue("@Origen", E.Origen);
                comando.Parameters.AddWithValue("@IdTorneo", E.IdTorneo);
                comando.ExecuteNonQuery();
                
            }
        }
    }
}
