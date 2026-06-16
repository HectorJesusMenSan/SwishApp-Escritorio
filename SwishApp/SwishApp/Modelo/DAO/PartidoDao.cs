using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SwishApp.Configuracion;
using SwishApp.Modelo;

namespace SwishApp.Modelo.Dao
{
    public class PartidoDao
    {
        // =====================================================
        // LISTAR POR TORNEO ACTIVO
        // =====================================================
        public List<Partido> Listar()
        {
            var lista = new List<Partido>();

            string sql =
                "SELECT * FROM partido " +
                "WHERE id_torneo = (SELECT MAX(id) FROM torneo) " +
                "ORDER BY id";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                using (var rs = ps.ExecuteReader())
                {
                    while (rs.Read())
                    {
                        lista.Add(LeerPartido(rs));
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
        // LISTAR POR TORNEO ESPECÍFICO
        // =====================================================
        public List<Partido> ListarPorTorneo(int idTorneo)
        {
            var lista = new List<Partido>();

            string sql =
                "SELECT * FROM partido " +
                "WHERE id_torneo = @idTorneo ORDER BY id";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@idTorneo", idTorneo);

                    using (var rs = ps.ExecuteReader())
                    {
                        while (rs.Read())
                        {
                            lista.Add(LeerPartido(rs));
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
        public Partido BuscarPorId(int id)
        {
            var p = new Partido();

            string sql = "SELECT * FROM partido WHERE id = @id";

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
                            p = LeerPartido(rs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return p;
        }

        // =====================================================
        // INSERTAR
        // =====================================================
        public void Insertar(Partido p)
        {
            string sql =
                "INSERT INTO partido(nombre, estado, puntos_a, puntos_b, " +
                "fecha, ronda, bracket, id_equipo_a, id_equipo_b, " +
                "id_torneo, bye, ganador, perdedor) " +
                "VALUES(@nombre, @estado, @pA, @pB, @fecha, @ronda, " +
                "@bracket, @eqA, @eqB, @torneo, @bye, @ganador, @perdedor)";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@nombre", p.Nombre);
                    ps.Parameters.AddWithValue("@estado", p.Estado);
                    ps.Parameters.AddWithValue("@pA", p.PuntosA);
                    ps.Parameters.AddWithValue("@pB", p.PuntosB);
                    ps.Parameters.AddWithValue("@fecha", p.Fecha);
                    ps.Parameters.AddWithValue("@ronda", p.Ronda);
                    ps.Parameters.AddWithValue("@bracket", p.Bracket);
                    ps.Parameters.AddWithValue("@eqA", p.IdEquipoA);
                    ps.Parameters.AddWithValue("@eqB", p.IdEquipoB);
                    ps.Parameters.AddWithValue("@torneo", p.IdTorneo);
                    ps.Parameters.AddWithValue("@bye", p.Bye);
                    ps.Parameters.AddWithValue("@ganador", p.Ganador);
                    ps.Parameters.AddWithValue("@perdedor", p.Perdedor);
                    ps.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // ACTUALIZAR
        // =====================================================
        public void Actualizar(Partido p)
        {
            string sql =
                "UPDATE partido SET nombre=@nombre, estado=@estado, " +
                "puntos_a=@pA, puntos_b=@pB, fecha=@fecha, " +
                "ronda=@ronda, bracket=@bracket, ganador=@ganador, " +
                "perdedor=@perdedor, id_equipo_a=@eqA, " +
                "id_equipo_b=@eqB, id_torneo=@torneo " +
                "WHERE id=@id";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@nombre", p.Nombre);
                    ps.Parameters.AddWithValue("@estado", p.Estado);
                    ps.Parameters.AddWithValue("@pA", p.PuntosA);
                    ps.Parameters.AddWithValue("@pB", p.PuntosB);
                    ps.Parameters.AddWithValue("@fecha", p.Fecha);
                    ps.Parameters.AddWithValue("@ronda", p.Ronda);
                    ps.Parameters.AddWithValue("@bracket", p.Bracket);
                    ps.Parameters.AddWithValue("@ganador", p.Ganador);
                    ps.Parameters.AddWithValue("@perdedor", p.Perdedor);
                    ps.Parameters.AddWithValue("@eqA", p.IdEquipoA);
                    ps.Parameters.AddWithValue("@eqB", p.IdEquipoB);
                    ps.Parameters.AddWithValue("@torneo", p.IdTorneo);
                    ps.Parameters.AddWithValue("@id", p.Id);
                    ps.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // EXISTE PARTIDO PENDIENTE
        // =====================================================
        public bool ExistePartidoPendiente(
            int equipoA, int equipoB, int idTorneo)
        {
            string sql =
                "SELECT COUNT(*) FROM partido " +
                "WHERE id_torneo = @torneo " +
                "AND estado != 'FINALIZADO' " +
                "AND ((id_equipo_a = @eqA AND id_equipo_b = @eqB) " +
                "OR   (id_equipo_a = @eqB AND id_equipo_b = @eqA))";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@torneo", idTorneo);
                    ps.Parameters.AddWithValue("@eqA", equipoA);
                    ps.Parameters.AddWithValue("@eqB", equipoB);

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

        // =====================================================
        // LEER PARTIDO DESDE READER
        // =====================================================
        private Partido LeerPartido(MySqlDataReader rs)
        {
            return new Partido
            {
                Id = rs.GetInt32("id"),
                Nombre = rs.GetString("nombre"),
                Estado = rs.GetString("estado"),
                PuntosA = rs.GetInt32("puntos_a"),
                PuntosB = rs.GetInt32("puntos_b"),
                Fecha = rs.IsDBNull(rs.GetOrdinal("fecha"))
                            ? "" : rs.GetDateTime("fecha").ToString("yyyy-MM-dd"),
                Ronda = rs.GetInt32("ronda"),
                Bracket = rs.IsDBNull(rs.GetOrdinal("bracket"))
                            ? "" : rs.GetString("bracket"),
                Ganador = rs.GetInt32("ganador"),
                Perdedor = rs.GetInt32("perdedor"),
                Bye = rs.GetBoolean("bye"),
                IdEquipoA = rs.GetInt32("id_equipo_a"),
                IdEquipoB = rs.GetInt32("id_equipo_b"),
                IdTorneo = rs.GetInt32("id_torneo")
            };
        }
    }
}