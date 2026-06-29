using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SwishApp.Logica;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;
using SwishApp.Vistas;

namespace SwishApp.Vistas
{
    public partial class FrmCaptura : Form
    {
        // ================= DAOs =================
        private PartidoDao partidoDao = new PartidoDao();
        private EquipoDao equipoDao = new EquipoDao();
        private JugadorDao jugadorDao = new JugadorDao();
        private EstadisticasDao estDao = new EstadisticasDao();
        private TorneoLogica logica = new TorneoLogica();

        // ================= DATOS =================
        private int idPartido;
        private Partido partido;
        private Equipo equipoA;
        private Equipo equipoB;
        private FrmPartidos frmPartidos;

        // ================= PESTAÑA ACTIVA =================
        private string pestañaActiva = "A";

        // ================= CONTROLES =================
        private Label lblMarcador;
        private Panel panelEquipoA;
        private Panel panelEquipoB;
        private Button btnTabA;
        private Button btnTabB;

        public FrmCaptura(int idPartido, FrmPartidos frmPartidos)
        {
            this.idPartido = idPartido;
            this.frmPartidos = frmPartidos;

            InitializeComponent();
            CargarDatos();
            NavBar.Agregar(this);
        }

        // =====================================================
        // CARGAR DATOS
        // =====================================================
        private void CargarDatos()
        {
            partido = partidoDao.BuscarPorId(idPartido);
            equipoA = equipoDao.BuscarPorId(partido.IdEquipoA);

            // BYE: no hay rival, mostrar pantalla simplificada
            if (partido.Bye || partido.IdEquipoB == 0)
            {
                ConfigurarFormularioBye();
                return;
            }

            equipoB = equipoDao.BuscarPorId(partido.IdEquipoB);
            ConfigurarFormulario();
            CargarJugadores();
        }

        // ── Nuevo método: agregar después de CargarDatos() ────
        private void ConfigurarFormularioBye()
        {
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.ForeColor = Color.White;
            this.Text = "Partido — BYE";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(420, 300);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            var lblIcon = new Label
            {
                Text = "🏆",
                Font = new Font("Arial", 36),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 28),
                Size = new Size(420, 60),
                AutoSize = false
            };

            var lblTitulo = new Label
            {
                Text = "BYE — Avance automático",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 98),
                Size = new Size(420, 28),
                AutoSize = false
            };

