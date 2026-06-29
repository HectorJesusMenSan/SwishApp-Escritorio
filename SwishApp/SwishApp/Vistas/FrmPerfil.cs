// =====================================================
// FrmPerfil.cs
// Pantalla de perfil: ver datos, cambiar username y contraseña
// =====================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;
using SwishApp.Configuracion;

namespace SwishApp.Vistas
{
    public class FrmPerfil : Form
    {
        private UsuarioDao usuarioDao = new UsuarioDao();

        // Campos - información
        private Label lblNombreValor;
        private Label lblUsernameValor;

        // Campos - edición username
        private TextBox txtNuevoUsername;

        // Campos - cambio de contraseña
        private TextBox txtPasswordActual;
        private TextBox txtPasswordNueva;
        private TextBox txtConfirmarNueva;

        // Referencia al formulario padre (para refrescar si hace falta)
        private Form _formPadre;

        public FrmPerfil(Form formPadre = null)
        {
            _formPadre = formPadre;
            ConfigurarFormulario();
        }

        // =====================================================
        // CONFIGURAR FORMULARIO
        // =====================================================
        private void ConfigurarFormulario()
        {
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.ForeColor = Color.White;
            this.Text = "Mi Perfil";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(420, 660);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            int x = 30;
            int yActual = 20;

            // ─── TÍTULO ───────────────────────────────────────────
            var lblTitulo = new Label
            {
                Text = "Mi Perfil",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 20, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, yActual),
                Size = new Size(420, 40),
                AutoSize = false
            };
            this.Controls.Add(lblTitulo);
            yActual += 55;

            // ─── CARD INFORMACIÓN ─────────────────────────────────
            var cardInfo = CrearCard(new Point(x, yActual), new Size(360, 120));

            var lblSecInfo = new Label
            {
                Text = "Información de cuenta",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(340, 20),
                AutoSize = false
            };

            var lblNombreTag = CrearLabel("Nombre completo", new Point(10, 38));
            lblNombreValor = new Label
            {
                Text = App.UsuarioActivo?.Nombre ?? "—",
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(10, 56),
                Size = new Size(340, 22),
                AutoSize = false
            };

            var lblUsernameTag = CrearLabel("Usuario", new Point(10, 82));
            lblUsernameValor = new Label
            {
                Text = "@" + (App.UsuarioActivo?.Username ?? "—"),
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 10),
                Location = new Point(10, 100),
                Size = new Size(340, 20),
                AutoSize = false
            };

            cardInfo.Controls.Add(lblSecInfo);
            cardInfo.Controls.Add(lblNombreTag);
            cardInfo.Controls.Add(lblNombreValor);
            cardInfo.Controls.Add(lblUsernameTag);
            cardInfo.Controls.Add(lblUsernameValor);
            this.Controls.Add(cardInfo);
            yActual += 130;

            // ─── CARD CAMBIAR USERNAME ────────────────────────────
            var cardUsername = CrearCard(new Point(x, yActual), new Size(360, 140));

            var lblSecUsername = new Label
            {
                Text = "Cambiar nombre de usuario",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(340, 20),
                AutoSize = false
            };

            var lblNuevoUser = CrearLabel("Nuevo usuario", new Point(10, 38));
            txtNuevoUsername = CrearCampo(new Point(10, 56), false);

            var btnCambiarUsername = CrearBoton(
                "Guardar usuario",
                new Point(10, 95),
                new Size(340, 38),
                Color.FromArgb(50, 100, 200)
            );
            btnCambiarUsername.Click += BtnCambiarUsername_Click;

            cardUsername.Controls.Add(lblSecUsername);
            cardUsername.Controls.Add(lblNuevoUser);
            cardUsername.Controls.Add(txtNuevoUsername);
            cardUsername.Controls.Add(btnCambiarUsername);
            this.Controls.Add(cardUsername);
            yActual += 155;

            // ─── CARD CAMBIAR CONTRASEÑA ──────────────────────────
            var cardPass = CrearCard(new Point(x, yActual), new Size(360, 230));

            var lblSecPass = new Label
            {
                Text = "Cambiar contraseña",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(340, 20),
                AutoSize = false
            };

            var lblActual = CrearLabel("Contraseña actual", new Point(10, 38));
            txtPasswordActual = CrearCampo(new Point(10, 56), true);

            var lblNueva = CrearLabel("Nueva contraseña", new Point(10, 95));
            txtPasswordNueva = CrearCampo(new Point(10, 113), true);

            var lblConfirmar = CrearLabel("Confirmar nueva contraseña", new Point(10, 152));
            txtConfirmarNueva = CrearCampo(new Point(10, 170), true);

            var btnCambiarPass = CrearBoton(
                "Cambiar contraseña",
                new Point(10, 185),
                new Size(340, 38),
                Color.FromArgb(244, 123, 37)
            );
            btnCambiarPass.Click += BtnCambiarPassword_Click;

