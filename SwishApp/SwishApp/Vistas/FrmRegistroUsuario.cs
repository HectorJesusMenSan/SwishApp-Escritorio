using System;
using System.Drawing;
using System.Windows.Forms;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;

namespace SwishApp.Vistas
{
    public partial class FrmRegistroUsuario : Form
    {
        private UsuarioDao usuarioDao = new UsuarioDao();

        private TextBox txtNombre;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmar;

        public FrmRegistroUsuario()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        // =====================================================
        // CONFIGURAR FORMULARIO
        // =====================================================
        private void ConfigurarFormulario()
        {
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.ForeColor = Color.White;
            this.Text = "Registro de Usuario";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(520, 680);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Título
            var lblTitulo = new Label
            {
                Text = "Crear Cuenta",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 20, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(60, 35),
                Size = new Size(400, 40),
                AutoSize = false
            };

            // Campos
            txtNombre = CrearCampo("Nombre completo", new Point(80, 90), false);
            txtUsername = CrearCampo("Usuario", new Point(80, 160), false);
            txtPassword = CrearCampo("Contraseña", new Point(80, 230), true);
            txtConfirmar = CrearCampo("Confirmar contraseña", new Point(80, 300), true);

            var lblNombre = CrearLabel("Nombre completo", new Point(80, 72));
            var lblUsername = CrearLabel("Usuario", new Point(80, 142));
            var lblPassword = CrearLabel("Contraseña", new Point(80, 212));
            var lblConfirmar = CrearLabel("Confirmar contraseña", new Point(80, 282));

            // Botón registrar
            var btnRegistrar = new Button
            {
                Text = "Crear Cuenta",
                Location = new Point(60, 365),
                Size = new Size(400, 48),
                BackColor = Color.FromArgb(244, 123, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.Click += BtnRegistrar_Click;

            // Link volver
            var lblVolver = new Label
            {
                Text = "← Volver al login",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 9, FontStyle.Underline),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(60, 430),
                Size = new Size(400, 25),
                AutoSize = false,
                Cursor = Cursors.Hand
            };

            lblVolver.Click += (s, e) =>
            {
                var frmLogin = new FrmLogin();
                frmLogin.Show();
                this.Close();
            };

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblConfirmar);
            this.Controls.Add(txtConfirmar);
            this.Controls.Add(btnRegistrar);
            this.Controls.Add(lblVolver);
        }

        // =====================================================
        // REGISTRAR
        // =====================================================
        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmar = txtConfirmar.Text;

            // Validaciones
            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Completa todos los campos.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (password != confirmar)
            {
                MessageBox.Show(
                    "Las contraseñas no coinciden.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show(
                    "La contraseña debe tener mínimo 6 caracteres.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            var usuario = new Usuario
            {
                Nombre = nombre,
                Username = username,
                Password = password
            };

            bool ok = usuarioDao.Registrar(usuario);

            if (ok)
            {
                MessageBox.Show(
                    "Cuenta creada correctamente. Inicia sesión.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                var frmLogin = new FrmLogin();
                frmLogin.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(
                    "El usuario ya existe. Elige otro.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =====================================================
        // HELPERS
        // =====================================================
        private TextBox CrearCampo(
            string placeholder, Point ubicacion, bool esPassword)
        {
            var txt = new TextBox
            {
                Location = ubicacion,
                Size = new Size(340, 32),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                Font = new Font("Arial", 11),
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = esPassword ? '●' : '\0'
            };

            return txt;
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
    }
}