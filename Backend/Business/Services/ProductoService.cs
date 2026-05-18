using TeaShop.Data;
using TeaShop.Models;

namespace TeaShop.Business.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;
    private readonly IImageService _imageService;

    public ProductoService(IProductoRepository repository, IImageService imageService)
    {
        _repository = repository;
        _imageService = imageService;
    }

    public IEnumerable<Producto> GetAllProductos(ProductoQueryParameters? queryParameters)
    {
        return _repository.GetAllProductos(queryParameters);
    }

    public Producto GetProducto(int productoId)
    {
        var producto = _repository.GetProducto(productoId);
        if (producto == null) throw new KeyNotFoundException("Producto no encontrado");
        return producto;
    }

    public async Task AddProducto(CrearProductoDTO dto)
    {
        if (dto.Precio < 0) throw new ArgumentException("El precio no puede ser negativo");

        Producto producto;
        if (dto.Tipo == "Te")
            {
                producto = new Te
                {
                    TipoHoja = dto.TipoHoja ?? string.Empty
                };
            }
            else if (dto.Tipo == "Comida")
            {
                producto = new Comida
                {
                    TipoComida = dto.TipoComida ?? string.Empty,
                    Gluten     = dto.Gluten ?? false
                };
            }
            else
            {
                throw new ArgumentException("El tipo de producto debe ser 'Te' o 'Comida'.");
            }

            if (dto.Imagen != null && dto.Imagen.Length > 0)
            {producto.ImagenUrl = await _imageService.UploadImageAsync(dto.Imagen);}

            producto.Nombre = dto.Nombre;
            producto.Origen = dto.Origen;
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;
            producto.EsOrganico = dto.EsOrganico;
            producto.FechaCaducidad = dto.FechaCaducidad ?? DateTime.Now.AddMonths(1); 
        
        
        _repository.AddProducto(producto);
        _repository.SaveChanges();
    }

    public async Task UpdateProducto(int productoId, CrearProductoDTO dto)
    {
        var producto = _repository.GetProducto(productoId);
        if (producto == null) throw new KeyNotFoundException("Producto no encontrado");
        if (dto.Precio < 0) throw new ArgumentException("El precio no puede ser negativo");

        producto.Nombre = dto.Nombre;
        producto.Origen = dto.Origen;
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock;
        producto.EsOrganico = dto.EsOrganico;
        producto.FechaCaducidad = dto.FechaCaducidad ?? producto.FechaCaducidad;

        if (producto is Te te && dto.TipoHoja != null)
            te.TipoHoja = dto.TipoHoja;
        else if (producto is Comida comida)
        {
            if (dto.TipoComida != null) comida.TipoComida = dto.TipoComida;
            comida.Gluten = dto.Gluten ?? false;
        }
        if (dto.Imagen != null && dto.Imagen.Length > 0) {producto.ImagenUrl = await _imageService.UploadImageAsync(dto.Imagen);}
        _repository.UpdateProducto(producto);
        _repository.SaveChanges();
    }

    public void DeleteProducto(int productoId)
    {
        var producto = _repository.GetProducto(productoId);
        if (producto == null) throw new KeyNotFoundException("Producto no encontrado");

        _repository.DeleteProducto(productoId);
    }
}