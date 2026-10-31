using MySqlConnector;
using ProductosApi.Models;

namespace ProductosApi.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly string _cs;

    public ProductoRepository(IConfiguration config)
    {
        _cs = config.GetConnectionString("DefaultConnection")
              ?? throw new InvalidOperationException("Falta la cadena de conexión.");
    }

    // CONEXIÓN: una por operación
    private MySqlConnection CrearConexion() => new MySqlConnection(_cs);

    private static Producto Mapear(MySqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        Nombre = r.GetString(1),
        Descripcion = r.IsDBNull(2) ? null : r.GetString(2),
        Precio = r.GetDecimal(3),
        Stock = r.GetInt32(4)
    };

    // CONSULTAS
    public List<Producto> ObtenerTodos()
    {
        var lista = new List<Producto>();
        using var con = CrearConexion();
        using var cmd = new MySqlCommand(
            "SELECT Id, Nombre, Descripcion, Precio, Stock FROM Productos", con);
        con.Open();
        using var r = cmd.ExecuteReader();
        while (r.Read()) lista.Add(Mapear(r));
        return lista;
    }

    public Producto? ObtenerPorId(int id)
    {
        using var con = CrearConexion();
        using var cmd = new MySqlCommand(
            "SELECT Id, Nombre, Descripcion, Precio, Stock FROM Productos WHERE Id = @Id", con);
        cmd.Parameters.AddWithValue("@Id", id);
        con.Open();
        using var r = cmd.ExecuteReader();
        return r.Read() ? Mapear(r) : null;
    }

    // INSERT: devuelve el Id generado
    public int Insertar(Producto p)
    {
        using var con = CrearConexion();
        using var cmd = new MySqlCommand(@"
            INSERT INTO Productos (Nombre, Descripcion, Precio, Stock)
            VALUES (@Nombre, @Descripcion, @Precio, @Stock);
            SELECT LAST_INSERT_ID();", con);
        cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", (object?)p.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Precio", p.Precio);
        cmd.Parameters.AddWithValue("@Stock", p.Stock);
        con.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    // UPDATE
    public bool Actualizar(Producto p)
    {
        using var con = CrearConexion();
        using var cmd = new MySqlCommand(@"
            UPDATE Productos
            SET Nombre=@Nombre, Descripcion=@Descripcion, Precio=@Precio, Stock=@Stock
            WHERE Id=@Id", con);
        cmd.Parameters.AddWithValue("@Id", p.Id);
        cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", (object?)p.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Precio", p.Precio);
        cmd.Parameters.AddWithValue("@Stock", p.Stock);
        con.Open();
        return cmd.ExecuteNonQuery() > 0;
    }

    // DELETE
    public bool Eliminar(int id)
    {
        using var con = CrearConexion();
        using var cmd = new MySqlCommand("DELETE FROM Productos WHERE Id=@Id", con);
        cmd.Parameters.AddWithValue("@Id", id);
        con.Open();
        return cmd.ExecuteNonQuery() > 0;
    }

    // BUSCAR por nombre
    public List<Producto> Buscar(string texto)
    {
        var lista = new List<Producto>();
        using var con = CrearConexion();
        using var cmd = new MySqlCommand(@"
            SELECT Id, Nombre, Descripcion, Precio, Stock
            FROM Productos WHERE Nombre LIKE @Texto", con);
        cmd.Parameters.AddWithValue("@Texto", $"%{texto}%");
        con.Open();
        using var r = cmd.ExecuteReader();
        while (r.Read()) lista.Add(Mapear(r));
        return lista;
    }
}
