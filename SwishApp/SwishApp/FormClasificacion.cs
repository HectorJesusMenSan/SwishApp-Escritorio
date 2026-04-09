using System;
using System.Drawing;
using System.Windows.Forms;

namespace SwishApp
{
    public class FormClasificacion : Form
    {
        public FormClasificacion()
        {
            CrearInterfaz();
        }

        private void CrearInterfaz()
        {
            // ============================
            // CONFIGURACIÓN
            // ============================
            this.Text = "Clasificación";
            this.BackColor = ColorTranslator.FromHtml("#121212");
            this.Size = new Size(700, 700);

            // ============================
            // HEADER
            // ============================
            Panel header = new Panel();
            header.Size = new Size(700, 60);

            Label volver = new Label();
            volver.Text = "←";
            volver.ForeColor = Color.White;
            volver.Location = new Point(10, 20);

            Label titulo = new Label();
            titulo.Text = "Torneo Ráfaga";
            titulo.ForeColor = Color.White;
            titulo.Font = new Font("Arial", 14, FontStyle.Bold);
            titulo.AutoSize = true;
            titulo.Location = new Point(260, 20);

            Label config = new Label();
            config.Text = "⚙";
            config.ForeColor = Color.White;
            config.Location = new Point(650, 20);

            header.Controls.Add(volver);
            header.Controls.Add(titulo);
            header.Controls.Add(config);
            this.Controls.Add(header);

            // ============================
            // EQUIPO DESTACADO
            // ============================
            Panel destacado = new Panel();
            destacado.Size = new Size(640, 140);
            destacado.Location = new Point(20, 80);
            destacado.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            destacado.BorderStyle = BorderStyle.FixedSingle;

            Label lider = new Label();
            lider.Text = "LÍDER DEL TORNEO";
            lider.ForeColor = ColorTranslator.FromHtml("#f47b25");
            lider.Location = new Point(10, 10);

            Label nombre = new Label();
            nombre.Text = "Lakers Ráfaga";
            nombre.ForeColor = Color.White;
            nombre.Font = new Font("Arial", 12, FontStyle.Bold);
            nombre.Location = new Point(10, 30);

            Label trofeo = new Label();
            trofeo.Text = "🏆";
            trofeo.Location = new Point(580, 20);

            // Stats
            Label stats = new Label();
            stats.Text = "PJ:12   V/D:10-2   PTS:980   DIF:+120";
            stats.ForeColor = Color.White;
            stats.Location = new Point(10, 70);

            Button btnEquipo = new Button();
            btnEquipo.Text = "VER PERFIL DE EQUIPO";
            btnEquipo.Size = new Size(600, 30);
            btnEquipo.Location = new Point(10, 100);
            btnEquipo.BackColor = Color.Red;
            btnEquipo.ForeColor = Color.White;

            destacado.Controls.Add(lider);
            destacado.Controls.Add(nombre);
            destacado.Controls.Add(trofeo);
            destacado.Controls.Add(stats);
            destacado.Controls.Add(btnEquipo);

            this.Controls.Add(destacado);

            // ============================
            // TABLA
            // ============================
            DataGridView tabla = new DataGridView();
            tabla.Size = new Size(640, 200);
            tabla.Location = new Point(20, 240);
            tabla.BackgroundColor = ColorTranslator.FromHtml("#1c1c1c");
            tabla.ForeColor = Color.White;
            tabla.ColumnCount = 8;

            tabla.Columns[0].Name = "Pos";
            tabla.Columns[1].Name = "Equipo";
            tabla.Columns[2].Name = "PJ";
            tabla.Columns[3].Name = "V/D";
            tabla.Columns[4].Name = "Pts+";
            tabla.Columns[5].Name = "Pts-";
            tabla.Columns[6].Name = "Dif";
            tabla.Columns[7].Name = "Faltas";

            tabla.Rows.Add("1", "Lakers Ráfaga", "12", "10/2", "980", "860", "+120", "42");
            tabla.Rows.Add("2", "Chicago Bulls", "12", "9/3", "840", "720", "+120", "38");
            tabla.Rows.Add("3", "Boston Celtics", "12", "8/4", "910", "890", "+20", "55");
            tabla.Rows.Add("4", "Miami Heat", "12", "7/5", "780", "795", "-15", "49");

            this.Controls.Add(tabla);

            // ============================
            // CRITERIOS
            // ============================
            Panel criterios = new Panel();
            criterios.Size = new Size(640, 140);
            criterios.Location = new Point(20, 460);
            criterios.BackColor = ColorTranslator.FromHtml("#1c1c1c");

            Label tituloC = new Label();
            tituloC.Text = "Criterio de Evaluación";
            tituloC.ForeColor = Color.White;
            tituloC.Font = new Font("Arial", 10, FontStyle.Bold);
            tituloC.Location = new Point(10, 10);

            Label texto = new Label();
            texto.Text = "La clasificación se basa en resultados de partidos.";
            texto.ForeColor = Color.Gray;
            texto.Location = new Point(10, 30);

            Label c1 = new Label();
            c1.Text = "Victoria: 2 puntos";
            c1.ForeColor = Color.White;
            c1.Location = new Point(10, 60);

            Label c2 = new Label();
            c2.Text = "Derrota: 1 punto";
            c2.ForeColor = Color.White;
            c2.Location = new Point(10, 80);

            Label c3 = new Label();
            c3.Text = "No presentación: 0 puntos";
            c3.ForeColor = Color.Red;
            c3.Location = new Point(10, 100);

            criterios.Controls.Add(tituloC);
            criterios.Controls.Add(texto);
            criterios.Controls.Add(c1);
            criterios.Controls.Add(c2);
            criterios.Controls.Add(c3);

            this.Controls.Add(criterios);

            // ============================
            // NAVBAR
            // ============================
            Panel nav = new Panel();
            nav.Size = new Size(700, 60);
            nav.Location = new Point(0, 620);
            nav.BackColor = ColorTranslator.FromHtml("#1c1c1c");

            Label n1 = CrearNav("GRÁFICA", false);
            n1.Location = new Point(80, 20);

            Label n2 = CrearNav("ESTADÍSTICAS", false);
            n2.Location = new Point(280, 20);

            Label n3 = CrearNav("RANKING", true);
            n3.Location = new Point(500, 20);

            nav.Controls.Add(n1);
            nav.Controls.Add(n2);
            nav.Controls.Add(n3);

            this.Controls.Add(nav);
        }

        private Label CrearNav(string texto, bool activo)
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