using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;

namespace SwishApp.Vistas
{
    public partial class FrmRegistro : Form
    {
        // ================= DAOs =================
        private EquipoDao equipoDao = new EquipoDao();
        private JugadorDao jugadorDao = new JugadorDao();
        private TorneoDao torneoDao = new TorneoDao();

        // ================= CONTROLES =================
        private TextBox txtNombreEquipo;
        private TextBox txtOrigen;
        private TextBox txtCategoria;
        private TextBox txtNombreJugador;
        private NumericUpDown nudNumero;
        private ComboBox cmbPosicion;
        private Panel panelJugadores;
        private Panel panelEquipos;

        public FrmRegistro()
        {
            InitializeComponent();
            ConfigurarFormulario();
            CargarEquipos();
        }

        // =====================================================
        // CONFIGURAR FORMULARIO
        // =====================================================
        private void ConfigurarFormulario()
        {
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.ForeColor = Color.White;
            this.Text = "Registro de Equipos";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(420, 750);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.AutoScroll = true;

            // =========================================
            // TÍTULO
            // =========================================
            var lblTitulo = new Label
            {
                Text = "Registro de Equipos",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 15),
                Size = new Size(420, 35),
                AutoSize = false
            };

            this.Controls.Add(lblTitulo);

            // =========================================
            // SECCIÓN EQUIPO
            // =========================================
            var cardEquipo = CrearCard(new Point(15, 60), new Size(385, 220));

            var lblSecEquipo = new Label
            {
                Text = "Datos del equipo",
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(360, 25),
                AutoSize = false
            };

            txtNombreEquipo = CrearTextBox("Nombre del equipo", new Point(10, 45));
            txtOrigen = CrearTextBox("Origen o ciudad", new Point(10, 95));
            txtCategoria = CrearTextBox("Categoría", new Point(10, 145));

            var btnGuardarEquipo = CrearBoton(
                "Guardar Equipo",
                new Point(10, 180),
                new Size(365, 35),
                Color.FromArgb(244, 123, 37)
            );

            btnGuardarEquipo.Click += BtnGuardarEquipo_Click;

            cardEquipo.Controls.Add(lblSecEquipo);
            cardEquipo.Controls.Add(txtNombreEquipo);
            cardEquipo.Controls.Add(txtOrigen);
            cardEquipo.Controls.Add(txtCategoria);
            cardEquipo.Controls.Add(btnGuardarEquipo);

            this.Controls.Add(cardEquipo);

            // =========================================
            // SECCIÓN JUGADORES
            // =========================================
            var cardJugador = CrearCard(new Point(15, 295), new Size(385, 360));

            var lblSecJugador = new Label
            {
                Text = "Integrantes",
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(360, 25),
                AutoSize = false
            };

            txtNombreJugador = CrearTextBox("Nombre del jugador", new Point(10, 45));

            var lblNumero = new Label
            {
                Text = "Número",
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 9),
                Location = new Point(10, 90),
                Size = new Size(100, 20),
                AutoSize = false
            };

            nudNumero = new NumericUpDown
            {
                Location = new Point(10, 110),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                Font = new Font("Arial", 10),
                Minimum = 0,
                Maximum = 99
            };

