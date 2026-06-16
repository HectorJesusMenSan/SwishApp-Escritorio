using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SwishApp.Configuracion;
using SwishApp.Modelo;

namespace SwishApp.Modelo.Dao
{
    public class TorneoDao
    {
        // =====================================================
        // INSERTAR
        // =====================================================
        public void Insertar(Torneo t)
        {
            string sql =
                "INSERT INTO torneo(nombre, tipo, estado, fecha_inicio, id_usuario) " +
                "VALUES(@nombre, @tipo, @estado, @fecha, @idUsuario)";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@nombre", t.Nombre);
                    ps.Parameters.AddWithValue("@tipo", t.Tipo);
                    ps.Parameters.AddWithValue("@estado", t.Estado);
                    ps.Parameters.AddWithValue("@fecha", t.FechaInicio);
                    ps.Parameters.AddWithValue("@idUsuario",
                        App.UsuarioActivo != null ? App.UsuarioActivo.Id : 0
                    );

                    ps.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // LISTAR
        // =====================================================
        public List<Torneo> Listar()
        {
            var lista = new List<Torneo>();

            string sql =
                "SELECT * FROM torneo " +
                "WHERE id_usuario = @idUsuario " +
                "ORDER BY id DESC";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue(
                        "@idUsuario",
                        App.UsuarioActivo != null ? App.UsuarioActivo.Id : 0
                    );

                    using (var rs = ps.ExecuteReader())
                    {
                        while (rs.Read())
                        {
                            lista.Add(LeerTorneo(rs));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lista;
        }

        // =====================================================
        // BUSCAR POR ID
        // =====================================================
        public Torneo BuscarPorId(int id)
        {
            var t = new Torneo();

            string sql = "SELECT * FROM torneo WHERE id = @id";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@id", id);

                    using (var rs = ps.ExecuteReader())
                    {
                        if (rs.Read())
                        {
                            t = LeerTorneo(rs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return t;
        }

        // =====================================================
        // OBTENER ÚLTIMO
        // =====================================================
        public Torneo ObtenerUltimo()
        {
            var t = new Torneo();

            string sql =
                "SELECT * FROM torneo ORDER BY id DESC LIMIT 1";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                using (var rs = ps.ExecuteReader())
                {
                    if (rs.Read())
                    {
                        t = LeerTorneo(rs);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return t;
        }

        // =====================================================
        // FINALIZAR
        // =====================================================
        public void Finalizar(int idTorneo)
        {
            string sql =
                "UPDATE torneo SET estado = 'FINALIZADO' " +
                "WHERE id = @id";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@id", idTorneo);
                    ps.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // ELIMINAR CON TODOS SUS DATOS
        // =====================================================
        public void Eliminar(int idTorneo)
        {
            try
            {
                using (var con = Conexion.GetConexion())
                {
                    // Eliminar estadísticas
                    string sqlEst =
                        "DELETE FROM estadisticas_por_partido " +
                        "WHERE id_partido IN (" +
                        "  SELECT id FROM partido WHERE id_torneo = @id)";

                    using (var ps = new MySqlCommand(sqlEst, con))
                    {
                        ps.Parameters.AddWithValue("@id", idTorneo);
                        ps.ExecuteNonQuery();
                    }

                    // Eliminar partidos
                    string sqlPartido =
                        "DELETE FROM partido WHERE id_torneo = @id";

                    using (var ps = new MySqlCommand(sqlPartido, con))
                    {
                        ps.Parameters.AddWithValue("@id", idTorneo);
                        ps.ExecuteNonQuery();
                    }

                    // Eliminar jugadores
                    string sqlJugador =
                        "DELETE FROM jugador WHERE id_equipo IN (" +
                        "  SELECT id FROM equipo WHERE id_torneo = @id)";

                    using (var ps = new MySqlCommand(sqlJugador, con))
                    {
                        ps.Parameters.AddWithValue("@id", idTorneo);
                        ps.ExecuteNonQuery();
                    }

                    // Eliminar equipos
                    string sqlEquipo =
                        "DELETE FROM equipo WHERE id_torneo = @id";

                    using (var ps = new MySqlCommand(sqlEquipo, con))
                    {
                        ps.Parameters.AddWithValue("@id", idTorneo);
                        ps.ExecuteNonQuery();
                    }

                    // Eliminar torneo
                    string sqlTorneo =
                        "DELETE FROM torneo WHERE id = @id";

                    using (var ps = new MySqlCommand(sqlTorneo, con))
                    {
                        ps.Parameters.AddWithValue("@id", idTorneo);
                        ps.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // LEER TORNEO DESDE READER
        // =====================================================
        private Torneo LeerTorneo(MySqlDataReader rs)
        {
            return new Torneo
            {
                Id = rs.GetInt32("id"),
                Nombre = rs.IsDBNull(rs.GetOrdinal("nombre"))
                              ? "" : rs.GetString("nombre"),
                Tipo = rs.IsDBNull(rs.GetOrdinal("tipo"))
                              ? "" : rs.GetString("tipo"),
                Estado = rs.IsDBNull(rs.GetOrdinal("estado"))
                              ? "" : rs.GetString("estado"),
                FechaInicio = rs.IsDBNull(rs.GetOrdinal("fecha_inicio"))
                              ? "" : rs.GetDateTime("fecha_inicio")
                                       .ToString("yyyy-MM-dd")
            };
        }
    }
}