using System;
using System.Drawing;
using System.Windows.Forms;

namespace SwishApp
{
    public class FormTorneo : Form
    {
        public FormTorneo()
        {
            CrearInterfaz();
        }

        private void CrearInterfaz()
        {
            // ============================
            // CONFIGURACIÓN
            // ============================
            this.Text = "Gráfica del Torneo";
            this.BackColor = ColorTranslator.FromHtml("#121212");
            this.Size = new Size(600, 700);
            this.AutoScroll = true;

            // ============================
            // HEADER
            // ============================
            Label titulo = new Label();
            titulo.Text = "Gráfica del Torneo";
            titulo.ForeColor = Color.White;
            titulo.Font = new Font("Arial", 14, FontStyle.Bold);
            titulo.AutoSize = true;
            titulo.Location = new Point(180, 20);

            Label subtitulo = new Label();
            subtitulo.Text = "Avance de partidos";
            subtitulo.ForeColor = Color.Gray;
            subtitulo.AutoSize = true;
            subtitulo.Location = new Point(210, 50);

            this.Controls.Add(titulo);
            this.Controls.Add(subtitulo);

            // ============================
            // PARTIDO 1 (PENDIENTE)
            // ============================
            Panel partido1 = CrearPartido(
                "Pendiente",
                "#aaa",
                "Lakers Ráfaga",
                "Bulls CDMX",
                "INICIAR PARTIDO",
                "#f47b25"
            );
            partido1.Location = new Point(30, 100);
            this.Controls.Add(partido1);

            // ============================
            // PARTIDO 2 (EN JUEGO)
            // ============================
            Panel partido2 = CrearPartido(
                "En juego",
                "#f47b25",
                "Warriors Norte",
                "Celtics Sur",
                "FINALIZAR PARTIDO",
                "red"
            );
            partido2.Location = new Point(30, 230);
            this.Controls.Add(partido2);

            // ============================
            // PARTIDO 3 (FINALIZADO)
            // ============================
            Panel partido3 = CrearPartido(
                "Finalizado",
                "#00ffa2",
                "Halcones",
                "Guerreros",
                "VER ESTADÍSTICAS",
                "green"
            );
            partido3.Location = new Point(30, 360);
            this.Controls.Add(partido3);

            // ============================
            // NAVBAR
            // ============================
            Panel nav = new Panel();
            nav.Size = new Size(600, 60);
            nav.Location = new Point(0, 600);
            nav.BackColor = ColorTranslator.FromHtml("#1c1c1c");

            Label nav1 = CrearNavItem("GRÁFICA", true);
            nav1.Location = new Point(60, 20);

            Label nav2 = CrearNavItem("ESTADÍSTICAS", false);
            nav2.Location = new Point(230, 20);

            Label nav3 = CrearNavItem("RANKING", false);
            nav3.Location = new Point(420, 20);

            nav.Controls.Add(nav1);
            nav.Controls.Add(nav2);
            nav.Controls.Add(nav3);

            this.Controls.Add(nav);
        }

        // ============================
        // MÉTODO PARA CREAR PARTIDOS
        // ============================
        private Panel CrearPartido(string estado, string colorEstado, string equipo1, string equipo2, string textoBtn, string colorBtn)
        {
            Panel card = new Panel();
            card.Size = new Size(520, 110);
            card.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            card.BorderStyle = BorderStyle.FixedSingle;

            // Estado
            Label lblEstado = new Label();
            lblEstado.Text = estado;
            lblEstado.ForeColor = ColorTranslator.FromHtml(colorEstado);
            lblEstado.Location = new Point(10, 10);

            // Equipos
            Label eq1 = new Label();
            eq1.Text = equipo1;
            eq1.ForeColor = Color.White;
            eq1.Location = new Point(20, 40);

            Label vs = new Label();
            vs.Text = "VS";
            vs.ForeColor = ColorTranslator.FromHtml("#f47b25");
            vs.Location = new Point(230, 40);

            Label eq2 = new Label();
            eq2.Text = equipo2;
            eq2.ForeColor = Color.White;
            eq2.Location = new Point(350, 40);

            // Botón
            Button btn = new Button();
            btn.Text = textoBtn;
            btn.Size = new Size(480, 25);
            btn.Location = new Point(20, 70);
            btn.BackColor = ColorTranslator.FromHtml(colorBtn);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;

            // Agregar todo
            card.Controls.Add(lblEstado);
            card.Controls.Add(eq1);
            card.Controls.Add(vs);
            card.Controls.Add(eq2);
            card.Controls.Add(btn);

            return card;
        }

        // ============================
        // NAV ITEM
        // ============================
        private Label CrearNavItem(string texto, bool activo)
        {
            Label lbl = new Label();
            lbl.Text = texto;
            lbl.AutoSize = true;

            if (activo)
            {
                lbl.ForeColor = ColorTranslator.FromHtml("#f47b25");
                lbl.Font = new Font("Arial", 9, FontStyle.Bold);
            }
            else
            {
                lbl.ForeColor = Color.Gray;
            }

            return lbl;
        }
    }
}