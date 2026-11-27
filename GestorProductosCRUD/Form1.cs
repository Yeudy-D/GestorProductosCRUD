using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GestorProductosCRUD
{
    public partial class Form1 : Form
    {
        private readonly List<Producto> _productos = new();
        private int _siguienteId = 1;
        private int _idSeleccionado = -1;

        private TextBox txtNombre = null!;
        private ComboBox cmbCategoria = null!;
        private TextBox txtPrecio = null!;
        private TextBox txtStock = null!;
        private TextBox txtBuscar = null!;
        private DataGridView dgvProductos = null!;
        private Button btnAgregar = null!;
        private Button btnActualizar = null!;
        private Button btnEliminar = null!;
        private Button btnLimpiar = null!;

        public Form1()
        {
            InitializeComponent();
            ConfigurarInterfaz();
            btnAgregar.Click += BtnAgregar_Click;
            btnLimpiar.Click += (_, _) => LimpiarCampos();
        }

        private void ConfigurarInterfaz()
        {
            Text = "Gestor de Productos - CRUD (Git Flow)";
            Size = new Size(900, 560);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(240, 244, 248);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

           
            Panel panelHeader = new()
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(15, 32, 65)
            };
            Label lblTitulo = new()
            {
                Text = "GESTOR DE INVENTARIO DE PRODUCTOS",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 16)
            };
            panelHeader.Controls.Add(lblTitulo);
            Controls.Add(panelHeader);

            Panel panelForm = new()
            {
                Location = new Point(20, 80),
                Size = new Size(280, 420),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            int y = 20;
            panelForm.Controls.Add(CrearEtiqueta("Nombre del Producto:", 15, y));
            txtNombre = new TextBox { Location = new Point(15, y + 22), Width = 245, Font = new Font("Segoe UI", 10) };
            panelForm.Controls.Add(txtNombre);

            y += 65;
            panelForm.Controls.Add(CrearEtiqueta("Categoría:", 15, y));
            cmbCategoria = new ComboBox
            {
                Location = new Point(15, y + 22),
                Width = 245,
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategoria.Items.AddRange(new object[] { "Electrónica", "Accesorios", "Oficina", "Hogar", "Otros" });
            cmbCategoria.SelectedIndex = 0;
            panelForm.Controls.Add(cmbCategoria);

            y += 65;
            panelForm.Controls.Add(CrearEtiqueta("Precio (RD$):", 15, y));
            txtPrecio = new TextBox { Location = new Point(15, y + 22), Width = 245, Font = new Font("Segoe UI", 10) };
            panelForm.Controls.Add(txtPrecio);

            y += 65;
            panelForm.Controls.Add(CrearEtiqueta("Stock disponible:", 15, y));
            txtStock = new TextBox { Location = new Point(15, y + 22), Width = 245, Font = new Font("Segoe UI", 10) };
            panelForm.Controls.Add(txtStock);

            y += 70;
            btnAgregar = CrearBoton("Agregar", 15, y, Color.FromArgb(20, 83, 136));
            btnActualizar = CrearBoton("Actualizar", 142, y, Color.FromArgb(41, 128, 185));
            btnEliminar = CrearBoton("Eliminar", 15, y + 45, Color.FromArgb(192, 57, 43));
            btnLimpiar = CrearBoton("Limpiar", 142, y + 45, Color.FromArgb(127, 140, 141));

            panelForm.Controls.AddRange(new Control[] { btnAgregar, btnActualizar, btnEliminar, btnLimpiar });
            Controls.Add(panelForm);

            Controls.Add(CrearEtiqueta("Buscar producto:", 320, 85));
            txtBuscar = new TextBox
            {
                Location = new Point(470, 82),
                Width = 390,
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Escribe el nombre o categoría..."
            };
            Controls.Add(txtBuscar);

          
            dgvProductos = new DataGridView
            {
                Location = new Point(320, 120),
                Size = new Size(540, 380),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };
            Controls.Add(dgvProductos);
        }

        private static Label CrearEtiqueta(string texto, int x, int y) => new()
        {
            Text = texto,
            Location = new Point(x, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 41, 59)
        };

        private static Button CrearBoton(string texto, int x, int y, Color color) => new()
        {
            Text = texto,
            Location = new Point(x, y),
            Size = new Size(118, 36),
            BackColor = color,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del producto es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio < 0)
            {
                MessageBox.Show("Ingresa un precio válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Ingresa una cantidad de stock válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _productos.Add(new Producto
            {
                Id = _siguienteId++,
                Nombre = txtNombre.Text.Trim(),
                Categoria = cmbCategoria.SelectedItem?.ToString() ?? "Otros",
                Precio = precio,
                Stock = stock
            });

            ActualizarTabla();
            LimpiarCampos();
        }

        private void ActualizarTabla()
        {
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = new List<Producto>(_productos);
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            cmbCategoria.SelectedIndex = 0;
            txtPrecio.Clear();
            txtStock.Clear();
            _idSeleccionado = -1;
            txtNombre.Focus();
        }
    }
}
