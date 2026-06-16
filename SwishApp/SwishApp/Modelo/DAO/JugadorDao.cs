using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SwishApp.Configuracion;
using SwishApp.Modelo;

namespace SwishApp.Modelo.Dao
{
    public class JugadorDao
    {
        // =====================================================
        // INSERTAR
        // =====================================================
        public void Insertar(Jugador j)
        {
            string sql =
                "INSERT INTO jugador(nombre, numero, posicion, id_equipo) " +
                "VALUES(@nombre, @numero, @posicion, @idEquipo)";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@nombre", j.Nombre);
                    ps.Parameters.AddWithValue("@numero", j.Numero);
                    ps.Parameters.AddWithValue("@posicion", j.Posicion);
                    ps.Parameters.AddWithValue("@idEquipo", j.IdEquipo);
                    ps.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // LISTAR POR EQUIPO
        // =====================================================
        public List<Jugador> ListarPorEquipo(int idEquipo)
        {
            var lista = new List<Jugador>();

            string sql =
                "SELECT * FROM jugador WHERE id_equipo = @idEquipo";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@idEquipo", idEquipo);

                    using (var rs = ps.ExecuteReader())
                    {
                        while (rs.Read())
                        {
                            lista.Add(LeerJugador(rs));
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
        // LISTAR POR TORNEO
        // =====================================================
        public List<Jugador> ListarPorTorneo(int idTorneo)
        {
            var lista = new List<Jugador>();

            string sql =
                "SELECT j.* FROM jugador j " +
                "INNER JOIN equipo e ON j.id_equipo = e.id " +
                "WHERE e.id_torneo = @idTorneo ORDER BY j.id";

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
                            lista.Add(LeerJugador(rs));
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
        public Jugador BuscarPorId(int id)
        {
            Jugador j = null;

            string sql = "SELECT * FROM jugador WHERE id = @id";

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
                            j = LeerJugador(rs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return j;
        }

        // =====================================================
        // EXISTE NÚMERO EN EQUIPO
        // =====================================================
        public bool ExisteNumeroEnEquipo(int numero, int idEquipo)
        {
            string sql =
                "SELECT COUNT(*) FROM jugador " +
                "WHERE numero = @numero AND id_equipo = @idEquipo";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@numero", numero);
                    ps.Parameters.AddWithValue("@idEquipo", idEquipo);

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
        // ELIMINAR
        // =====================================================
        public void Eliminar(int id)
        {
            string sql = "DELETE FROM jugador WHERE id = @id";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@id", id);
                    ps.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // LEER JUGADOR DESDE READER
        // =====================================================
        private Jugador LeerJugador(MySqlDataReader rs)
        {
            return new Jugador
            {
                Id = rs.GetInt32("id"),
                Nombre = rs.GetString("nombre"),
                Numero = rs.GetInt32("numero"),
                Posicion = rs.GetString("posicion"),
                IdEquipo = rs.GetInt32("id_equipo")
            };
        }
    }
}