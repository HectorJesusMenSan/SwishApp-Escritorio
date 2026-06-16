using System;
using MySql.Data.MySqlClient;
using SwishApp.Configuracion;

namespace SwishApp.Modelo.Dao
{
    public class EstadisticasDao
    {
        // =====================================================
        // SUMAR PUNTOS
        // =====================================================
        public void SumarPuntos(int idPartido, int idJugador, int puntos)
        {
            try
            {
                using (var con = Conexion.GetConexion())
                {
                    // Verificar si existe
                    string sqlVerificar =
                        "SELECT COUNT(*) FROM estadisticas_por_partido " +
                        "WHERE id_partido = @p AND id_jugador = @j";

                    int count = 0;

                    using (var ps = new MySqlCommand(sqlVerificar, con))
                    {
                        ps.Parameters.AddWithValue("@p", idPartido);
                        ps.Parameters.AddWithValue("@j", idJugador);
                        count = Convert.ToInt32(ps.ExecuteScalar());
                    }

                    if (count > 0)
                    {
                        string sqlUpdate =
                            "UPDATE estadisticas_por_partido " +
                            "SET puntos = puntos + @puntos " +
                            "WHERE id_partido = @p AND id_jugador = @j";

                        using (var ps = new MySqlCommand(sqlUpdate, con))
                        {
                            ps.Parameters.AddWithValue("@puntos", puntos);
                            ps.Parameters.AddWithValue("@p", idPartido);
                            ps.Parameters.AddWithValue("@j", idJugador);
                            ps.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string sqlInsert =
                            "INSERT INTO estadisticas_por_partido" +
                            "(id_partido, id_jugador, puntos, faltas) " +
                            "VALUES(@p, @j, @puntos, 0)";

                        using (var ps = new MySqlCommand(sqlInsert, con))
                        {
                            ps.Parameters.AddWithValue("@p", idPartido);
                            ps.Parameters.AddWithValue("@j", idJugador);
                            ps.Parameters.AddWithValue("@puntos", puntos);
                            ps.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // SUMAR FALTA
        // =====================================================
        public void SumarFalta(int idPartido, int idJugador)
        {
            try
            {
                using (var con = Conexion.GetConexion())
                {
                    string sqlVerificar =
                        "SELECT COUNT(*) FROM estadisticas_por_partido " +
                        "WHERE id_partido = @p AND id_jugador = @j";

                    int count = 0;

                    using (var ps = new MySqlCommand(sqlVerificar, con))
                    {
                        ps.Parameters.AddWithValue("@p", idPartido);
                        ps.Parameters.AddWithValue("@j", idJugador);
                        count = Convert.ToInt32(ps.ExecuteScalar());
                    }

                    if (count > 0)
                    {
                        string sqlUpdate =
                            "UPDATE estadisticas_por_partido " +
                            "SET faltas = faltas + 1 " +
                            "WHERE id_partido = @p AND id_jugador = @j";

                        using (var ps = new MySqlCommand(sqlUpdate, con))
                        {
                            ps.Parameters.AddWithValue("@p", idPartido);
                            ps.Parameters.AddWithValue("@j", idJugador);
                            ps.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string sqlInsert =
                            "INSERT INTO estadisticas_por_partido" +
                            "(id_partido, id_jugador, puntos, faltas) " +
                            "VALUES(@p, @j, 0, 1)";

                        using (var ps = new MySqlCommand(sqlInsert, con))
                        {
                            ps.Parameters.AddWithValue("@p", idPartido);
                            ps.Parameters.AddWithValue("@j", idJugador);
                            ps.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // OBTENER PUNTOS JUGADOR
        // =====================================================
        public int ObtenerPuntos(int idPartido, int idJugador)
        {
            string sql =
                "SELECT puntos FROM estadisticas_por_partido " +
                "WHERE id_partido = @p AND id_jugador = @j";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@p", idPartido);
                    ps.Parameters.AddWithValue("@j", idJugador);

                    var result = ps.ExecuteScalar();
                    if (result != null)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        // =====================================================
        // OBTENER FALTAS JUGADOR
        // =====================================================
        public int ObtenerFaltas(int idPartido, int idJugador)
        {
            string sql =
                "SELECT faltas FROM estadisticas_por_partido " +
                "WHERE id_partido = @p AND id_jugador = @j";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@p", idPartido);
                    ps.Parameters.AddWithValue("@j", idJugador);

                    var result = ps.ExecuteScalar();
                    if (result != null)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        // =====================================================
        // OBTENER PUNTOS EQUIPO EN PARTIDO
        // =====================================================
        public int ObtenerPuntosEquipo(int idPartido, int idEquipo)
        {
            string sql =
                "SELECT COALESCE(SUM(e.puntos), 0) " +
                "FROM estadisticas_por_partido e " +
                "INNER JOIN jugador j ON e.id_jugador = j.id " +
                "WHERE e.id_partido = @p AND j.id_equipo = @eq";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@p", idPartido);
                    ps.Parameters.AddWithValue("@eq", idEquipo);

                    return Convert.ToInt32(ps.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        // =====================================================
        // ACTUALIZAR MARCADOR
        // =====================================================
        public void ActualizarMarcador(int idPartido)
        {
            try
            {
                using (var con = Conexion.GetConexion())
                {
                    // Obtener equipos del partido
                    string sqlPartido =
                        "SELECT id_equipo_a, id_equipo_b " +
                        "FROM partido WHERE id = @id";

                    int idEquipoA = 0;
                    int idEquipoB = 0;

                    using (var ps = new MySqlCommand(sqlPartido, con))
                    {
                        ps.Parameters.AddWithValue("@id", idPartido);

                        using (var rs = ps.ExecuteReader())
                        {
                            if (rs.Read())
                            {
                                idEquipoA = rs.GetInt32("id_equipo_a");
                                idEquipoB = rs.GetInt32("id_equipo_b");
                            }
                        }
                    }

                    int puntosA = ObtenerPuntosEquipo(idPartido, idEquipoA);
                    int puntosB = ObtenerPuntosEquipo(idPartido, idEquipoB);

                    string sqlUpdate =
                        "UPDATE partido SET puntos_a=@pA, puntos_b=@pB " +
                        "WHERE id=@id";

                    using (var ps = new MySqlCommand(sqlUpdate, con))
                    {
                        ps.Parameters.AddWithValue("@pA", puntosA);
                        ps.Parameters.AddWithValue("@pB", puntosB);
                        ps.Parameters.AddWithValue("@id", idPartido);
                        ps.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}