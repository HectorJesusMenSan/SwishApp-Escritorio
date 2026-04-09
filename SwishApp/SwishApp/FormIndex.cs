using System;
using System.Drawing;
using System.Windows.Forms;

namespace SwishApp
{
    public class FormIndex : Form
    {
        public FormIndex()
        {
            CrearInterfaz();
        }

        private void CrearInterfaz()
        {
            // ============================
            // CONFIGURACIÓN DEL FORM
            // ============================
            this.Text = "Swish App - Torneo";
            this.BackColor = ColorTranslator.FromHtml("#121212");
            this.Size = new Size(600, 550);

            // ============================
            // HEADER
            // ============================
            Label titulo = new Label();
            titulo.Text = "Gestión de Torneos de Básquetbol";
            titulo.ForeColor = Color.White;
            titulo.Font = new Font("Arial", 14, FontStyle.Bold);
            titulo.AutoSize = true;
            titulo.Location = new Point(60, 30);

            Label subtitulo = new Label();
            subtitulo.Text = "Selecciona el tipo de torneo";
            subtitulo.ForeColor = Color.Gray;
            subtitulo.AutoSize = true;
            subtitulo.Location = new Point(150, 60);

            this.Controls.Add(titulo);
            this.Controls.Add(subtitulo);

            // ============================
            // TARJETA RÁFAGA
            // ============================
            Panel cardRafaga = new Panel();
            cardRafaga.Size = new Size(240, 130);
            cardRafaga.Location = new Point(30, 120);
            cardRafaga.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            cardRafaga.BorderStyle = BorderStyle.FixedSingle;

            Label tituloRafaga = new Label();
            tituloRafaga.Text = "Torneo Ráfaga";
            tituloRafaga.ForeColor = Color.White;
            tituloRafaga.Font = new Font("Arial", 10, FontStyle.Bold);
            tituloRafaga.Location = new Point(10, 10);

            Label textoRafaga = new Label();
            textoRafaga.Text = "Doble eliminación, partidos rápidos.";
            textoRafaga.ForeColor = Color.Gray;
            textoRafaga.Size = new Size(200, 50);
            textoRafaga.Location = new Point(10, 40);

            RadioButton radioRafaga = new RadioButton();
            radioRafaga.Location = new Point(200, 10);

            cardRafaga.Controls.Add(tituloRafaga);
            cardRafaga.Controls.Add(textoRafaga);
            cardRafaga.Controls.Add(radioRafaga);
            this.Controls.Add(cardRafaga);

            // ============================
            // TARJETA LIGA
            // ============================
            Panel cardLiga = new Panel();
            cardLiga.Size = new Size(240, 130);
            cardLiga.Location = new Point(300, 120);
            cardLiga.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            cardLiga.BorderStyle = BorderStyle.FixedSingle;

            Label tituloLiga = new Label();
            tituloLiga.Text = "Torneo de Liga";
            tituloLiga.ForeColor = Color.White;
            tituloLiga.Font = new Font("Arial", 10, FontStyle.Bold);
            tituloLiga.Location = new Point(10, 10);

            Label textoLiga = new Label();
            textoLiga.Text = "Todos contra todos, tabla de posiciones.";
            textoLiga.ForeColor = Color.Gray;
            textoLiga.Size = new Size(200, 50);
            textoLiga.Location = new Point(10, 40);

            RadioButton radioLiga = new RadioButton();
            radioLiga.Location = new Point(200, 10);

            cardLiga.Controls.Add(tituloLiga);
            cardLiga.Controls.Add(textoLiga);
            cardLiga.Controls.Add(radioLiga);
            this.Controls.Add(cardLiga);

            // ============================
            // INFO
            // ============================
            Panel infoRafaga = new Panel();
            infoRafaga.Size = new Size(240, 70);
            infoRafaga.Location = new Point(30, 270);
            infoRafaga.BackColor = ColorTranslator.FromHtml("#1c1c1c");

            Label lblInfoR = new Label();
            lblInfoR.Text = "Modo Ráfaga\nIdeal para torneos rápidos";
            lblInfoR.ForeColor = Color.White;
            lblInfoR.Location = new Point(10, 10);

            infoRafaga.Controls.Add(lblInfoR);
            this.Controls.Add(infoRafaga);

            Panel infoLiga = new Panel();
            infoLiga.Size = new Size(240, 70);
            infoLiga.Location = new Point(300, 270);
            infoLiga.BackColor = ColorTranslator.FromHtml("#1c1c1c");

            Label lblInfoL = new Label();
            lblInfoL.Text = "Modo Liga\nPara ligas largas";
            lblInfoL.ForeColor = Color.White;
            lblInfoL.Location = new Point(10, 10);

            infoLiga.Controls.Add(lblInfoL);
            this.Controls.Add(infoLiga);

            // ============================
            // BOTÓN
            // ============================
            Button btn = new Button();
            // Evento cuando se da clic al botón
            btn.Click += (s, e) =>
            {
                FormRegistro ventana = new FormRegistro();
                ventana.Show();
                this.Hide(); // cierra completamente
            };
            btn.Text = "Continuar";
            btn.Size = new Size(500, 40);
            btn.Location = new Point(40, 380);
            btn.BackColor = ColorTranslator.FromHtml("#f47b25");
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;

            this.Controls.Add(btn);
        }
    }
}