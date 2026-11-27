using System;
using System.Linq;
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
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            CargarDatosIniciales();
            dgvProductos.CellClick += DgvProductos_CellClick;
            btnActualizar.Click += BtnActualizar_Click;
            btnEliminar.Click += BtnEliminar_Click;
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
        // ACTULIZAR TABLA Y LIMPIAR CAMPOS
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

        private void CargarDatosIniciales()
        {
            _productos.Add(new Producto { Id = _siguienteId++, Nombre = "Laptop Lenovo", Categoria = "Electrónica", Precio = 35000, Stock = 5 });
            _productos.Add(new Producto { Id = _siguienteId++, Nombre = "Mouse Inalámbrico", Categoria = "Accesorios", Precio = 850, Stock = 20 });
            _productos.Add(new Producto { Id = _siguienteId++, Nombre = "Silla Ergonómica", Categoria = "Oficina", Precio = 7500, Stock = 8 });
            ActualizarTabla();
        }

        private void TxtBuscar_TextChanged(object? sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim();

            var filtrados = string.IsNullOrEmpty(filtro)
                ? _productos.ToList()
                : _productos.Where(p =>
                    (p.Nombre ?? "").Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                    (p.Categoria ?? "").Contains(filtro, StringComparison.OrdinalIgnoreCase))
                  .ToList();

            dgvProductos.DataSource = null;
            dgvProductos.DataSource = filtrados;
            dgvProductos.Refresh();
        }

        private void DgvProductos_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dgvProductos.Rows[e.RowIndex];
            if (fila.DataBoundItem is Producto producto)
            {
                _idSeleccionado = producto.Id;
                txtNombre.Text = producto.Nombre;
                cmbCategoria.SelectedItem = producto.Categoria;
                txtPrecio.Text = producto.Precio.ToString();
                txtStock.Text = producto.Stock.ToString();
            }
        }

        private void BtnActualizar_Click(object? sender, EventArgs e)
        {
            if (_idSeleccionado == -1)
            {
                MessageBox.Show("Selecciona un producto de la tabla para actualizar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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

            var producto = _productos.FirstOrDefault(p => p.Id == _idSeleccionado);
            if (producto != null)
            {
                producto.Nombre = txtNombre.Text.Trim();
                producto.Categoria = cmbCategoria.SelectedItem?.ToString() ?? "Otros";
                producto.Precio = precio;
                producto.Stock = stock;

                ActualizarTabla();
                LimpiarCampos();
                MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (_idSeleccionado == -1)
            {
                MessageBox.Show("Selecciona un producto de la tabla para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                "¿Estás seguro de que deseas eliminar este producto?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                var producto = _productos.FirstOrDefault(p => p.Id == _idSeleccionado);
                if (producto != null)
                {
                    _productos.Remove(producto);
                    ActualizarTabla();
                    LimpiarCampos();
                    MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

    }

}
