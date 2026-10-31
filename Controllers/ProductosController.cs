using Microsoft.AspNetCore.Mvc;
using ProductosApi.Models;
using ProductosApi.Repositories;

namespace ProductosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoRepository _repo;
    public ProductosController(IProductoRepository repo) => _repo = repo;

    [HttpGet]
    public IActionResult Get() => Ok(_repo.ObtenerTodos());

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var p = _repo.ObtenerPorId(id);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpGet("buscar")]
    public IActionResult Buscar([FromQuery] string texto) => Ok(_repo.Buscar(texto));

    [HttpPost]
    public IActionResult Post(Producto producto)
    {
        producto.Id = _repo.Insertar(producto);
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, Producto producto)
    {
        producto.Id = id;
        return _repo.Actualizar(producto) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) =>
        _repo.Eliminar(id) ? NoContent() : NotFound();
}