            var lblPosicion = new Label
            {
                Text = "Posición",
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 9),
                Location = new Point(120, 90),
                Size = new Size(240, 20),
                AutoSize = false
            };

            cmbPosicion = new ComboBox
            {
                Location = new Point(120, 110),
                Size = new Size(245, 30),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                Font = new Font("Arial", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbPosicion.Items.AddRange(new object[]
            {
                "Base (PG)", "Escolta (SG)", "Alero (SF)",
                "Ala-Pívot (PF)", "Pívot (C)"
            });

            cmbPosicion.SelectedIndex = 0;

            var btnAgregarJugador = CrearBoton(
                "Agregar Jugador",
                new Point(10, 150),
                new Size(365, 35),
                Color.FromArgb(50, 50, 50)
            );

            btnAgregarJugador.Click += BtnAgregarJugador_Click;

            // Lista jugadores Y=195
            panelJugadores = new Panel
            {
                Location = new Point(10, 195),
                Size = new Size(365, 80),
                AutoScroll = true,
                BackColor = Color.FromArgb(40, 40, 40)
            };

            // Botón nuevo equipo Y=285
            var btnNuevoEquipo = CrearBoton(
                "+ Agregar Nuevo Equipo",
                new Point(10, 285),
                new Size(365, 35),
                Color.FromArgb(30, 30, 30)
            );

            btnNuevoEquipo.Click += BtnNuevoEquipo_Click;

            cardJugador.Controls.Add(lblSecJugador);
            cardJugador.Controls.Add(txtNombreJugador);
            cardJugador.Controls.Add(lblNumero);
            cardJugador.Controls.Add(nudNumero);
            cardJugador.Controls.Add(lblPosicion);
            cardJugador.Controls.Add(cmbPosicion);
            cardJugador.Controls.Add(btnAgregarJugador);
            cardJugador.Controls.Add(panelJugadores);
            cardJugador.Controls.Add(btnNuevoEquipo);

            this.Controls.Add(cardJugador);

            // =========================================
            // SECCIÓN EQUIPOS REGISTRADOS
            // =========================================
            var lblEquiposReg = new Label
            {
                Text = "Equipos registrados",
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(15, 590),
                Size = new Size(385, 25),
                AutoSize = false
            };

            this.Controls.Add(lblEquiposReg);

            panelEquipos = new Panel
            {
                Location = new Point(15, 620),
                Size = new Size(385, 180),
                AutoScroll = true,
                BackColor = Color.FromArgb(18, 18, 18)
            };

            this.Controls.Add(panelEquipos);

            // =========================================
            // BOTÓN FINALIZAR REGISTRO
            // =========================================
            var btnFinalizar = CrearBoton(
                "Finalizar Registro y Generar Partidos",
                new Point(15, 810),
                new Size(385, 50),
                Color.FromArgb(244, 123, 37)
            );

            btnFinalizar.Click += BtnFinalizar_Click;

            this.Controls.Add(btnFinalizar);
        }

        // =====================================================
        // GUARDAR EQUIPO
        // =====================================================
        private void BtnGuardarEquipo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreEquipo.Text) ||
                string.IsNullOrWhiteSpace(txtOrigen.Text) ||
                string.IsNullOrWhiteSpace(txtCategoria.Text))
            {
                MessageBox.Show(
                    "Completa todos los campos del equipo.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var torneo = torneoDao.ObtenerUltimo();

            var equipo = new Equipo
            {
                Nombre = txtNombreEquipo.Text.Trim(),
                Origen = txtOrigen.Text.Trim(),
                Categoria = txtCategoria.Text.Trim(),
                IdTorneo = torneo.Id,
                Estado = "ACTIVO",
                Derrotas = 0
            };

            equipoDao.Insertar(equipo);

            // Limpiar campos
            txtNombreEquipo.Text = "";
            txtOrigen.Text = "";
            txtCategoria.Text = "";

            CargarEquipos();
            CargarJugadoresUltimoEquipo();

            MessageBox.Show(
                "Equipo guardado correctamente.",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // =====================================================
        // AGREGAR JUGADOR
        // =====================================================
        private void BtnAgregarJugador_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreJugador.Text))
            {
                MessageBox.Show(
                    "Escribe el nombre del jugador.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var equipos = equipoDao.Listar();

            if (equipos.Count == 0)
            {
                MessageBox.Show(
                    "Primero registra un equipo.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var ultimoEquipo = equipos[equipos.Count - 1];

            int numero = (int)nudNumero.Value;

            if (jugadorDao.ExisteNumeroEnEquipo(numero, ultimoEquipo.Id))
            {
                MessageBox.Show(
                    "El número " + numero + " ya está registrado en este equipo.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            var jugador = new Jugador
            {
                Nombre = txtNombreJugador.Text.Trim(),
                Numero = numero,
                Posicion = cmbPosicion.SelectedItem.ToString(),
                IdEquipo = ultimoEquipo.Id
            };

            jugadorDao.Insertar(jugador);

            txtNombreJugador.Text = "";
            nudNumero.Value = 0;

            CargarJugadoresUltimoEquipo();
        }

        // =====================================================
        // NUEVO EQUIPO
        // =====================================================
        private void BtnNuevoEquipo_Click(object sender, EventArgs e)
        {
            var equipos = equipoDao.Listar();

            if (equipos.Count == 0)
            {
                MessageBox.Show(
                    "Primero guarda un equipo.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var ultimoEquipo = equipos[equipos.Count - 1];
            var jugadores = jugadorDao.ListarPorEquipo(ultimoEquipo.Id);

            if (jugadores.Count < 5)
            {
                MessageBox.Show(
                    "El equipo " + ultimoEquipo.Nombre +
                    " necesita mínimo 5 jugadores antes de agregar otro.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Limpiar campos para nuevo equipo
            txtNombreEquipo.Text = "Nombre del equipo";
            txtNombreEquipo.ForeColor = Color.FromArgb(120, 120, 120);

            txtOrigen.Text = "Origen o ciudad";
            txtOrigen.ForeColor = Color.FromArgb(120, 120, 120);

            txtCategoria.Text = "Categoría";
            txtCategoria.ForeColor = Color.FromArgb(120, 120, 120);

            panelJugadores.Controls.Clear();

            MessageBox.Show(
                "Puedes registrar el siguiente equipo.",
                "Listo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // =====================================================
        // FINALIZAR REGISTRO
        // =====================================================
        private void BtnFinalizar_Click(object sender, EventArgs e)
        {
            var equipos = equipoDao.Listar();

            if (equipos.Count < 2)
            {
                MessageBox.Show(
                    "Necesitas al menos 2 equipos para generar partidos.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Validar que cada equipo tenga mínimo 5 jugadores
            foreach (var eq in equipos)
            {
                var jugadores = jugadorDao.ListarPorEquipo(eq.Id);

                if (jugadores.Count < 5)
                {
                    MessageBox.Show(
                        "El equipo " + eq.Nombre +
                        " necesita mínimo 5 jugadores.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
            }

            // Generar partidos
            GenerarPartidos(equipos);

            App.IdTorneoActivo = equipos[0].IdTorneo;

            var frmPartidos = new FrmPartidos();
            frmPartidos.Show();
            this.Hide();
        }

        // =====================================================
        // GENERAR PARTIDOS INICIALES
        // =====================================================
        private void GenerarPartidos(List<Equipo> equipos)
        {
            var partidoDao = new PartidoDao();

            // Verificar solo partidos del torneo actual
            var existentes = partidoDao.ListarPorTorneo(equipos[0].IdTorneo);
            if (existentes.Count > 0) return;

            int idEquipoBye = 0;

            // BYE si número impar
            if (equipos.Count % 2 != 0)
            {
                var rnd = new Random();
                int indice = rnd.Next(equipos.Count);

                idEquipoBye = equipos[indice].Id;

                var bye = new Partido
                {
                    Nombre = equipos[indice].Nombre + " BYE",
                    Estado = "FINALIZADO",
                    PuntosA = 0,
                    PuntosB = 0,
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                    Bracket = "WINNERS",
                    Ronda = 1,
                    IdEquipoA = idEquipoBye,
                    IdEquipoB = 0,
                    IdTorneo = equipos[indice].IdTorneo,
                    Ganador = idEquipoBye,
                    Perdedor = 0,
                    Bye = true
                };

                partidoDao.Insertar(bye);
            }

            // Partidos normales
            for (int i = 0; i < equipos.Count; i++)
            {
                if (equipos[i].Id == idEquipoBye) continue;

                int j = i + 1;

                while (j < equipos.Count &&
                       equipos[j].Id == idEquipoBye)
                    j++;

                if (j >= equipos.Count) break;

                var eqA = equipos[i];
                var eqB = equipos[j];

                var partido = new Partido
                {
                    Nombre = eqA.Nombre + " VS " + eqB.Nombre,
                    Estado = "PENDIENTE",
                    PuntosA = 0,
                    PuntosB = 0,
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                    Bracket = "WINNERS",
                    Ronda = 1,
                    IdEquipoA = eqA.Id,
                    IdEquipoB = eqB.Id,
                    IdTorneo = eqA.IdTorneo,
                    Bye = false
                };

                partidoDao.Insertar(partido);

                i = j;
            }
        }

        // =====================================================
        // CARGAR EQUIPOS
        // =====================================================
        private void CargarEquipos()
        {
            panelEquipos.Controls.Clear();

            var equipos = equipoDao.Listar();
            int y = 0;

            foreach (var eq in equipos)
            {
                var card = new Panel
                {
                    Location = new Point(0, y),
                    Size = new Size(365, 60),
                    BackColor = Color.FromArgb(28, 28, 28)
                };

                var lblNombre = new Label
                {
                    Text = eq.Nombre,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Location = new Point(10, 8),
                    Size = new Size(200, 20),
                    AutoSize = false
                };

                var lblInfo = new Label
                {
                    Text = eq.Origen + " -- " + eq.Categoria,
                    ForeColor = Color.FromArgb(170, 170, 170),
                    Font = new Font("Arial", 8),
                    Location = new Point(10, 32),
                    Size = new Size(200, 20),
                    AutoSize = false
                };

                var btnEliminar = new Button
                {
                    Text = "X",
                    Location = new Point(320, 15),
                    Size = new Size(35, 30),
                    BackColor = Color.FromArgb(220, 53, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = eq.Id
                };

                btnEliminar.FlatAppearance.BorderSize = 0;
                btnEliminar.Click += (s, ev) =>
                {
                    var btn = (Button)s;
                    equipoDao.Eliminar((int)btn.Tag);
                    CargarEquipos();
                };

                card.Controls.Add(lblNombre);
                card.Controls.Add(lblInfo);
                card.Controls.Add(btnEliminar);

                panelEquipos.Controls.Add(card);
                y += 65;
            }
        }

        // =====================================================
        // CARGAR JUGADORES DEL ÚLTIMO EQUIPO
        // =====================================================
        private void CargarJugadoresUltimoEquipo()
        {
            panelJugadores.Controls.Clear();

            var equipos = equipoDao.Listar();
            if (equipos.Count == 0) return;

            var ultimoEquipo = equipos[equipos.Count - 1];
            var jugadores = jugadorDao.ListarPorEquipo(ultimoEquipo.Id);

            int y = 0;

            foreach (var j in jugadores)
            {
                var fila = new Panel
                {
                    Location = new Point(0, y),
                    Size = new Size(360, 30),
                    BackColor = Color.FromArgb(40, 40, 40)
                };

                var lbl = new Label
                {
                    Text = "#" + j.Numero + " " + j.Nombre,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 9),
                    Location = new Point(5, 5),
                    Size = new Size(270, 20),
                    AutoSize = false
                };

                var btnEliminar = new Button
                {
                    Text = "X",
                    Location = new Point(320, 3),
                    Size = new Size(30, 24),
                    BackColor = Color.FromArgb(220, 53, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 8, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = j.Id
                };

                btnEliminar.FlatAppearance.BorderSize = 0;
                btnEliminar.Click += (s, ev) =>
                {
                    var btn = (Button)s;
                    var jugActual = jugadorDao.ListarPorEquipo(ultimoEquipo.Id);

                    if (jugActual.Count <= 5)
                    {
                        MessageBox.Show(
                            "El equipo necesita mínimo 5 jugadores.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;
                    }

                    jugadorDao.Eliminar((int)btn.Tag);
                    CargarJugadoresUltimoEquipo();
                };

                fila.Controls.Add(lbl);
                fila.Controls.Add(btnEliminar);
                panelJugadores.Controls.Add(fila);

                y += 32;
            }
        }

        // =====================================================
        // HELPERS
        // =====================================================
        private Panel CrearCard(Point ubicacion, Size tamaño)
        {
            var card = new Panel
            {
                Location = ubicacion,
                Size = tamaño,
                BackColor = Color.FromArgb(28, 28, 28),
                Padding = new Padding(10)
            };

            return card;
        }

        private TextBox CrearTextBox(string placeholder, Point ubicacion)
        {
            var txt = new TextBox
            {
                Location = ubicacion,
                Size = new Size(365, 30),
                BackColor = Color.FromArgb(40, 40, 40),
                //ForeColor = Color.White,
                Font = new Font("Arial", 10),
                BorderStyle = BorderStyle.FixedSingle,
                Text = placeholder,
                ForeColor = Color.FromArgb(120, 120, 120)
            };

            txt.GotFocus += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.White;
                }
            };

            txt.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.FromArgb(120, 120, 120);
                }
            };

            return txt;
        }

        private Button CrearBoton(
            string texto, Point ubicacion, Size tamaño, Color color)
        {
            var btn = new Button
            {
                Text = texto,
                Location = ubicacion,
                Size = tamaño,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;

            return btn;
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );
    }
}