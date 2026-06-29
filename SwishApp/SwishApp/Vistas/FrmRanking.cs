using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SwishApp.Configuracion;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;
using SwishApp.Vistas;

namespace SwishApp.Vistas
{
    public partial class FrmRanking : Form
    {
        private JugadorDao jugadorDao = new JugadorDao();
        private EquipoDao equipoDao = new EquipoDao();
        private int idTorneo;

        public FrmRanking(int idTorneo)
        {
            this.idTorneo = idTorneo;
            InitializeComponent();
            ConfigurarFormulario();
            CargarRanking();
        }

        // =====================================================
        // CONFIGURAR FORMULARIO
        // =====================================================
        private void ConfigurarFormulario()
        {
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.ForeColor = Color.White;
            this.Text = "Ranking MVP";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(430, 700);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.AutoScroll = true;
        }

        // =====================================================
        // CARGAR RANKING
        // =====================================================
        private void CargarRanking()
        {
            this.Controls.Clear();

            // Título
            var lblTitulo = new Label
            {
                Text = "Ranking de Jugadores",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 15),
                Size = new Size(420, 35),
                AutoSize = false
            };

            this.Controls.Add(lblTitulo);

            // Calcular ranking
            var jugadores = jugadorDao.ListarPorTorneo(idTorneo);
            var ranking = new List<RankingItem>();

            try
            {
                using (var con = Conexion.GetConexion())
                {
                    foreach (var j in jugadores)
                    {
                        string sql =
                            "SELECT COALESCE(SUM(puntos),0) puntos, " +
                            "COALESCE(SUM(faltas),0) faltas, " +
                            "COUNT(*) partidos " +
                            "FROM estadisticas_por_partido " +
                            "WHERE id_jugador = @id";

                        using (var ps = new MySqlCommand(sql, con))
                        {
                            ps.Parameters.AddWithValue("@id", j.Id);

                            using (var rs = ps.ExecuteReader())
                            {
                                if (rs.Read())
                                {
                                    int pts = rs.GetInt32("puntos");
                                    int faltas = rs.GetInt32("faltas");
                                    int partidos = rs.GetInt32("partidos");

                                    double promedio = partidos > 0
                                        ? (double)pts / partidos : 0;

                                    double score = pts - (faltas * 0.5);

                                    var equipo = equipoDao.BuscarPorId(j.IdEquipo);

                                    ranking.Add(new RankingItem
                                    {
                                        Jugador = j,
                                        Equipo = equipo,
                                        Puntos = pts,
                                        Faltas = faltas,
                                        Partidos = partidos,
                                        Promedio = promedio,
                                        Score = score
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Ordenar por score
            ranking.Sort((a, b) => b.Score.CompareTo(a.Score));

            int y = 60;

            // MVP
            if (ranking.Count > 0)
            {
                var mvp = ranking[0];
                var cardMvp = CrearCardMvp(mvp);
                cardMvp.Location = new Point(15, y);
                this.Controls.Add(cardMvp);
                y += 180;
            }

            // Top 10
            var lblTop = new Label
            {
                Text = "Top 10 Rendimiento",
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(15, y),
                Size = new Size(385, 28),
                AutoSize = false
            };

            this.Controls.Add(lblTop);
            y += 35;

            int pos = 1;
            int limit = Math.Min(ranking.Count, 10);

            for (int i = 0; i < limit; i++)
            {
                var fila = CrearFilaRanking(ranking[i], pos);
                fila.Location = new Point(15, y);
                this.Controls.Add(fila);
                y += 58;
                pos++;
            }

            // Fórmula
            var lblFormula = new Label
            {
                Text = "Score = Puntos - (Faltas × 0.5)",
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 9),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, y + 10),
                Size = new Size(420, 25),
                AutoSize = false
            };

            this.Controls.Add(lblFormula);
            y += 40;

            // Botón volver
            var btnVolver = new Button
            {
                Text = "← Volver",
                Location = new Point(15, y + 10),
                Size = new Size(385, 45),
                BackColor = Color.FromArgb(42, 42, 42),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Click += (s, e) =>
            {
                var frmTorneos = new FrmTorneos();
                frmTorneos.Show();
                this.Close();
            };

            this.Controls.Add(btnVolver);
        }

        // =====================================================
        // CREAR CARD MVP
        // =====================================================
        private Panel CrearCardMvp(RankingItem mvp)
        {
            var card = new Panel
            {
                Size = new Size(385, 165),
                BackColor = Color.FromArgb(28, 28, 28)
            };

            card.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, 385, 165, 15, 15)
            );

            // Círculo número
            var lblNum = new Label
            {
                Text = mvp.Jugador.Numero.ToString(),
                ForeColor = Color.FromArgb(244, 123, 37),
                BackColor = Color.FromArgb(50, 30, 10),
                Font = new Font("Arial", 18, FontStyle.Bold),
                Location = new Point(15, 20),
                Size = new Size(65, 65),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            lblNum.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, 65, 65, 32, 32)
            );

            // MVP label
            var lblMvpTag = new Label
            {
                Text = "MVP DEL TORNEO",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 8, FontStyle.Bold),
                Location = new Point(90, 20),
                Size = new Size(280, 18),
                AutoSize = false
            };

            // Nombre
            var lblNombre = new Label
            {
                Text = mvp.Jugador.Nombre,
                ForeColor = Color.White,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(90, 40),
                Size = new Size(280, 28),
                AutoSize = false
            };

            // Equipo
            var lblEquipo = new Label
            {
                Text = mvp.Equipo != null ? mvp.Equipo.Nombre : "",
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 9),
                Location = new Point(90, 68),
                Size = new Size(280, 20),
                AutoSize = false
            };

            // Stats
            var statsPanel = new Panel
            {
                Location = new Point(10, 98),
                Size = new Size(365, 55),
                BackColor = Color.Transparent
            };

            string[] statLabels = { "PTS", "Fouls", "GP", "PPG" };
            string[] statValores =
            {
                mvp.Puntos.ToString(),
                mvp.Faltas.ToString(),
                mvp.Partidos.ToString(),
                mvp.Promedio.ToString("F1")
            };

            for (int i = 0; i < 4; i++)
            {
                var pnlStat = new Panel
                {
                    Location = new Point(i * 90, 0),
                    Size = new Size(88, 52),
                    BackColor = Color.FromArgb(18, 18, 18)
                };

                pnlStat.Region = System.Drawing.Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, 88, 52, 10, 10)
                );

                var lblVal = new Label
                {
                    Text = statValores[i],
                    ForeColor = Color.White,
                    Font = new Font("Arial", 13, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 5),
                    Size = new Size(88, 25),
                    AutoSize = false
                };

                var lblLab = new Label
                {
                    Text = statLabels[i],
                    ForeColor = Color.FromArgb(170, 170, 170),
                    Font = new Font("Arial", 8),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 30),
                    Size = new Size(88, 18),
                    AutoSize = false
                };