            cardPass.Controls.Add(lblSecPass);
            cardPass.Controls.Add(lblActual);
            cardPass.Controls.Add(txtPasswordActual);
            cardPass.Controls.Add(lblNueva);
            cardPass.Controls.Add(txtPasswordNueva);
            cardPass.Controls.Add(lblConfirmar);
            cardPass.Controls.Add(txtConfirmarNueva);
            cardPass.Controls.Add(btnCambiarPass);
            this.Controls.Add(cardPass);
            yActual += 245;

            // ─── BOTÓN VOLVER ─────────────────────────────────────
            var btnVolver = CrearBoton(
                "← Volver",
                new Point(x, yActual),
                new Size(360, 38),
                Color.FromArgb(40, 40, 40)
            );
            btnVolver.Click += (s, e) => this.Close();
            this.Controls.Add(btnVolver);
        }

        // =====================================================
        // CAMBIAR USERNAME
        // =====================================================
        private void BtnCambiarUsername_Click(object sender, EventArgs e)
        {
            string nuevoUsername = txtNuevoUsername.Text.Trim();

            if (string.IsNullOrWhiteSpace(nuevoUsername))
            {
                MostrarError("Escribe el nuevo nombre de usuario.");
                return;
            }

            if (nuevoUsername == App.UsuarioActivo.Username)
            {
                MostrarError("El nuevo usuario es igual al actual.");
                return;
            }

            if (usuarioDao.ExisteUsername(nuevoUsername))
            {
                MostrarError("Ese nombre de usuario ya está en uso.");
                return;
            }

            bool ok = usuarioDao.ActualizarUsername(App.UsuarioActivo.Id, nuevoUsername);

            if (ok)
            {
                App.UsuarioActivo.Username = nuevoUsername;
                lblUsernameValor.Text = "@" + nuevoUsername;
                txtNuevoUsername.Text = "";

                MessageBox.Show(
                    "Nombre de usuario actualizado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MostrarError("No se pudo actualizar el usuario. Intenta de nuevo.");
            }
        }

        // =====================================================
        // CAMBIAR CONTRASEÑA
        // =====================================================
        private void BtnCambiarPassword_Click(object sender, EventArgs e)
        {
            string actual = txtPasswordActual.Text;
            string nueva = txtPasswordNueva.Text;
            string confirmar = txtConfirmarNueva.Text;

            if (string.IsNullOrWhiteSpace(actual) ||
                string.IsNullOrWhiteSpace(nueva) ||
                string.IsNullOrWhiteSpace(confirmar))
            {
                MostrarError("Completa todos los campos de contraseña.");
                return;
            }

            // Verificar contraseña actual contra la BD
            string hashAlmacenado = usuarioDao.ObtenerHash(App.UsuarioActivo.Id);

            if (!Contrasenia.VerificarPassword(actual, hashAlmacenado))
            {
                MostrarError("La contraseña actual es incorrecta.");
                return;
            }

            if (nueva != confirmar)
            {
                MostrarError("La nueva contraseña y su confirmación no coinciden.");
                return;
            }

            if (nueva.Length < 6)
            {
                MostrarError("La nueva contraseña debe tener mínimo 6 caracteres.");
                return;
            }

            bool ok = usuarioDao.ActualizarPassword(App.UsuarioActivo.Id, nueva);

            if (ok)
            {
                txtPasswordActual.Text = "";
                txtPasswordNueva.Text = "";
                txtConfirmarNueva.Text = "";

                MessageBox.Show(
                    "Contraseña actualizada correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MostrarError("No se pudo actualizar la contraseña. Intenta de nuevo.");
            }
        }

        // =====================================================
        // HELPERS UI
        // =====================================================
        private Panel CrearCard(Point ubicacion, Size tamaño)
        {
            return new Panel
            {
                Location = ubicacion,
                Size = tamaño,
                BackColor = Color.FromArgb(28, 28, 28),
                Padding = new Padding(10)
            };
        }

        private Label CrearLabel(string texto, Point ubicacion)
        {
            return new Label
            {
                Text = texto,
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 9),
                Location = ubicacion,
                Size = new Size(340, 18),
                AutoSize = false
            };
        }

        private TextBox CrearCampo(Point ubicacion, bool esPassword)
        {
            return new TextBox
            {
                Location = ubicacion,
                Size = new Size(340, 32),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                Font = new Font("Arial", 11),
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = esPassword ? '●' : '\0'
            };
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

        private void MostrarError(string mensaje)
        {
            MessageBox.Show(
                mensaje, "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPerfil));
            this.SuspendLayout();
            // 
            // FrmPerfil
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmPerfil";
            this.ResumeLayout(false);

        }
    }
}