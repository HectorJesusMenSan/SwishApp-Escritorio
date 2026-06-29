// =====================================================
// NavBar.cs  —  helper estático
// Agrega los botones "Perfil" y "Cerrar sesión" a
// cualquier formulario principal de SwishApp.
//
// USO (dentro del constructor o ConfigurarFormulario
//      de cualquier FrmXxx):
//
//     NavBar.Agregar(this);
//
// =====================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using SwishApp.Vistas;

namespace SwishApp.Vistas
{
    public static class NavBar
    {
        // Altura reservada para la barra (suma al alto del form)
        public const int ALTURA = 46;

        // =====================================================
        // AGREGAR NAVBAR AL FORMULARIO
        // =====================================================
        public static void Agregar(Form form)
        {
            // Panel contenedor (franja superior)
            var panelNav = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = ALTURA,
                BackColor = Color.FromArgb(18, 18, 18)
            };

            // ── Botón PERFIL (izquierda) ─────────────────────
            var btnPerfil = new Button
            {
                Text      = "👤 Perfil",
                Location  = new Point(8, 8),
                Size      = new Size(100, 30),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Arial", 9, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnPerfil.FlatAppearance.BorderSize = 0;
            btnPerfil.Click += (s, e) =>
            {
                var frmPerfil = new FrmPerfil(form);
                frmPerfil.ShowDialog(form);   // modal sobre el form actual
            };

            // ── Botón CERRAR SESIÓN (derecha) ────────────────
            var btnCerrar = new Button
            {
                Text      = "Cerrar sesión",
                Size      = new Size(120, 30),
                BackColor = Color.FromArgb(180, 40, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Arial", 9, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnCerrar.FlatAppearance.BorderSize = 0;

            // Posicionar a la derecha; se recalcula si el form cambia de tamaño
            Action reposicionar = () =>
            {
                btnCerrar.Location = new Point(
                    panelNav.Width - btnCerrar.Width - 8, 8
                );
            };
            reposicionar();
            panelNav.Resize += (s, e) => reposicionar();

            btnCerrar.Click += (s, e) =>
            {
                var confirmacion = MessageBox.Show(
                    "¿Cerrar sesión?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmacion == DialogResult.Yes)
                {
                    // Limpiar sesión global
                    App.UsuarioActivo  = null;
                    App.IdTorneoActivo = 0;

                    // Abrir login y cerrar el formulario actual
                    var frmLogin = new FrmLogin();
                    frmLogin.Show();
                    form.Close();
                }
            };

            panelNav.Controls.Add(btnPerfil);
            panelNav.Controls.Add(btnCerrar);

            // Insertar el panel ANTES que el resto de controles
            // para que DockStyle.Top lo coloque en primer lugar
            form.Controls.Add(panelNav);
            panelNav.BringToFront();
        }
    }
}
