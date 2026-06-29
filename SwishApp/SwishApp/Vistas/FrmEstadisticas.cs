// =====================================================
// =====================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SwishApp.Configuracion;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;

namespace SwishApp.Vistas
{
    // ─── Modelo interno ───────────────────────────────
    internal class ClasificacionEquipo
    {
        public Equipo  Equipo        { get; set; }
        public int     PJ            { get; set; }
        public int     Victorias     { get; set; }
        public int     Derrotas      { get; set; }
        public int     PuntosFavor   { get; set; }
        public int     PuntosContra  { get; set; }
        public int     Diferencia    { get; set; }
        public int     Faltas        { get; set; }
    }

    public class FrmEstadisticas : Form
    {
        // ── DAOs ──────────────────────────────────────
        private readonly EquipoDao   equipoDao   = new EquipoDao();
        private readonly PartidoDao  partidoDao  = new PartidoDao();
        private readonly JugadorDao  jugadorDao  = new JugadorDao();

        private readonly int   idTorneo;
        private readonly Form  frmAnterior;   // para el botón ←

        public FrmEstadisticas(int idTorneo, Form frmAnterior)
        {
            this.idTorneo    = idTorneo;
            this.frmAnterior = frmAnterior;
            ConfigurarFormulario();
            CargarEstadisticas();
        }

        // =====================================================
        // CONFIGURAR FORMULARIO
        // =====================================================
        private void ConfigurarFormulario()
        {
            this.BackColor       = Color.FromArgb(18, 18, 18);
            this.ForeColor       = Color.White;
            this.Text            = "Estadísticas del Torneo";
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.Size            = new Size(620, 780);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.AutoScroll      = true;
        }

