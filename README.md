# GestorProductosCRUD

Aplicación de escritorio para la gestión de inventario de productos (CRUD), desarrollada en C# con Windows Forms como parte de la práctica de **Git y Git Flow** para la asignatura de **Programación III** en el ITLA.

## Funcionalidades

- **Registro de productos:** Permite agregar nuevos productos indicando nombre, categoría, precio y stock disponible, con validaciones de campos vacíos y valores numéricos.
- **Listado y búsqueda en tiempo real:** Carga datos iniciales de prueba y filtra los productos al instante por nombre o categoría conforme se escribe en el buscador.
- **Edición:** Al seleccionar una fila de la tabla, los datos se cargan en el formulario para modificarlos y actualizar el registro.
- **Eliminación:** Permite borrar un producto seleccionado previa confirmación del usuario.
- **Limpieza de formulario:** Restablece todos los campos y quita la selección actual.

## Tecnologías utilizadas

- C# (.NET / Windows Forms)
- Visual Studio
- Git y GitHub

## Flujo de trabajo (Git Flow)

El repositorio está organizado con las ramas base `main`, `dev` y `qa`. Cada funcionalidad se desarrolló en su propia rama `feature/` partiendo desde `dev` y se integró mediante Pull Requests hacia `dev`, `qa` y `main` (15 Pull Requests cerrados en total):

1. `feature/modelo-y-interfaz` – Creación de la clase `Producto` y diseño base de la interfaz.
2. `feature/crear-producto` – Lógica para agregar productos, limpiar campos y validar entradas.
3. `feature/listar-y-buscar` – Carga inicial de datos y filtrado en tiempo real.
4. `feature/editar-producto` – Selección desde el `DataGridView` y actualización de registros.
5. `feature/eliminar-producto` – Eliminación de productos con cuadro de confirmación.

## Cómo ejecutar el proyecto

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/Yeudy-D/GestorProductosCRUD.git