                pnlStat.Controls.Add(lblVal);
                pnlStat.Controls.Add(lblLab);
                statsPanel.Controls.Add(pnlStat);
            }

            card.Controls.Add(lblNum);
            card.Controls.Add(lblMvpTag);
            card.Controls.Add(lblNombre);
            card.Controls.Add(lblEquipo);
            card.Controls.Add(statsPanel);

            return card;
        }

        // =====================================================
        // CREAR FILA RANKING
        // =====================================================
        private Panel CrearFilaRanking(RankingItem r, int pos)
        {
            var fila = new Panel
            {
                Size = new Size(385, 50),
                BackColor = pos == 1
                    ? Color.FromArgb(40, 25, 5)
                    : Color.FromArgb(28, 28, 28)
            };

            fila.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, 385, 50, 10, 10)
            );

            if (pos == 1)
            {
                var borde = new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(4, 50),
                    BackColor = Color.FromArgb(244, 123, 37)
                };

                fila.Controls.Add(borde);
            }

            var lblPos = new Label
            {
                Text = pos.ToString(),
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 10),
                Location = new Point(15, 15),
                Size = new Size(25, 20),
                AutoSize = false
            };

            var lblNombre = new Label
            {
                Text = r.Jugador.Nombre,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(45, 8),
                Size = new Size(180, 18),
                AutoSize = false
            };

            string equipoNombre = r.Equipo != null ? r.Equipo.Nombre : "";

            var lblEquipo = new Label
            {
                Text = equipoNombre + " | Score: " +
                            r.Score.ToString("F1"),
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 8),
                Location = new Point(45, 28),
                Size = new Size(250, 16),
                AutoSize = false
            };

            fila.Controls.Add(lblPos);
            fila.Controls.Add(lblNombre);
            fila.Controls.Add(lblEquipo);

            return fila;
        }

        // =====================================================
        // CLASE INTERNA RANKING ITEM
        // =====================================================
        private class RankingItem
        {
            public Jugador Jugador { get; set; }
            public Equipo Equipo { get; set; }
            public int Puntos { get; set; }
            public int Faltas { get; set; }
            public int Partidos { get; set; }
            public double Promedio { get; set; }
            public double Score { get; set; }
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );
    }
}