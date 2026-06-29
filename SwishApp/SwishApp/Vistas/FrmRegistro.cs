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
            NavBar.Agregar(this);
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
            // Form más alto para que todo quepa cómodamente
            this.Size = new Size(480, 920);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.AutoScroll = true;

            int margen = 16;
            int ancho = 448; // 480 - 2*margen

            // ── TÍTULO ────────────────────────────────────
            // Y=64 → deja 46px para NavBar + 18px de margen
            this.Controls.Add(new Label
            {
                Text = "Registro de Equipos",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 17, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 64),
                Size = new Size(480, 36),
                AutoSize = false
            });

            // =====================================================
            // CARD — DATOS DEL EQUIPO  (baja 46px por NavBar)
            // =====================================================
            var cardEquipo = CrearCard(new Point(margen, 112), new Size(ancho, 235));

            cardEquipo.Controls.Add(CrearSectionLabel("Datos del equipo", new Point(12, 12)));

            txtNombreEquipo = CrearTextBox("Nombre del equipo", new Point(12, 42));
            txtOrigen = CrearTextBox("Origen o ciudad", new Point(12, 88));
            txtCategoria = CrearTextBox("Categoría", new Point(12, 134));

            var btnGuardar = CrearBoton(
                "Guardar equipo",
                new Point(12, 182),
                new Size(ancho - 24, 40),
                Color.FromArgb(244, 123, 37));
            btnGuardar.Click += BtnGuardarEquipo_Click;

            cardEquipo.Controls.Add(txtNombreEquipo);
            cardEquipo.Controls.Add(txtOrigen);
            cardEquipo.Controls.Add(txtCategoria);
            cardEquipo.Controls.Add(btnGuardar);
            this.Controls.Add(cardEquipo);

            // =====================================================
            // CARD — INTEGRANTES  (baja con el resto del layout)
            // Height=370 para que btnNuevoEquipo quepa holgado
            // =====================================================
            var cardJugador = CrearCard(new Point(margen, 362), new Size(ancho, 370));

            cardJugador.Controls.Add(CrearSectionLabel("Integrantes del equipo", new Point(12, 12)));

            txtNombreJugador = CrearTextBox("Nombre del jugador", new Point(12, 42));

            // Fila número + posición
            cardJugador.Controls.Add(new Label
            {
                Text = "Número",
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Arial", 9),
                Location = new Point(12, 92),
                Size = new Size(80, 18),
                AutoSize = false
            });

            nudNumero = new NumericUpDown
            {
                Location = new Point(12, 112),
                Size = new Size(90, 34),
                BackColor = Color.FromArgb(38, 38, 38),
                ForeColor = Color.White,
                Font = new Font("Arial", 11),
                Minimum = 0,
                Maximum = 99,
                BorderStyle = BorderStyle.FixedSingle
            };

            cardJugador.Controls.Add(new Label
            {
                Text = "Posición",
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Arial", 9),
                Location = new Point(116, 92),
                Size = new Size(320, 18),
                AutoSize = false
            });

            cmbPosicion = new ComboBox
            {
                Location = new Point(116, 112),
                Size = new Size(ancho - 128, 34),
                BackColor = Color.FromArgb(38, 38, 38),
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

            var btnAgregar = CrearBoton(
                "+ Agregar jugador",
                new Point(12, 160),
                new Size(ancho - 24, 38),
                Color.FromArgb(50, 80, 160));
            btnAgregar.Click += BtnAgregarJugador_Click;

            // ── Panel lista de jugadores (más alto) ───────
            var lblListaJug = new Label
            {
                Text = "Jugadores registrados en este equipo",
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Arial", 8),
                Location = new Point(12, 210),
                Size = new Size(ancho - 24, 16),
                AutoSize = false
            };

            panelJugadores = new Panel
            {
                Location = new Point(12, 228),
                Size = new Size(ancho - 24, 100),  // más alto
                AutoScroll = true,
                BackColor = Color.FromArgb(28, 28, 28),
                BorderStyle = BorderStyle.FixedSingle
            };

            var btnNuevoEquipo = CrearBoton(
                "+ Registrar otro equipo",
                new Point(12, 338),                       // debajo del panel
                new Size(ancho - 24, 25),
                Color.FromArgb(30, 30, 30));
            btnNuevoEquipo.Click += BtnNuevoEquipo_Click;

            cardJugador.Controls.Add(txtNombreJugador);
            cardJugador.Controls.Add(nudNumero);
            cardJugador.Controls.Add(cmbPosicion);
            cardJugador.Controls.Add(btnAgregar);
            cardJugador.Controls.Add(lblListaJug);
            cardJugador.Controls.Add(panelJugadores);
            cardJugador.Controls.Add(btnNuevoEquipo);
            this.Controls.Add(cardJugador);

            // =====================================================
            // CARD — EQUIPOS REGISTRADOS  (ahora con altura real)
            // =====================================================
            this.Controls.Add(new Label
            {
                Text = "Equipos registrados",
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(margen, 662),
                Size = new Size(ancho, 19),
                AutoSize = false
            });

            // Panel con altura generosa y borde visible
            panelEquipos = new Panel
            {
                Location = new Point(margen, 740),
                Size = new Size(ancho, 130),   // antes 180 pero invisible
                AutoScroll = true,
                BackColor = Color.FromArgb(24, 24, 24),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelEquipos);

            // =====================================================
            // BOTÓN FINALIZAR
            // =====================================================
            var btnFinalizar = CrearBoton(
                "✓  Finalizar registro y generar partidos",
                new Point(margen, 866),
                new Size(ancho, 50),
                Color.FromArgb(244, 123, 37));
            btnFinalizar.Font = new Font("Arial", 11, FontStyle.Bold);
            btnFinalizar.Click += BtnFinalizar_Click;
            this.Controls.Add(btnFinalizar);
        }

        // =====================================================
        // GUARDAR EQUIPO
        // =====================================================
        private void BtnGuardarEquipo_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreEquipo.Text.Trim();
            string origen = txtOrigen.Text.Trim();
            string categoria = txtCategoria.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                nombre == "Nombre del equipo" ||
                string.IsNullOrWhiteSpace(origen) ||
                origen == "Origen o ciudad" ||
                string.IsNullOrWhiteSpace(categoria) ||
                categoria == "Categoría")
            {
                MostrarAviso("Completa todos los campos del equipo.");
                return;
            }

            var torneo = torneoDao.ObtenerUltimo();
            equipoDao.Insertar(new Equipo
            {
                Nombre = nombre,
                Origen = origen,
                Categoria = categoria,
                IdTorneo = torneo.Id,
                Estado = "ACTIVO",
                Derrotas = 0
            });

            LimpiarCamposEquipo();
            CargarEquipos();
            CargarJugadoresUltimoEquipo();

            MessageBox.Show("Equipo guardado correctamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =====================================================
        // AGREGAR JUGADOR
        // =====================================================
        private void BtnAgregarJugador_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreJugador.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                nombre == "Nombre del jugador")
            {
                MostrarAviso("Escribe el nombre del jugador.");
                return;
            }

            var equipos = equipoDao.Listar();
            if (equipos.Count == 0)
            {
                MostrarAviso("Primero registra un equipo.");
                return;
            }

            var ultimo = equipos[equipos.Count - 1];
            int numero = (int)nudNumero.Value;

            if (jugadorDao.ExisteNumeroEnEquipo(numero, ultimo.Id))
            {
                MessageBox.Show(
                    "El número " + numero + " ya está en este equipo.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            jugadorDao.Insertar(new Jugador
            {
                Nombre = nombre,
                Numero = numero,
                Posicion = cmbPosicion.SelectedItem.ToString(),
                IdEquipo = ultimo.Id
            });

            txtNombreJugador.Text = "Nombre del jugador";
            txtNombreJugador.ForeColor = Color.FromArgb(120, 120, 120);
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
                MostrarAviso("Primero guarda un equipo.");
                return;
            }

            var ultimo = equipos[equipos.Count - 1];
            var jugadores = jugadorDao.ListarPorEquipo(ultimo.Id);

            if (jugadores.Count < 5)
            {
                MostrarAviso(
                    "El equipo \"" + ultimo.Nombre +
                    "\" necesita mínimo 5 jugadores antes de continuar.");
                return;
            }

            LimpiarCamposEquipo();
            panelJugadores.Controls.Clear();

            MessageBox.Show(
                "Puedes registrar el siguiente equipo.",
                "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =====================================================
        // FINALIZAR REGISTRO
        // =====================================================
        private void BtnFinalizar_Click(object sender, EventArgs e)
        {
            var equipos = equipoDao.Listar();

            if (equipos.Count < 2)
            {
                MostrarAviso("Necesitas al menos 2 equipos para generar partidos.");
                return;
            }

            foreach (var eq in equipos)
            {
                var jugs = jugadorDao.ListarPorEquipo(eq.Id);
                if (jugs.Count < 5)
                {
                    MostrarAviso(
                        "El equipo \"" + eq.Nombre +
                        "\" necesita mínimo 5 jugadores.");
                    return;
                }
            }

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

            var existentes = partidoDao.ListarPorTorneo(equipos[0].IdTorneo);
            if (existentes.Count > 0) return;

            // Mezclar aleatoriamente
            var rnd = new Random();
            for (int i = equipos.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                var tmp = equipos[i]; equipos[i] = equipos[j]; equipos[j] = tmp;
            }

            int idBye = 0;

            if (equipos.Count % 2 != 0)
            {
                int indiceBye = rnd.Next(equipos.Count);
                idBye = equipos[indiceBye].Id;

                partidoDao.Insertar(new Partido
                {
                    Nombre = equipos[indiceBye].Nombre + " — BYE",
                    Estado = "FINALIZADO",
                    PuntosA = 0,
                    PuntosB = 0,
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                    Bracket = "WINNERS",
                    Ronda = 1,
                    IdEquipoA = idBye,
                    IdEquipoB = 0,
                    IdTorneo = equipos[indiceBye].IdTorneo,
                    Ganador = idBye,
                    Perdedor = 0,
                    Bye = true
                });
            }

            for (int i = 0; i < equipos.Count; i++)
            {
                if (equipos[i].Id == idBye) continue;

                int j = i + 1;
                while (j < equipos.Count && equipos[j].Id == idBye) j++;
                if (j >= equipos.Count) break;

                partidoDao.Insertar(new Partido
                {
                    Nombre = equipos[i].Nombre + " VS " + equipos[j].Nombre,
                    Estado = "PENDIENTE",
                    PuntosA = 0,
                    PuntosB = 0,
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                    Bracket = "WINNERS",
                    Ronda = 1,
                    IdEquipoA = equipos[i].Id,
                    IdEquipoB = equipos[j].Id,
                    IdTorneo = equipos[i].IdTorneo,
                    Bye = false
                });

                i = j;
            }
        }

        // =====================================================
        // CARGAR EQUIPOS REGISTRADOS
        // =====================================================
        private void CargarEquipos()
        {
            panelEquipos.Controls.Clear();

            var equipos = equipoDao.Listar();
            int y = 4;

            if (equipos.Count == 0)
            {
                panelEquipos.Controls.Add(new Label
                {
                    Text = "Aún no hay equipos registrados.",
                    ForeColor = Color.FromArgb(120, 120, 120),
                    Font = new Font("Arial", 9),
                    Location = new Point(8, 8),
                    Size = new Size(420, 20),
                    AutoSize = false
                });
                return;
            }

            foreach (var eq in equipos)
            {
                var fila = new Panel
                {
                    Location = new Point(4, y),
                    Size = new Size(panelEquipos.Width - 24, 52),
                    BackColor = Color.FromArgb(32, 32, 32)
                };

                // Franja de color izquierda
                fila.Controls.Add(new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(4, 52),
                    BackColor = Color.FromArgb(244, 123, 37)
                });

                fila.Controls.Add(new Label
                {
                    Text = eq.Nombre,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Location = new Point(14, 6),
                    Size = new Size(270, 20),
                    AutoSize = false
                });

                fila.Controls.Add(new Label
                {
                    Text = eq.Origen + "  ·  " + eq.Categoria,
                    ForeColor = Color.FromArgb(150, 150, 150),
                    Font = new Font("Arial", 8),
                    Location = new Point(14, 28),
                    Size = new Size(270, 18),
                    AutoSize = false
                });

                var btnX = new Button
                {
                    Text = "✕",
                    Location = new Point(fila.Width - 40, 11),
                    Size = new Size(30, 30),
                    BackColor = Color.FromArgb(180, 40, 40),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = eq.Id
                };
                btnX.FlatAppearance.BorderSize = 0;
                btnX.Click += (s, ev) =>
                {
                    equipoDao.Eliminar((int)((Button)s).Tag);
                    CargarEquipos();
                };

                fila.Controls.Add(btnX);
                panelEquipos.Controls.Add(fila);
                y += 58;
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

            var ultimo = equipos[equipos.Count - 1];
            var jugadores = jugadorDao.ListarPorEquipo(ultimo.Id);
            int y = 2;

            foreach (var j in jugadores)
            {
                var fila = new Panel
                {
                    Location = new Point(2, y),
                    Size = new Size(panelJugadores.Width - 20, 28),
                    BackColor = Color.FromArgb(38, 38, 38)
                };

                fila.Controls.Add(new Label
                {
                    Text = "#" + j.Numero + "  " + j.Nombre +
                                "  ·  " + j.Posicion,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 9),
                    Location = new Point(6, 5),
                    Size = new Size(fila.Width - 40, 18),
                    AutoSize = false
                });

                var btnX = new Button
                {
                    Text = "✕",
                    Location = new Point(fila.Width - 28, 3),
                    Size = new Size(22, 22),
                    BackColor = Color.FromArgb(180, 40, 40),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 8, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = j.Id
                };
                btnX.FlatAppearance.BorderSize = 0;
                btnX.Click += (s, ev) =>
                {
                    var jugs = jugadorDao.ListarPorEquipo(ultimo.Id);
                    if (jugs.Count <= 5)
                    {
                        MessageBox.Show(
                            "El equipo necesita mínimo 5 jugadores.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    jugadorDao.Eliminar((int)((Button)s).Tag);
                    CargarJugadoresUltimoEquipo();
                };

                fila.Controls.Add(btnX);
                panelJugadores.Controls.Add(fila);
                y += 30;
            }
        }

        // =====================================================
        // HELPERS
        // =====================================================
        private void LimpiarCamposEquipo()
        {
            ResetTextBox(txtNombreEquipo, "Nombre del equipo");
            ResetTextBox(txtOrigen, "Origen o ciudad");
            ResetTextBox(txtCategoria, "Categoría");
        }

        private void ResetTextBox(TextBox txt, string placeholder)
        {
            txt.Text = placeholder;
            txt.ForeColor = Color.FromArgb(120, 120, 120);
        }

        private void MostrarAviso(string msg)
        {
            MessageBox.Show(msg, "Aviso",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private Label CrearSectionLabel(string texto, Point ubicacion)
        {
            return new Label
            {
                Text = texto,
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = ubicacion,
                Size = new Size(420, 22),
                AutoSize = false
            };
        }

        private Panel CrearCard(Point ubicacion, Size tamaño)
        {
            return new Panel
            {
                Location = ubicacion,
                Size = tamaño,
                BackColor = Color.FromArgb(26, 26, 26)
            };
        }

        private TextBox CrearTextBox(string placeholder, Point ubicacion)
        {
            var txt = new TextBox
            {
                Location = ubicacion,
                Size = new Size(424, 34),
                BackColor = Color.FromArgb(38, 38, 38),
                Font = new Font("Arial", 10),
                BorderStyle = BorderStyle.FixedSingle,
                Text = placeholder,
                ForeColor = Color.FromArgb(120, 120, 120)
            };

            txt.GotFocus += (s, e) => { if (txt.Text == placeholder) { txt.Text = ""; txt.ForeColor = Color.White; } };
            txt.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txt.Text)) { txt.Text = placeholder; txt.ForeColor = Color.FromArgb(120, 120, 120); } };

            return txt;
        }

        private Button CrearBoton(string texto, Point ubicacion, Size tamaño, Color color)
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
            int nWidthEllipse, int nHeightEllipse);
    }
}