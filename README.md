# API de Productos – ADO.NET + MySQL + Swagger

CRUD de productos en .NET 8 usando ADO.NET (`MySqlConnector`) con patrón Repository.

## Ejecución
1. Ejecutar `Database/script.sql` en MySQL Workbench.
2. Poner tu contraseña de root en `appsettings.json` (reemplaza TU_PASSWORD).
3. `dotnet restore` y `dotnet run`.
4. Abrir `https://localhost:7100/swagger`.

## Endpoints
| Método | Ruta | Acción |
|---|---|---|
| GET | /api/productos | Consulta todos |
| GET | /api/productos/{id} | Consulta por Id |
| GET | /api/productos/buscar?texto=lap | Buscar por nombre |
| POST | /api/productos | Insertar |
| PUT | /api/productos/{id} | Actualizar |
| DELETE | /api/productos/{id} | Eliminar |

Flujo
Desde Swagger se envía una petición HTTP a la API, por ejemplo un POST /api/productos para agregar un producto.
El controlador recibe la petición y llama al repositorio a través de la interfaz IProductoRepository. Aquí no se escribe nada de SQL, solo se decide qué método usar.
En Program.cs se configuró la inyección de dependencias, que es lo que le entrega al controlador la clase ProductoRepository cuando pide la interfaz.
El repositorio es el que trabaja con la base de datos usando ADO.NET. En cada operación abre una conexión con MySqlConnection, usando la cadena que está en appsettings.json. Después arma el comando con MySqlCommand y le pasa los valores como parámetros, para evitar SQL Injection. Según lo que haga, ejecuta el comando con ExecuteReader (consultas y búsqueda), ExecuteScalar (insertar, para obtener el Id nuevo) o ExecuteNonQuery (actualizar y eliminar). Al terminar, la conexión se cierra sola gracias al using.
MySQL ejecuta la instrucción sobre la tabla Productos y regresa el resultado.
El repositorio le devuelve al controlador los datos, un true o false, o el Id del nuevo producto. El controlador responde con un código HTTP (200, 201, 204 o 404) y Swagger muestra el resultado en pantalla.
