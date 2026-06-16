using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;
using SwishApp.Vistas;

namespace SwishApp.Vistas
{
    public partial class FrmPartidos : Form
    {
        private PartidoDao partidoDao = new PartidoDao();
        private EquipoDao equipoDao = new EquipoDao();
        private TorneoDao torneoDao = new TorneoDao();

        public FrmPartidos()
        {
            InitializeComponent();
            ConfigurarFormulario();
            CargarPartidos();
        }

        // =====================================================
        // CONFIGURAR FORMULARIO
        // =====================================================
        private void ConfigurarFormulario()
        {
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.ForeColor = Color.White;
            this.Text = "Partidos del Torneo";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(420, 700);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        // =====================================================
        // CARGAR PARTIDOS
        // =====================================================
        public void CargarPartidos()
        {
            this.Controls.Clear();

            // Título
            var lblTitulo = new Label
            {
                Text = "Gráfica del Torneo",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 15),
                Size = new Size(420, 35),
                AutoSize = false
            };

            this.Controls.Add(lblTitulo);

            // Panel scrollable
            var panel = new Panel
            {
                Location = new Point(0, 60),
                Size = new Size(420, 560),
                AutoScroll = true,
                BackColor = Color.FromArgb(18, 18, 18)
            };

            this.Controls.Add(panel);

            List<Partido> partidos;

            if (App.IdTorneoActivo > 0)
                partidos = partidoDao.ListarPorTorneo(App.IdTorneoActivo);
            else
                partidos = partidoDao.Listar();

            int y = 10;

            bool granFinalFinalizada = false;

            foreach (var p in partidos)
            {
                // Ocultar BYEs
                if (p.IdEquipoB == 0) continue;

                if (p.Bracket == "GRAN_FINAL" &&
                    p.Estado == "FINALIZADO")
                    granFinalFinalizada = true;

                var eqA = equipoDao.BuscarPorId(p.IdEquipoA);
                var eqB = equipoDao.BuscarPorId(p.IdEquipoB);

                var card = CrearCardPartido(p, eqA, eqB, y);
                panel.Controls.Add(card);

                y += 140;
            }

            // Botón finalizar torneo
            if (granFinalFinalizada)
            {
                var btnFinalizar = new Button
                {
                    Text = "🏆 FINALIZAR TORNEO",
                    Location = new Point(15, y + 10),
                    Size = new Size(380, 50),
                    BackColor = Color.FromArgb(244, 123, 37),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 11, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };

                btnFinalizar.FlatAppearance.BorderSize = 0;
                btnFinalizar.Click += BtnFinalizarTorneo_Click;

                panel.Controls.Add(btnFinalizar);
            }

            if (partidos.Count == 0)
            {
                var lblVacio = new Label
                {
                    Text = "No hay partidos generados.",
                    ForeColor = Color.FromArgb(170, 170, 170),
                    Font = new Font("Arial", 10),
                    Location = new Point(20, 20),
                    Size = new Size(360, 30),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false
                };

                panel.Controls.Add(lblVacio);
            }
        }

        // =====================================================
        // CREAR CARD PARTIDO
        // =====================================================
        private Panel CrearCardPartido(
            Partido p, Equipo eqA, Equipo eqB, int y)
        {
            var card = new Panel
            {
                Location = new Point(10, y),
                Size = new Size(385, 125),
                BackColor = Color.FromArgb(28, 28, 28)
            };

            // Estado
            Color colorEstado = p.Estado == "FINALIZADO"
                ? Color.FromArgb(80, 50, 20)
                : Color.FromArgb(20, 80, 20);

            var lblEstado = new Label
            {
                Text = p.Estado,
                ForeColor = Color.White,
                BackColor = colorEstado,
                Font = new Font("Arial", 8, FontStyle.Bold),
                Location = new Point(10, 8),
                Size = new Size(90, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            // Equipos
            var lblEqA = new Label
            {
                Text = eqA != null ? eqA.Nombre : "?",
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(10, 40),
                Size = new Size(130, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false
            };

            var lblVs = new Label
            {
                Text = "VS",
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 10),
                Location = new Point(155, 40),
                Size = new Size(40, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            var lblEqB = new Label
            {
                Text = eqB != null ? eqB.Nombre : "?",
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(210, 40),
                Size = new Size(130, 25),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false
            };

            // Botón
            string textoBtn = p.Estado == "FINALIZADO"
                ? "VER ESTADÍSTICAS" : "INICIAR PARTIDO";

            Color colorBtn = p.Estado == "FINALIZADO"
                ? Color.FromArgb(46, 134, 222)
                : Color.FromArgb(244, 123, 37);

            var btn = new Button
            {
                Text = textoBtn,
                Location = new Point(10, 78),
                Size = new Size(365, 38),
                BackColor = colorBtn,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Tag = p
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.Click += BtnPartido_Click;

            card.Controls.Add(lblEstado);
            card.Controls.Add(lblEqA);
            card.Controls.Add(lblVs);
            card.Controls.Add(lblEqB);
            card.Controls.Add(btn);

            return card;
        }

        // =====================================================
        // BOTÓN PARTIDO
        // =====================================================
        private void BtnPartido_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var partido = (Partido)btn.Tag;

            if (partido.Estado == "FINALIZADO")
            {
                var frmRanking = new FrmRanking(App.IdTorneoActivo);
                frmRanking.Show();
                this.Hide();
            }
            else
            {
                var frmCaptura = new FrmCaptura(partido.Id, this);
                frmCaptura.Show();
                this.Hide();
            }
        }

        // =====================================================
        // FINALIZAR TORNEO
        // =====================================================
        private void BtnFinalizarTorneo_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "¿Finalizar el torneo?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                torneoDao.Finalizar(App.IdTorneoActivo);

                MessageBox.Show(
                    "Torneo finalizado.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                var frmInicio = new FrmInicio();
                frmInicio.Show();
                this.Close();
            }
        }
    }
}