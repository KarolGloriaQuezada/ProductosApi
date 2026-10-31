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

## Flujo
1. **Cliente (Swagger)** envía una petición HTTP, por ejemplo `POST /api/productos`.
2. **Controller** recibe la petición, valida el modelo y llama a `IProductoRepository`. No contiene SQL.
3. **Inyección de dependencias** (`Program.cs`) entrega `ProductoRepository` como implementación de la interfaz.
4. **Repositorio (ADO.NET)**, en cada operación:
   - Abre un `MySqlConnection` con la cadena de `appsettings.json`.
   - Crea un `MySqlCommand` con parámetros (evita SQL Injection).
   - Ejecuta `ExecuteReader` (consultas y búsqueda), `ExecuteScalar` (insert, devuelve el Id) o `ExecuteNonQuery` (update y delete).
   - Cierra la conexión automáticamente con `using`.
5. **MySQL** procesa la sentencia sobre la tabla `Productos`.
6. **Respuesta**: el repositorio devuelve datos, un `bool` o el Id al controller, que responde `200`, `201`, `204` o `404`. Swagger muestra el resultado.

## Evidencias
Capturas de Swagger en la carpeta `evidencias/`.
