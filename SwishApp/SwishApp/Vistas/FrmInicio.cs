using System;
using System.Drawing;
using System.Windows.Forms;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;

namespace SwishApp.Vistas
{
    public partial class FrmInicio : Form
    {
        // ================= DAO =================
        private TorneoDao torneoDao = new TorneoDao();

        // ================= OPCIÓN SELECCIONADA =================
        private string opcionSeleccionada = "";

        public FrmInicio()
        {
            InitializeComponent();
            ConfigurarEstilos();
        }

        // =====================================================
        // CONFIGURAR ESTILOS
        // =====================================================
        private void ConfigurarEstilos()
        {
            // Formulario
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.ForeColor = Color.White;
            this.Text = "Swish App";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // =========================================
            // TÍTULO
            // =========================================
            var lblTitulo = new Label
            {
                Text = "Gestión de Torneos",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 40),
                Size = new Size(400, 40),
                AutoSize = false
            };

            // =========================================
            // CARD TORNEO RÁFAGA
            // =========================================
            var pnlRafaga = CrearCard(
                "🏀 Torneo Ráfaga",
                "Eliminación directa, partidos rápidos.",
                new Point(20, 120)
            );

            // =========================================
            // CARD VER TORNEOS
            // =========================================
            var pnlVerTorneos = CrearCard(
                "📅 Ver Torneos",
                "Ver torneos existentes o finalizados.",
                new Point(20, 240)
            );

            // =========================================
            // EVENTOS CARD RÁFAGA
            // =========================================
            pnlRafaga.Click += (s, e) =>
            {
                SeleccionarOpcion(pnlRafaga, pnlVerTorneos);
                opcionSeleccionada = "rafaga";
            };

            foreach (Control c in pnlRafaga.Controls)
            {
                c.Click += (s, e) =>
                {
                    SeleccionarOpcion(pnlRafaga, pnlVerTorneos);
                    opcionSeleccionada = "rafaga";
                };
            }

            // =========================================
            // EVENTOS CARD VER TORNEOS
            // =========================================
            pnlVerTorneos.Click += (s, e) =>
            {
                SeleccionarOpcion(pnlVerTorneos, pnlRafaga);
                opcionSeleccionada = "verTorneos";
            };

            foreach (Control c in pnlVerTorneos.Controls)
            {
                c.Click += (s, e) =>
                {
                    SeleccionarOpcion(pnlVerTorneos, pnlRafaga);
                    opcionSeleccionada = "verTorneos";
                };
            }

            // =========================================
            // BOTÓN CONTINUAR
            // =========================================
            var btnContinuar = new Button
            {
                Text = "Continuar",
                Location = new Point(20, 380),
                Size = new Size(360, 50),
                BackColor = Color.FromArgb(244, 123, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnContinuar.FlatAppearance.BorderSize = 0;
            btnContinuar.Click += BtnContinuar_Click;

            // Agregar controles
            this.Controls.Add(lblTitulo);
            this.Controls.Add(pnlRafaga);
            this.Controls.Add(pnlVerTorneos);
            this.Controls.Add(btnContinuar);
        }

        // =====================================================
        // CREAR CARD
        // =====================================================
        private Panel CrearCard(
            string titulo, string descripcion, Point ubicacion)
        {
            var panel = new Panel
            {
                Location = ubicacion,
                Size = new Size(360, 90),
                BackColor = Color.FromArgb(28, 28, 28),
                Cursor = Cursors.Hand
            };

            panel.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, 360, 90, 15, 15)
            );

            var lblTitulo = new Label
            {
                Text = titulo,
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(15, 15),
                Size = new Size(330, 25),
                AutoSize = false
            };

            var lblDesc = new Label
            {
                Text = descripcion,
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 9),
                Location = new Point(15, 45),
                Size = new Size(330, 30),
                AutoSize = false
            };

            panel.Controls.Add(lblTitulo);
            panel.Controls.Add(lblDesc);

            return panel;
        }

        // =====================================================
        // SELECCIONAR OPCIÓN
        // =====================================================
        private void SeleccionarOpcion(Panel seleccionado, Panel otro)
        {
            seleccionado.BackColor = Color.FromArgb(50, 30, 10);

            foreach (Control c in seleccionado.Controls)
                if (c is Label lbl && lbl.Font.Bold)
                    lbl.ForeColor = Color.FromArgb(244, 123, 37);

            otro.BackColor = Color.FromArgb(28, 28, 28);

            foreach (Control c in otro.Controls)
                if (c is Label lbl && lbl.Font.Bold)
                    lbl.ForeColor = Color.White;
        }

        // =====================================================
        // BOTÓN CONTINUAR
        // =====================================================
        private void BtnContinuar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(opcionSeleccionada))
            {
                MessageBox.Show(
                    "Selecciona una opción.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (opcionSeleccionada == "verTorneos")
            {
                var frmTorneos = new FrmTorneos();
                frmTorneos.Show();
                this.Hide();
            }
            else
            {
                // Crear torneo nuevo
                var torneo = new Torneo
                {
                    Nombre = "Rafaga",
                    Tipo = "rafaga",
                    Estado = "ACTIVO",
                    FechaInicio = DateTime.Now.ToString("yyyy-MM-dd")
                };

                torneoDao.Insertar(torneo);

                var frmRegistro = new FrmRegistro();
                frmRegistro.Show();
                this.Hide();
            }
        }

        // =====================================================
        // ROUNDED CORNERS (WIN32)
        // =====================================================
        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );
    }
}