        // =====================================================
        // CARGAR ESTADÍSTICAS
        // =====================================================
        private void CargarEstadisticas()
        {
            this.Controls.Clear();

            var clasificacion = CalcularClasificacion();

            int yActual = 18;

            // ─── HEADER ───────────────────────────────────
            var panelHeader = new Panel
            {
                Location  = new Point(0, 0),
                Size      = new Size(620, 52),
                BackColor = Color.FromArgb(28, 28, 28)
            };

            var btnAtras = new Button
            {
                Text      = "←",
                Location  = new Point(10, 10),
                Size      = new Size(42, 32),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Arial", 13, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnAtras.FlatAppearance.BorderSize = 0;
            btnAtras.Click += (s, e) =>
            {
                frmAnterior?.Show();
                this.Close();
            };

            var lblTitulo = new Label
            {
                Text      = "Clasificación General",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font      = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location  = new Point(60, 10),
                Size      = new Size(440, 32),
                AutoSize  = false
            };

            var btnRanking = new Button
            {
                Text      = "🏅 Ranking",
                Location  = new Point(510, 10),
                Size      = new Size(100, 32),
                BackColor = Color.FromArgb(244, 123, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Arial", 9, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnRanking.FlatAppearance.BorderSize = 0;
            btnRanking.Click += (s, e) =>
            {
                var frmRanking = new FrmRanking(idTorneo);
                frmRanking.Show();
                this.Hide();
            };

            panelHeader.Controls.Add(btnAtras);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(btnRanking);
            this.Controls.Add(panelHeader);

            yActual = 62;

            // ─── CARD LÍDER ───────────────────────────────
            if (clasificacion.Count > 0)
            {
                var lider = clasificacion[0];
                var cardLider = CrearCardLider(lider, yActual);
                this.Controls.Add(cardLider);
                yActual += cardLider.Height + 14;
            }

            // ─── TABLA CLASIFICACIÓN ──────────────────────
            var lblTabla = new Label
            {
                Text      = "Clasificación General",
                ForeColor = Color.White,
                Font      = new Font("Arial", 11, FontStyle.Bold),
                Location  = new Point(14, yActual),
                Size      = new Size(590, 24),
                AutoSize  = false
            };
            this.Controls.Add(lblTabla);
            yActual += 30;

            // Cabecera tabla
            var cabecera = CrearFilaTabla(
                "Pos", "Equipo", "PJ", "V/D", "Pts+", "Pts-", "Dif", "Faltas",
                yActual,
                Color.FromArgb(40, 40, 40),
                Color.FromArgb(244, 123, 37),
                isCabecera: true
            );
            this.Controls.Add(cabecera);
            yActual += 34;

            // Filas
            int pos = 1;
            foreach (var c in clasificacion)
            {
                bool esLider = pos == 1;
                string vd    = c.Victorias + "/" + c.Derrotas;
                string dif   = (c.Diferencia >= 0 ? "+" : "") + c.Diferencia;

                var fila = CrearFilaTabla(
                    pos.ToString(),
                    c.Equipo.Nombre,
                    c.PJ.ToString(),
                    vd,
                    c.PuntosFavor.ToString(),
                    c.PuntosContra.ToString(),
                    dif,
                    c.Faltas.ToString(),
                    yActual,
                    esLider
                        ? Color.FromArgb(50, 30, 10)
                        : Color.FromArgb(28, 28, 28),
                    esLider
                        ? Color.White
                        : Color.FromArgb(200, 200, 200),
                    isCabecera: false,
                    diferencia: c.Diferencia
                );

                this.Controls.Add(fila);
                yActual += 34;
                pos++;
            }

            yActual += 14;

            // ─── CARD CRITERIOS ───────────────────────────
            var cardCriterios = CrearCardCriterios(yActual);
            this.Controls.Add(cardCriterios);
            yActual += cardCriterios.Height + 20;

            // Ajustar altura del form al contenido
            this.ClientSize = new Size(620, Math.Max(780, yActual + 20));
        }

        // =====================================================
        // CARD LÍDER
        // =====================================================
        private Panel CrearCardLider(ClasificacionEquipo lider, int y)
        {
            var card = new Panel
            {
                Location  = new Point(14, y),
                Size      = new Size(590, 170),
                BackColor = Color.FromArgb(28, 28, 28)
            };

            // Franja naranja izquierda
            var franja = new Panel
            {
                Location  = new Point(0, 0),
                Size      = new Size(6, 170),
                BackColor = Color.FromArgb(244, 123, 37)
            };

            var lblTag = new Label
            {
                Text      = "LÍDER DEL TORNEO",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font      = new Font("Arial", 8, FontStyle.Bold),
                Location  = new Point(18, 14),
                Size      = new Size(200, 18),
                AutoSize  = false
            };

            var lblTrofeo = new Label
            {
                Text      = "🏆",
                Font      = new Font("Arial", 26),
                Location  = new Point(510, 10),
                Size      = new Size(60, 50),
                AutoSize  = false
            };

            var lblNombre = new Label
            {
                Text      = lider.Equipo.Nombre,
                ForeColor = Color.White,
                Font      = new Font("Arial", 18, FontStyle.Bold),
                Location  = new Point(18, 34),
                Size      = new Size(480, 36),
                AutoSize  = false
            };

            // Stats del líder
            string[] statTitulos = { "PJ", "V/D", "PTS+", "DIF" };
            string[] statValores =
            {
                lider.PJ.ToString(),
                lider.Victorias + "/" + lider.Derrotas,
                lider.PuntosFavor.ToString(),
                (lider.Diferencia >= 0 ? "+" : "") + lider.Diferencia
            };

            int xStat = 18;
            for (int i = 0; i < 4; i++)
            {
                bool esDif = i == 3;

                var lblVal = new Label
                {
                    Text      = statValores[i],
                    ForeColor = esDif
                        ? Color.FromArgb(244, 123, 37)
                        : Color.White,
                    Font      = new Font("Arial", 14, FontStyle.Bold),
                    Location  = new Point(xStat, 88),
                    Size      = new Size(130, 28),
                    AutoSize  = false
                };

                var lblStat = new Label
                {
                    Text      = statTitulos[i],
                    ForeColor = Color.FromArgb(170, 170, 170),
                    Font      = new Font("Arial", 9),
                    Location  = new Point(xStat, 118),
                    Size      = new Size(130, 18),
                    AutoSize  = false
                };

                card.Controls.Add(lblVal);
                card.Controls.Add(lblStat);
                xStat += 140;
            }

            card.Controls.Add(franja);
            card.Controls.Add(lblTag);
            card.Controls.Add(lblTrofeo);
            card.Controls.Add(lblNombre);

            return card;
        }

        // =====================================================
        // FILA TABLA
        // =====================================================
        private Panel CrearFilaTabla(
            string pos, string equipo, string pj, string vd,
            string ptsP, string ptsC, string dif, string faltas,
            int y, Color bgColor, Color fgColor,
            bool isCabecera, int diferencia = 0)
        {
            var fila = new Panel
            {
                Location  = new Point(14, y),
                Size      = new Size(590, isCabecera ? 34 : 32),
                BackColor = bgColor
            };

            // Anchos de columna: Pos, Equipo, PJ, V/D, Pts+, Pts-, Dif, Faltas
            int[] anchos = { 36, 160, 38, 60, 50, 50, 55, 55 };
            string[] valores = { pos, equipo, pj, vd, ptsP, ptsC, dif, faltas };

            int xCol = 8;
            for (int i = 0; i < valores.Length; i++)
            {
                Color colorTexto = fgColor;

                // Columna diferencia: naranja si positiva, roja si negativa
                if (!isCabecera && i == 6)
                {
                    colorTexto = diferencia >= 0
                        ? Color.FromArgb(244, 123, 37)
                        : Color.FromArgb(220, 53, 69);
                }

                var lbl = new Label
                {
                    Text      = valores[i],
                    ForeColor = colorTexto,
                    Font      = new Font("Arial",
                                    isCabecera ? 9 : 10,
                                    isCabecera ? FontStyle.Bold : FontStyle.Regular),
                    Location  = new Point(xCol, isCabecera ? 8 : 6),
                    Size      = new Size(anchos[i], 22),
                    AutoSize  = false,
                    TextAlign = i == 1
                        ? ContentAlignment.MiddleLeft
                        : ContentAlignment.MiddleCenter
                };

                fila.Controls.Add(lbl);
                xCol += anchos[i];
            }

            return fila;
        }

        // =====================================================
        // CARD CRITERIOS
        // =====================================================
        private Panel CrearCardCriterios(int y)
        {
            var card = new Panel
            {
                Location  = new Point(14, y),
                Size      = new Size(590, 160),
                BackColor = Color.FromArgb(28, 28, 28)
            };

            var lblTitulo = new Label
            {
                Text      = "Criterio de Clasificación",
                ForeColor = Color.White,
                Font      = new Font("Arial", 11, FontStyle.Bold),
                Location  = new Point(14, 14),
                Size      = new Size(560, 22),
                AutoSize  = false
            };

            var lblDesc = new Label
            {
                Text      = "La tabla se ordena por victorias. En empate se usa la diferencia de puntos.",
                ForeColor = Color.FromArgb(170, 170, 170),
                Font      = new Font("Arial", 9),
                Location  = new Point(14, 40),
                Size      = new Size(560, 32),
                AutoSize  = false
            };

            card.Controls.Add(lblTitulo);
            card.Controls.Add(lblDesc);

            // Criterios
            var criterios = new (string Texto, string Badge, Color BadgeColor)[]
            {
                ("Victorias",             "Prioridad 1", Color.FromArgb(244, 123, 37)),
                ("Diferencia de puntos",  "Prioridad 2", Color.FromArgb(50, 50, 80)),
                ("Faltas acumuladas",     "Estadística", Color.FromArgb(180, 40, 40))
            };

            int yC = 80;
            foreach (var (texto, badge, colorBadge) in criterios)
            {
                var lblTexto = new Label
                {
                    Text      = texto,
                    ForeColor = Color.White,
                    Font      = new Font("Arial", 10),
                    Location  = new Point(14, yC),
                    Size      = new Size(300, 22),
                    AutoSize  = false
                };

                var lblBadge = new Label
                {
                    Text      = badge,
                    ForeColor = Color.White,
                    BackColor = colorBadge,
                    Font      = new Font("Arial", 8, FontStyle.Bold),
                    Location  = new Point(470, yC),
                    Size      = new Size(100, 22),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize  = false
                };

                card.Controls.Add(lblTexto);
                card.Controls.Add(lblBadge);
                yC += 26;
            }

            return card;
        }

        // =====================================================
        // CALCULAR CLASIFICACIÓN
        // =====================================================
        private List<ClasificacionEquipo> CalcularClasificacion()
        {
            var equipos  = equipoDao.ListarPorTorneo(idTorneo);
            var partidos = partidoDao.ListarPorTorneo(idTorneo);
            var lista    = new List<ClasificacionEquipo>();

            foreach (var eq in equipos)
            {
                int pj = 0, victorias = 0, derrotas = 0;
                int ptsF = 0, ptsC = 0, faltas = 0;

                foreach (var p in partidos)
                {
                    if (p.Estado != "FINALIZADO") continue;

                    bool participa =
                        p.IdEquipoA == eq.Id ||
                        p.IdEquipoB == eq.Id;

                    if (!participa) continue;

                    pj++;

                    if (p.Ganador  == eq.Id) victorias++;
                    if (p.Perdedor == eq.Id) derrotas++;

                    if (p.IdEquipoA == eq.Id)
                    {
                        ptsF += p.PuntosA;
                        ptsC += p.PuntosB;
                    }
                    else
                    {
                        ptsF += p.PuntosB;
                        ptsC += p.PuntosA;
                    }
                }

                // Faltas del equipo sumando todos sus jugadores
                var jugadores = jugadorDao.ListarPorEquipo(eq.Id);
                try
                {
                    using (var con = Conexion.GetConexion())
                    {
                        string sql =
                            "SELECT COALESCE(SUM(faltas),0) total " +
                            "FROM estadisticas_por_partido " +
                            "WHERE id_jugador = @id";

                        foreach (var j in jugadores)
                        {
                            using (var ps = new MySqlCommand(sql, con))
                            {
                                ps.Parameters.AddWithValue("@id", j.Id);
                                object res = ps.ExecuteScalar();
                                if (res != null && res != DBNull.Value)
                                    faltas += Convert.ToInt32(res);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                lista.Add(new ClasificacionEquipo
                {
                    Equipo       = eq,
                    PJ           = pj,
                    Victorias    = victorias,
                    Derrotas     = derrotas,
                    PuntosFavor  = ptsF,
                    PuntosContra = ptsC,
                    Diferencia   = ptsF - ptsC,
                    Faltas       = faltas
                });
            }

            // Ordenar: victorias desc → diferencia desc
            lista.Sort((a, b) =>
            {
                int cmp = b.Victorias.CompareTo(a.Victorias);
                if (cmp != 0) return cmp;
                return b.Diferencia.CompareTo(a.Diferencia);
            });

            return lista;
        }
    }
}
