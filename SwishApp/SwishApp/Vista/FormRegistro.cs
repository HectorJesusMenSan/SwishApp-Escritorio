using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using SwishApp.Controlador;
using SwishApp.Dao;
using SwishApp.Vista;

namespace SwishApp
{
    public partial class FormRegistro : Form
    {
        DataGridView dgv;
        EquiposControlador controlador = new EquiposControlador();
        TextBox txtNombre, txtCategoria, txtOrigen;
        public FormRegistro()
        {   
            CrearInterfaz();
            
        }

        private void CrearInterfaz()
        {
            // ============================
            // CONFIGURACIÓN
            // ============================
            this.Text = "Registro de Equipos";
            this.BackColor = ColorTranslator.FromHtml("#121212");
            this.Size = new Size(600, 1000);
            this.AutoScroll = true;

            // ============================
            // HEADER
            // ============================
            Label titulo = new Label();
            titulo.Text = "Registro de Equipos";
            titulo.ForeColor = Color.White;
            titulo.Font = new Font("Arial", 16, FontStyle.Bold);
            titulo.AutoSize = true;
            titulo.Location = new Point(160, 20);

            this.Controls.Add(titulo);

            // ============================
            // CARD EQUIPO
            // ============================
            Panel cardEquipo = new Panel();
            cardEquipo.Size = new Size(520, 260);
            cardEquipo.Location = new Point(30, 70);
            cardEquipo.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            cardEquipo.BorderStyle = BorderStyle.FixedSingle;

            Label tituloEquipo = new Label();
            tituloEquipo.Text = "Datos del equipo";
            tituloEquipo.ForeColor = Color.White;
            tituloEquipo.Font = new Font("Arial", 10, FontStyle.Bold);
            tituloEquipo.Location = new Point(10, 10);

            // ================= NOMBRE =================
            Label lblNombre = new Label();
            lblNombre.Text = "Nombre del equipo";
            lblNombre.ForeColor = Color.White;
            lblNombre.Location = new Point(10, 40);

            txtNombre = new TextBox();
            txtNombre.Size = new Size(480, 25);
            txtNombre.Location = new Point(10, 60);
            txtNombre.BackColor = ColorTranslator.FromHtml("#2a2a2a");
            txtNombre.ForeColor = Color.White;

            // ================= CATEGORIA =================
            Label lblCategoria = new Label();
            lblCategoria.Text = "Categoría";
            lblCategoria.ForeColor = Color.White;
            lblCategoria.Location = new Point(10, 95);

            txtCategoria = new TextBox();
            txtCategoria.Size = new Size(480, 25);
            txtCategoria.Location = new Point(10, 115);
            txtCategoria.BackColor = ColorTranslator.FromHtml("#2a2a2a");
            txtCategoria.ForeColor = Color.White;

            // ================= ORIGEN =================
            Label lblOrigen = new Label();
            lblOrigen.Text = "Origen";
            lblOrigen.ForeColor = Color.White;
            lblOrigen.Location = new Point(10, 150);

            txtOrigen = new TextBox();
            txtOrigen.Size = new Size(480, 25);
            txtOrigen.Location = new Point(10, 170);
            txtOrigen.BackColor = ColorTranslator.FromHtml("#2a2a2a");
            txtOrigen.ForeColor = Color.White;

            // ================= BOTON =================
            Button btnEquipo = new Button();
            btnEquipo.Text = "Agregar equipo";
            btnEquipo.Size = new Size(480, 35);
            btnEquipo.Location = new Point(10, 210);
            btnEquipo.BackColor = ColorTranslator.FromHtml("#f47b25");
            btnEquipo.ForeColor = Color.White;
            btnEquipo.FlatStyle = FlatStyle.Flat;
            // EVENTO DEL BOTON
            btnEquipo.Click += BtnEquipo_Click;

            // AGREGAR CONTROLES AL PANEL
            cardEquipo.Controls.Add(tituloEquipo);
            cardEquipo.Controls.Add(lblNombre);
            cardEquipo.Controls.Add(txtNombre);

            cardEquipo.Controls.Add(lblCategoria);
            cardEquipo.Controls.Add(txtCategoria);

            cardEquipo.Controls.Add(lblOrigen);
            cardEquipo.Controls.Add(txtOrigen);

            cardEquipo.Controls.Add(btnEquipo);

            // AGREGAR PANEL AL FORMULARIO
            this.Controls.Add(cardEquipo);

            // ============================
            // CARD JUGADOR
            // ============================
            Panel cardJugador = new Panel();
            cardJugador.Size = new Size(520, 220);
            cardJugador.Location = new Point(30, 350);
            cardJugador.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            cardJugador.BorderStyle = BorderStyle.FixedSingle;

            Label tituloJugador = new Label();
            tituloJugador.Text = "Agregar jugador";
            tituloJugador.ForeColor = Color.White;
            tituloJugador.Font = new Font("Arial", 10, FontStyle.Bold);
            tituloJugador.Location = new Point(10, 10);

            Label lblNomJugador = new Label();
            lblNomJugador.Text = "Nombre";
            lblNomJugador.ForeColor = Color.White;
            lblNomJugador.Location = new Point(10, 40);

            TextBox txtNomJugador = new TextBox();
            txtNomJugador.Size = new Size(480, 25);
            txtNomJugador.Location = new Point(10, 60);
            txtNomJugador.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            txtNomJugador.ForeColor = Color.White;

            Label lblDorsal = new Label();
            lblDorsal.Text = "Dorsal";
            lblDorsal.ForeColor = Color.White;
            lblDorsal.Location = new Point(10, 100);

            TextBox txtDorsal = new TextBox();
            txtDorsal.Size = new Size(100, 25);
            txtDorsal.Location = new Point(10, 120);
            txtDorsal.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            txtDorsal.ForeColor = Color.White;

            Label lblPos = new Label();
            lblPos.Text = "Posición";
            lblPos.ForeColor = Color.White;
            lblPos.Location = new Point(130, 100);

            ComboBox combo = new ComboBox();
            combo.Size = new Size(360, 25);
            combo.Location = new Point(130, 120);

            combo.Items.Add("Base");
            combo.Items.Add("Escolta");
            combo.Items.Add("Alero");
            combo.Items.Add("Ala-Pívot");
            combo.Items.Add("Pívot");

            Button btnJugador = new Button();
            btnJugador.Text = "Agregar jugador";
            btnJugador.Size = new Size(480, 30);
            btnJugador.Location = new Point(10, 160);
            btnJugador.BackColor = ColorTranslator.FromHtml("#f47b25");
            btnJugador.ForeColor = Color.White;
            btnJugador.FlatStyle = FlatStyle.Flat;

            cardJugador.Controls.Add(tituloJugador);
            cardJugador.Controls.Add(lblNomJugador);
            cardJugador.Controls.Add(txtNomJugador);
            cardJugador.Controls.Add(lblDorsal);
            cardJugador.Controls.Add(txtDorsal);
            cardJugador.Controls.Add(lblPos);
            cardJugador.Controls.Add(combo);
            cardJugador.Controls.Add(btnJugador);

            this.Controls.Add(cardJugador);

            // ============================
            // LISTA JUGADORES
            // ============================
            Panel cardListaJug = new Panel();
            cardListaJug.Size = new Size(520, 130);
            cardListaJug.Location = new Point(30, 590);
            cardListaJug.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            cardListaJug.BorderStyle = BorderStyle.FixedSingle;

            Label tituloListaJug = new Label();
            tituloListaJug.Text = "Jugadores";
            tituloListaJug.ForeColor = Color.White;
            tituloListaJug.Font = new Font("Arial", 10, FontStyle.Bold);
            tituloListaJug.Location = new Point(10, 10);

            ListBox listaJug = new ListBox();
            listaJug.Size = new Size(480, 80);
            listaJug.Location = new Point(10, 40);
            listaJug.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            listaJug.ForeColor = Color.White;

            listaJug.Items.Add("Carlos Jiménez - #10");
            listaJug.Items.Add("Luis Ortega - #23");

            cardListaJug.Controls.Add(tituloListaJug);
            cardListaJug.Controls.Add(listaJug);

            this.Controls.Add(cardListaJug);

            // ============================
            // LISTA EQUIPOS
            // ============================
            Panel cardListaEq = new Panel();
            cardListaEq.Size = new Size(520, 130);
            cardListaEq.Location = new Point(30, 740);
            cardListaEq.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            cardListaEq.BorderStyle = BorderStyle.FixedSingle;

            Label tituloListaEq = new Label();
            tituloListaEq.Text = "Equipos registrados";
            tituloListaEq.ForeColor = Color.White;
            tituloListaEq.Font = new Font("Arial", 10, FontStyle.Bold);
            tituloListaEq.Location = new Point(10, 10);

            ListBox listaEq = new ListBox();
            listaEq.Size = new Size(480, 80);
            listaEq.Location = new Point(10, 40);
            listaEq.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            listaEq.ForeColor = Color.White;

            listaEq.Items.Add("Chicomesuchil - San Juan Chicomesuchil");
            listaEq.Items.Add("Chak´s - Ixtlan de Juarez");

            cardListaEq.Controls.Add(tituloListaEq);
            cardListaEq.Controls.Add(listaEq);

            this.Controls.Add(cardListaEq);

            // ============================
            // BOTONES
            // ============================
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Size = new Size(250, 40);
            btnCancelar.Location = new Point(30, 900);
            btnCancelar.BackColor = ColorTranslator.FromHtml("#1c1c1c");
            btnCancelar.ForeColor = ColorTranslator.FromHtml("#f47b25");
            btnCancelar.FlatStyle = FlatStyle.Flat;

            Button btnContinuar = new Button();
            btnContinuar.Text = "Continuar";
            btnContinuar.Size = new Size(250, 40);
            btnContinuar.Location = new Point(300, 900);
            btnContinuar.BackColor = ColorTranslator.FromHtml("#f47b25");
            btnContinuar.ForeColor = Color.White;
            btnContinuar.FlatStyle = FlatStyle.Flat;

            this.Controls.Add(btnCancelar);
            this.Controls.Add(btnContinuar);


            
        }
        private void BtnEquipo_Click(object sender, EventArgs e)
        {
            // Lógica para agregar el equipo a la lista
            controlador.RegistrarEquipo(txtNombre.Text, txtCategoria.Text, txtOrigen.Text, 1);
        }
    }
}