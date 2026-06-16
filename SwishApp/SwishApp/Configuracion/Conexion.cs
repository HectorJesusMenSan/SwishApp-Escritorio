using System;
using MySql.Data.MySqlClient;

namespace SwishApp.Configuracion
{
    public class Conexion
    {
        // ================= DATOS DE CONEXIÓN =================
        private static readonly string Host = "localhost";
        private static readonly string Puerto = "3306";
        private static readonly string BaseDatos = "swish_app";
        private static readonly string Usuario = "root";
        private static readonly string Password = "Santiago3000#";

        private static readonly string CadenaConexion
            = $"server={Host};" +
              $"port={Puerto};" +
              $"database={BaseDatos};" +
              $"uid={Usuario};" +
              $"pwd={Password};";

        // =====================================================
        // OBTENER CONEXIÓN
        // =====================================================
        public static MySqlConnection GetConexion()
        {
            MySqlConnection con = null;

            try
            {
                con = new MySqlConnection(CadenaConexion);
                con.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error de conexión: " + ex.Message);
            }

            return con;
        }
    }
}   