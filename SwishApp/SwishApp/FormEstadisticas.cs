using System;
using System.Drawing;
using System.Windows.Forms;

namespace SwishApp
{
    public class FormEstadisticas : Form
    {
        public FormEstadisticas()
        {
            CrearInterfaz();
        }

        private void CrearInterfaz()
        {
            // ============================
            // CONFIGURACIÓN
            // ============================
            this.Text = "Estadísticas";
            this.BackColor = ColorTranslator.FromHtml("#121212");
            this.Size = new Size(600, 700);

            // ============================
            // HEADER
            // ============================
            Panel header = new Panel();
            header.Size = new Size(600, 80);
            header.BackColor = ColorTranslator.FromHtml("#121212");

            Label btnRegresar = new Label();
            btnRegresar.Text = "←";
            btnRegresar.ForeColor = Color.White;
            btnRegresar.Font = new Font("Arial", 14);
            btnRegresar.Location = new Point(10, 20);

            Label marcador = new Label();
            marcador.Text = "45 - 42";
            marcador.ForeColor = ColorTranslator.FromHtml("#f47b25");
            marcador.Font = new Font("Arial", 16, FontStyle.Bold);
            marcador.AutoSize = true;
            marcador.Location = new Point(250, 10);

            Label equipos = new Label();
            equipos.Text = "Equipo A vs Equipo B";
            equipos.ForeColor = Color.Gray;
            equipos.AutoSize = true;
            equipos.Location = new Point(230, 40);

            Label ronda = new Label();
            ronda.Text = "RONDA 3";
            ronda.ForeColor = ColorTranslator.FromHtml("#f47b25");
            ronda.Location = new Point(500, 30);

            header.Controls.Add(btnRegresar);
            header.Controls.Add(marcador);
            header.Controls.Add(equipos);
            header.Controls.Add(ronda);

            this.Controls.Add(header);

            // ============================
            // JUGADOR NORMAL
            // ============================
            Panel jugador1 = CrearJugador(
                "10",
                "Juan Pérez",
                "Ciudad de México",
                "12 PTS",
                "Faltas: 3",
                false
            );
            jugador1.Location = new Point(20, 100);
            this.Controls.Add(jugador1);

            // ============================
            // JUGADOR EN PELIGRO
            // ============================
            Panel jugador2 = CrearJugador(
                "05",
                "Carlos Ruiz",
                "Guadalajara",
                "8 PTS",
                "Faltas: 4",
                true
            );
            jugador2.Location = new Point(20, 250);
            this.Controls.Add(jugador2);

            // ============================
            // FOOTER
            // ============================
            Panel footer = new Panel();
            footer.Size = new Size(600, 70);
            footer.Location = new Point(0, 600);
            footer.BackColor = ColorTranslator.FromHtml("#121212");

            Button btnUndo = new Button();
            btnUndo.Text = "⟲";
            btnUndo.Size = new Size(50, 40);
            btnUndo.Location = new Point(20, 15);
            btnUndo.BackColor = ColorTranslator.FromHtml("#333");
            btnUndo.ForeColor = Color.White;

            Button btnFinalizar = new Button();
            btnFinalizar.Text = "FINALIZAR PARTIDO";
            btnFinalizar.Size = new Size(450, 40);
            btnFinalizar.Location = new Point(100, 15);
            btnFinalizar.BackColor = Color.Red;
            btnFinalizar.ForeColor = Color.White;

            footer.Controls.Add(btnUndo);
            footer.Controls.Add(btnFinalizar);

            this.Controls.Add(footer);
        }

        // ============================
        // MÉTODO PARA CREAR JUGADOR
        // ============================
        private Panel CrearJugador(string numero, string nombre, string ciudad, string puntos, string faltas, bool peligro)
        {
            Panel card = new Panel();
            card.Size = new Size(540, 130);
            card.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            card.BorderStyle = BorderStyle.FixedSingle;

            if (peligro)
            {
                card.BorderStyle = BorderStyle.Fixed3D;
            }

            // Número
            Label num = new Label();
            num.Text = numero;
            num.Size = new Size(40, 40);
            num.Location = new Point(10, 20);
            num.TextAlign = ContentAlignment.MiddleCenter;
            num.BackColor = peligro ? Color.FromArgb(100, 255, 0, 0) : Color.FromArgb(50, 244, 123, 37);
            num.ForeColor = peligro ? Color.Red : ColorTranslator.FromHtml("#f47b25");

            // Nombre
            Label lblNombre = new Label();
            lblNombre.Text = nombre;
            lblNombre.ForeColor = Color.White;
            lblNombre.Font = new Font("Arial", 10, FontStyle.Bold);
            lblNombre.Location = new Point(60, 20);

            // Ciudad
            Label lblCiudad = new Label();
            lblCiudad.Text = ciudad;
            lblCiudad.ForeColor = Color.Gray;
            lblCiudad.Location = new Point(60, 45);

            // Puntos
            Label lblPts = new Label();
            lblPts.Text = puntos;
            lblPts.ForeColor = ColorTranslator.FromHtml("#f47b25");
            lblPts.Location = new Point(420, 20);

            // Faltas
            Label lblFaltas = new Label();
            lblFaltas.Text = faltas;
            lblFaltas.ForeColor = peligro ? Color.Red : Color.Gray;
            lblFaltas.Location = new Point(420, 45);

            // BOTONES
            Button btn2 = CrearBoton("+2 pts", peligro);
            btn2.Location = new Point(20, 80);

            Button btn3 = CrearBoton("+3 pts", peligro);
            btn3.Location = new Point(180, 80);

            Button btnFalta = new Button();
            btnFalta.Text = "Falta";
            btnFalta.Size = new Size(140, 30);
            btnFalta.Location = new Point(340, 80);
            btnFalta.BackColor = peligro ? Color.Red : ColorTranslator.FromHtml("#f47b25");
            btnFalta.ForeColor = Color.White;

            card.Controls.Add(num);
            card.Controls.Add(lblNombre);
            card.Controls.Add(lblCiudad);
            card.Controls.Add(lblPts);
            card.Controls.Add(lblFaltas);
            card.Controls.Add(btn2);
            card.Controls.Add(btn3);
            card.Controls.Add(btnFalta);

            return card;
        }

        private Button CrearBoton(string texto, bool deshabilitado)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Size = new Size(140, 30);
            btn.BackColor = ColorTranslator.FromHtml("#333");
            btn.ForeColor = Color.White;

            if (deshabilitado)
            {
                btn.Enabled = false;
            }

            return btn;
        }
    }
}