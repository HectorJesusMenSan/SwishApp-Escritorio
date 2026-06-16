using System;
using System.Drawing;
using System.Windows.Forms;
using SwishApp.Modelo;
using SwishApp.Modelo.Dao;

namespace SwishApp.Vistas
{
    public partial class FrmLogin : Form
    {
        private UsuarioDao usuarioDao = new UsuarioDao();

        private TextBox txtUsername;
        private TextBox txtPassword;

        public FrmLogin()
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
            this.Text = "Swish App - Login";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // =========================================
            // LOGO / TÍTULO
            // =========================================
            var lblLogo = new Label
            {
                Text = "🏀",
                Font = new Font("Arial", 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 40),
                Size = new Size(400, 60),
                AutoSize = false
            };

            var lblTitulo = new Label
            {
                Text = "Swish App",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 22, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 105),
                Size = new Size(400, 40),
                AutoSize = false
            };

            var lblSub = new Label
            {
                Text = "Gestión de Torneos",
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 148),
                Size = new Size(400, 25),
                AutoSize = false
            };

            // =========================================
            // CAMPOS
            // =========================================
            var lblUser = new Label
            {
                Text = "Usuario",
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 9),
                Location = new Point(30, 195),
                Size = new Size(340, 18),
                AutoSize = false
            };

            txtUsername = new TextBox
            {
                Location = new Point(30, 215),
                Size = new Size(340, 30),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                Font = new Font("Arial", 11),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblPass = new Label
            {
                Text = "Contraseña",
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Arial", 9),
                Location = new Point(30, 258),
                Size = new Size(340, 18),
                AutoSize = false
            };

            txtPassword = new TextBox
            {
                Location = new Point(30, 278),
                Size = new Size(340, 30),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                Font = new Font("Arial", 11),
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '●'
            };

            // =========================================
            // BOTÓN ENTRAR
            // =========================================
            var btnEntrar = new Button
            {
                Text = "Entrar",
                Location = new Point(30, 330),
                Size = new Size(340, 48),
                BackColor = Color.FromArgb(244, 123, 37),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnEntrar.FlatAppearance.BorderSize = 0;
            btnEntrar.Click += BtnEntrar_Click;

            // =========================================
            // LINK REGISTRO
            // =========================================
            var lblRegistro = new Label
            {
                Text = "¿No tienes cuenta? Regístrate",
                ForeColor = Color.FromArgb(244, 123, 37),
                Font = new Font("Arial", 9, FontStyle.Underline),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 395),
                Size = new Size(400, 25),
                AutoSize = false,
                Cursor = Cursors.Hand
            };

            lblRegistro.Click += (s, e) =>
            {
                var frmReg = new FrmRegistroUsuario();
                frmReg.Show();
                this.Hide();
            };

            // Agregar controles
            this.Controls.Add(lblLogo);
            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblSub);
            this.Controls.Add(lblUser);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnEntrar);
            this.Controls.Add(lblRegistro);
        }

        // =====================================================
        // BOTÓN ENTRAR
        // =====================================================
        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) ||
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

            Usuario usuario = usuarioDao.Login(username, password);

            if (usuario == null)
            {
                MessageBox.Show(
                    "Usuario o contraseña incorrectos.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // Guardar usuario en sesión
            App.UsuarioActivo = usuario;

            var frmInicio = new FrmInicio();
            frmInicio.Show();
            this.Close();
        }
    }
}