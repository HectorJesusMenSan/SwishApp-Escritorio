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
    public class EquipoDao
    {
        // =====================================================
        // INSERTAR
        // =====================================================
        public void Insertar(Equipo e)
        {
            string sql =
                "INSERT INTO equipo(nombre, categoria, origen, " +
                "id_torneo, derrotas, estado) " +
                "VALUES(@nombre, @categoria, @origen, " +
                "@idTorneo, 0, 'ACTIVO')";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@nombre", e.Nombre);
                    ps.Parameters.AddWithValue("@categoria", e.Categoria);
                    ps.Parameters.AddWithValue("@origen", e.Origen);
                    ps.Parameters.AddWithValue("@idTorneo", e.IdTorneo);
                    ps.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // LISTAR POR TORNEO ACTIVO
        // =====================================================
        public List<Equipo> Listar()
        {
            var lista = new List<Equipo>();

            string sql =
                "SELECT * FROM equipo " +
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
                        lista.Add(LeerEquipo(rs));
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
        public List<Equipo> ListarPorTorneo(int idTorneo)
        {
            var lista = new List<Equipo>();

            string sql =
                "SELECT * FROM equipo " +
                "WHERE id_torneo = @idTorneo " +
                "ORDER BY id";

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
                            lista.Add(LeerEquipo(rs));
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
        public Equipo BuscarPorId(int id)
        {
            var e = new Equipo();

            string sql = "SELECT * FROM equipo WHERE id = @id";

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
                            e = LeerEquipo(rs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return e;
        }

        // =====================================================
        // ACTUALIZAR
        // =====================================================
        public void Actualizar(Equipo e)
        {
            string sql =
                "UPDATE equipo SET nombre=@nombre, " +
                "categoria=@categoria, origen=@origen " +
                "WHERE id=@id";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@nombre", e.Nombre);
                    ps.Parameters.AddWithValue("@categoria", e.Categoria);
                    ps.Parameters.AddWithValue("@origen", e.Origen);
                    ps.Parameters.AddWithValue("@id", e.Id);
                    ps.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // ACTUALIZAR DERROTAS
        // =====================================================
        public void ActualizarDerrotas(Equipo e)
        {
            string sql =
                "UPDATE equipo SET derrotas=@derrotas, " +
                "estado=@estado WHERE id=@id";

            try
            {
                using (var con = Conexion.GetConexion())
                using (var ps = new MySqlCommand(sql, con))
                {
                    ps.Parameters.AddWithValue("@derrotas", e.Derrotas);
                    ps.Parameters.AddWithValue("@estado", e.Estado);
                    ps.Parameters.AddWithValue("@id", e.Id);
                    ps.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // ELIMINAR
        // =====================================================
        public void Eliminar(int id)
        {
            string sql = "DELETE FROM equipo WHERE id = @id";

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
        // LEER EQUIPO DESDE READER
        // =====================================================
        private Equipo LeerEquipo(MySqlDataReader rs)
        {
            return new Equipo
            {
                Id = rs.GetInt32("id"),
                Nombre = rs.GetString("nombre"),
                Categoria = rs.GetString("categoria"),
                Origen = rs.GetString("origen"),
                IdTorneo = rs.GetInt32("id_torneo"),
                Derrotas = rs.GetInt32("derrotas"),
                Estado = rs.IsDBNull(rs.GetOrdinal("estado"))
                            ? "ACTIVO" : rs.GetString("estado")
            };
        }
    }
}