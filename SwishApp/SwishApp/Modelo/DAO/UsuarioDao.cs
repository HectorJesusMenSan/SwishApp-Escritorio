using System;
using MySql.Data.MySqlClient;
using SwishApp.Configuracion;
using SwishApp.Modelo;

namespace SwishApp.Modelo.Dao
{
    public class UsuarioDao
    {
        // =====================================================
        // REGISTRAR
        // =====================================================
        public bool Registrar(Usuario u)
        {
            // Verificar si ya existe el username
            if (ExisteUsername(u.Username))
                return false;

            string sql =
                "INSERT INTO usuario(nombre, username, password) " +
                "VALUES(@nombre, @username, @password)";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    // Hashear contraseña antes de guardar
                    string hashPassword =
                        Contrasenia.HashPassword(u.Password);

                    ps.Parameters.AddWithValue("@nombre", u.Nombre);
                    ps.Parameters.AddWithValue("@username", u.Username);
                    ps.Parameters.AddWithValue("@password", hashPassword);

                    ps.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // =====================================================
        // LOGIN
        // =====================================================
        public Usuario Login(string username, string password)
        {
            string sql =
                "SELECT * FROM usuario WHERE username = @username";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@username", username);

                    using (var rs = ps.ExecuteReader())
                    {
                        if (rs.Read())
                        {
                            string hashAlmacenado =
                                rs.GetString("password");

                            // Verificar contraseña
                            bool valida = Contrasenia.VerificarPassword(
                                password, hashAlmacenado
                            );

                            if (valida)
                            {
                                return new Usuario
                                {
                                    Id = rs.GetInt32("id"),
                                    Nombre = rs.GetString("nombre"),
                                    Username = rs.GetString("username")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        // =====================================================
        // EXISTE USERNAME
        // =====================================================
        public bool ExisteUsername(string username)
        {
            string sql =
                "SELECT COUNT(*) FROM usuario " +
                "WHERE username = @username";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@username", username);

                    int count = Convert.ToInt32(ps.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}