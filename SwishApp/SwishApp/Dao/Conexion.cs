using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace SwishApp.Dao
{
    internal class Conexion
    {
        private string cadena = "server=localhost;database=swish_app;uid=root;pwd=Santiago3000#;";

        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(cadena);
        }
    }
}