            var lblDesc = new Label
            {
                Text = (equipoA?.Nombre ?? "Equipo") + " avanza sin rival en esta ronda.",
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 132),
                Size = new Size(420, 22),
                AutoSize = false
            };

            var btnConfirmar = new Button
            {
                Text = "Confirmar avance",
                Location = new Point(20, 170),
                Size = new Size(380, 48),
                BackColor = Color.FromArgb(244, 123, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.Click += (s, e) =>
            {
                logica.FinalizarPartido(partido);
                frmPartidos.CargarPartidos();
                frmPartidos.Show();
                this.Close();
            };

            var btnVolver = new Button
            {
                Text = "← Volver",
                Location = new Point(20, 232),
                Size = new Size(380, 36),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10),
                Cursor = Cursors.Hand
            };
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Click += (s, e) =>
            {
                frmPartidos.Show();
                this.Close();
            };

            this.Controls.Add(lblIcon);
            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblDesc);
            this.Controls.Add(btnConfirmar);
            this.Controls.Add(btnVolver);
        }

        // =====================================================
        // CONFIGURAR FORMULARIO
        // =====================================================
        private void ConfigurarFormulario()
        {
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.ForeColor = Color.White;
            this.Text = "Captura de Datos";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(620, 920);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.AutoScroll = true;

            // =========================================
            // MARCADOR
            // =========================================
            int pA = estDao.ObtenerPuntosEquipo(idPartido, equipoA.Id);
            int pB = estDao.ObtenerPuntosEquipo(idPartido, equipoB.Id);

            lblMarcador = new Label
            {
                Text = pA + " - " + pB,
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 22, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(90, 50),
                Size = new Size(420, 40),
                AutoSize = false
            };

            var lblEquipos = new Label
            {
                Text = equipoA.Nombre + " vs " + equipoB.Nombre,
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(90, 95),
                Size = new Size(420, 25),
                AutoSize = false
            };

            this.Controls.Add(lblMarcador);
            this.Controls.Add(lblEquipos);

            // =========================================
            // PESTAÑAS
            // =========================================
            var panelTabs = new Panel
            {
                Location = new Point(95, 120),
                Size = new Size(385, 45),
                BackColor = Color.FromArgb(28, 28, 28)
            };

            panelTabs.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, 385, 45, 20, 20)
            );

            btnTabA = new Button
            {
                Text = equipoA.Nombre,
                Location = new Point(5, 5),
                Size = new Size(185, 35),
                BackColor = Color.FromArgb(244, 123, 37),
                ForeColor = Color.FromArgb(18, 18, 18),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnTabA.FlatAppearance.BorderSize = 0;
            btnTabA.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, 185, 35, 15, 15)
            );

            btnTabA.Click += (s, e) => MostrarEquipo("A");

            btnTabB = new Button
            {
                Text = equipoB.Nombre,
                Location = new Point(195, 5),
                Size = new Size(185, 35),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(244, 123, 37),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnTabB.FlatAppearance.BorderSize = 0;
            btnTabB.Click += (s, e) => MostrarEquipo("B");

            panelTabs.Controls.Add(btnTabA);
            panelTabs.Controls.Add(btnTabB);

            this.Controls.Add(panelTabs);

            // =========================================
            // PANEL JUGADORES
            // =========================================
            panelEquipoA = new Panel
            {
                Location = new Point(90, 170),
                Size = new Size(420, 580),
                AutoScroll = true,
                BackColor = Color.FromArgb(18, 18, 18),
                Visible = true
            };

            panelEquipoB = new Panel
            {
                Location = new Point(90, 170),
                Size = new Size(420, 580),
                AutoScroll = true,
                BackColor = Color.FromArgb(18, 18, 18),
                Visible = false
            };

            this.Controls.Add(panelEquipoA);
            this.Controls.Add(panelEquipoB);

            // =========================================
            // BOTÓN FINALIZAR PARTIDO
            // =========================================
            var btnFinalizar = new Button
            {
                Text = "Finalizar Partido",
                Location = new Point(105,780),
                Size = new Size(385, 50),
                BackColor = Color.FromArgb(244, 123, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnFinalizar.FlatAppearance.BorderSize = 0;
            btnFinalizar.Click += BtnFinalizar_Click;

            this.Controls.Add(btnFinalizar);
        }

        // =====================================================
        // CARGAR JUGADORES
        // =====================================================
        private void CargarJugadores()
        {
            panelEquipoA.Controls.Clear();
            panelEquipoB.Controls.Clear();

            var jugadoresA = jugadorDao.ListarPorEquipo(equipoA.Id);
            var jugadoresB = jugadorDao.ListarPorEquipo(equipoB.Id);

            int yA = 0;
            int yB = 0;

            foreach (var j in jugadoresA)
            {
                var card = CrearCardJugador(j, "A");
                card.Location = new Point(0, yA);
                panelEquipoA.Controls.Add(card);
                yA += 120;
            }

            foreach (var j in jugadoresB)
            {
                var card = CrearCardJugador(j, "B");
                card.Location = new Point(0, yB);
                panelEquipoB.Controls.Add(card);
                yB += 120;
            }

            // Actualizar marcador
            int pA = estDao.ObtenerPuntosEquipo(idPartido, equipoA.Id);
            int pB = estDao.ObtenerPuntosEquipo(idPartido, equipoB.Id);
            lblMarcador.Text = pA + " - " + pB;
        }

        // =====================================================
        // CREAR CARD JUGADOR
        // =====================================================
        private Panel CrearCardJugador(Jugador j, string equipo)
        {
            int faltas = estDao.ObtenerFaltas(idPartido, j.Id);
            int puntos = estDao.ObtenerPuntos(idPartido, j.Id);
            bool expuls = faltas >= 5;

            var card = new Panel
            {
                Size = new Size(580, 124),
                BackColor = expuls
                    ? Color.FromArgb(30, 30, 30)
                    : Color.FromArgb(28, 28, 28)
            };

            if (expuls)
                card.BackColor = Color.FromArgb(20, 20, 20);

            // Número
            var lblNum = new Label
            {
                Text = j.Numero.ToString(),
                ForeColor = Color.FromArgb(244, 123, 37),
                BackColor = Color.FromArgb(50, 30, 10),
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 15),
                Size = new Size(45, 45),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            lblNum.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, 45, 45, 22, 22)
            );

            // Nombre
            var lblNombre = new Label
            {
                Text = j.Nombre,
                ForeColor = expuls ? Color.Gray : Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(65, 15),
                Size = new Size(180, 20),
                AutoSize = false
            };

            // Puntos
            var lblPuntos = new Label
            {
                Text = puntos + " PTS",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(270, 15),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false
            };

            // Faltas
            var lblFaltas = new Label
            {
                Text = "Faltas: " + faltas,
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 8),
                Location = new Point(65, 38),
                Size = new Size(100, 20),
                AutoSize = false
            };

            // Botones
            var btn1 = CrearBtnPuntos("+1", j, equipo, 1, expuls, 10, 70);
            var btn2 = CrearBtnPuntos("+2", j, equipo, 2, expuls, 105, 70);
            var btn3 = CrearBtnPuntos("+3", j, equipo, 3, expuls, 200, 70);

            var btnFalta = new Button
            {
                Text = "Falta",
                Location = new Point(295, 70),
                Size = new Size(78, 32),
                BackColor = expuls
                    ? Color.Gray
                    : Color.FromArgb(244, 123, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Cursor = expuls ? Cursors.Default : Cursors.Hand,
                Enabled = !expuls,
                Tag = j
            };

            btnFalta.FlatAppearance.BorderSize = 0;
            btnFalta.Click += (s, e) =>
            {
                var jug = (Jugador)((Button)s).Tag;
                estDao.SumarFalta(idPartido, jug.Id);
                CargarJugadores();
            };

            card.Controls.Add(lblNum);
            card.Controls.Add(lblNombre);
            card.Controls.Add(lblPuntos);
            card.Controls.Add(lblFaltas);
            card.Controls.Add(btn1);
            card.Controls.Add(btn2);
            card.Controls.Add(btn3);
            card.Controls.Add(btnFalta);

            return card;
        }

        // =====================================================
        // CREAR BOTÓN PUNTOS
        // =====================================================
        private Button CrearBtnPuntos(
            string texto, Jugador j, string equipo,
            int puntos, bool expuls, int x, int y)
        {
            var btn = new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(85, 32),
                BackColor = expuls
                    ? Color.Gray
                    : Color.FromArgb(42, 42, 42),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = expuls ? Cursors.Default : Cursors.Hand,
                Enabled = !expuls,
                Tag = new object[] { j, puntos }
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) =>
            {
                var datos = (object[])((Button)s).Tag;
                var jug = (Jugador)datos[0];
                int pts = (int)datos[1];

                estDao.SumarPuntos(idPartido, jug.Id, pts);
                estDao.ActualizarMarcador(idPartido);
                CargarJugadores();
            };

            return btn;
        }

        // =====================================================
        // MOSTRAR EQUIPO
        // =====================================================
        private void MostrarEquipo(string equipo)
        {
            pestañaActiva = equipo;

            if (equipo == "A")
            {
                panelEquipoA.Visible = true;
                panelEquipoB.Visible = false;
                btnTabA.BackColor = Color.FromArgb(244, 123, 37);
                btnTabA.ForeColor = Color.FromArgb(18, 18, 18);
                btnTabB.BackColor = Color.Transparent;
                btnTabB.ForeColor = Color.FromArgb(244, 123, 37);
            }
            else
            {
                panelEquipoA.Visible = false;
                panelEquipoB.Visible = true;
                btnTabB.BackColor = Color.FromArgb(244, 123, 37);
                btnTabB.ForeColor = Color.FromArgb(18, 18, 18);
                btnTabA.BackColor = Color.Transparent;
                btnTabA.ForeColor = Color.FromArgb(244, 123, 37);
            }
        }

        // =====================================================
        // FINALIZAR PARTIDO
        // =====================================================
        private void BtnFinalizar_Click(object sender, EventArgs e)
        {
            partido = partidoDao.BuscarPorId(idPartido);

            if (partido.PuntosA == 0 && partido.PuntosB == 0)
            {
                var conf = MessageBox.Show(
                    "El marcador es 0-0. ¿Deseas finalizar el partido?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (conf != DialogResult.Yes) return;
            }

            logica.FinalizarPartido(partido);

            frmPartidos.CargarPartidos();
            frmPartidos.Show();
            this.Close();
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        private void FrmCaptura_Load(object sender, EventArgs e)
        {

        }
    }
}