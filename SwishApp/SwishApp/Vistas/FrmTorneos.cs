using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Google.Protobuf.WellKnownTypes;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;

namespace SwishApp.Vistas
{
    public partial class FrmTorneos : Form
    {
        private TorneoDao torneoDao = new TorneoDao();

        public FrmTorneos()
        {
            InitializeComponent();
            ConfigurarEstilos();
            CargarTorneos();
            NavBar.Agregar(this);
        }

        // =====================================================
        // CONFIGURAR ESTILOS
        // =====================================================
        private void ConfigurarEstilos()
        {
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.ForeColor = Color.White;
            this.Text = "Torneos";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        // =====================================================
        // CARGAR TORNEOS
        // =====================================================
        private void CargarTorneos()
        {
            this.Controls.Clear();

            // ── Título ──────────────────────────────────────
            var lblTitulo = new Label
            {
                Text = "Torneos",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 40),
                Size = new Size(400, 40),
                AutoSize = false
            };

            this.Controls.Add(lblTitulo);

            // ── Panel scrollable (reducido 60px abajo para el botón) ──
            var panel = new Panel
            {
                Location = new Point(0, 70),
                Size = new Size(400, 400),   // antes 460 — se acorta para el botón
                AutoScroll = true,
                BackColor = Color.FromArgb(18, 18, 18)
            };

            this.Controls.Add(panel);

            var torneos = torneoDao.Listar();

            if (torneos.Count == 0)
            {
                var lblVacio = new Label
                {
                    Text = "No hay torneos registrados.",
                    ForeColor = Color.FromArgb(170, 170, 170),
                    Font = new Font("Arial", 10),
                    Location = new Point(20, 20),
                    Size = new Size(360, 30),
                    AutoSize = false
                };

                panel.Controls.Add(lblVacio);
                // NO hacemos return aquí — el botón de abajo debe agregarse igual
            }
            else
            {
                int y = 10;

                foreach (var t in torneos)
                {
                    var card = CrearCardTorneo(t, y);
                    panel.Controls.Add(card);
                    y += 160;
                }
            }

            // ── Botón ← Inicio (siempre visible) ────────────
            // ── Botón ← Inicio (siempre visible) ────────────
            var btnInicio = new Button
            {
                Text = "← Inicio",
                Location = new Point(10, 480),   // ← corregido (antes 70)
                Size = new Size(380, 48),    // ← corregido (antes Size(3, 48))
                BackColor = Color.FromArgb(42, 42, 42),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnInicio.FlatAppearance.BorderSize = 0;
            btnInicio.Click += (s, e) =>
            {
                var frmInicio = new FrmInicio();
                frmInicio.Show();
                this.Close();
            };

            this.Controls.Add(btnInicio);
        }

        // =====================================================
        // CREAR CARD TORNEO
        // =====================================================
        private Panel CrearCardTorneo(Torneo t, int y)
        {
            var card = new Panel
            {
                Location = new Point(10, y),
                Size = new Size(365, 145),
                BackColor = Color.FromArgb(28, 28, 28)
            };

            card.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, 365, 145, 15, 15)
            );

            // Estado
            var lblEstado = new Label
            {
                Text = t.Estado,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(80, 50, 20),
                Font = new Font("Arial", 8, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(80, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            // Nombre
            var lblNombre = new Label
            {
                Text = t.Nombre,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 40),
                Size = new Size(200, 25),
                AutoSize = false
            };

            // Fecha
            var lblFecha = new Label
            {
                Text = t.FechaInicio,
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 9),
                Location = new Point(10, 65),
                Size = new Size(200, 20),
                AutoSize = false
            };

            // Botón acción
            string textoBtn = t.Estado == "FINALIZADO"
                ? "VER ESTADÍSTICAS" : "CONTINUAR";

            Color colorBtn = t.Estado == "FINALIZADO"
                ? Color.FromArgb(46, 134, 222)
                : Color.FromArgb(244, 123, 37);

            var btnAccion = new Button
            {
                Text = textoBtn,
                Location = new Point(10, 95),
                Size = new Size(230, 38),
                BackColor = colorBtn,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Tag = t
            };

            btnAccion.FlatAppearance.BorderSize = 0;
            btnAccion.Click += BtnAccion_Click;

            // Botón eliminar
            var btnEliminar = new Button
            {
                Text = "Eliminar",
                Location = new Point(255, 95),
                Size = new Size(100, 38),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Tag = t.Id
            };

            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;

            card.Controls.Add(lblEstado);
            card.Controls.Add(lblNombre);
            card.Controls.Add(lblFecha);
            card.Controls.Add(btnAccion);
            card.Controls.Add(btnEliminar);

            return card;
        }

        // =====================================================
        // BOTÓN ACCIÓN
        // =====================================================
        private void BtnAccion_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var torneo = (Torneo)btn.Tag;

            if (torneo.Estado == "FINALIZADO")
            {
                var frmRanking = new FrmRanking(torneo.Id);
                frmRanking.Show();
                this.Hide();
            }
            else
            {
                // Guardar torneo activo globalmente
                App.IdTorneoActivo = torneo.Id;

                var frmPartidos = new FrmPartidos();
                frmPartidos.Show();
                this.Hide();
            }
        }

        // =====================================================
        // BOTÓN ELIMINAR
        // =====================================================
        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            int id = (int)btn.Tag;

            var resultado = MessageBox.Show(
                "¿Eliminar este torneo y todos sus datos?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (resultado == DialogResult.Yes)
            {
                torneoDao.Eliminar(id);
                CargarTorneos();
            }
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );
    }
}