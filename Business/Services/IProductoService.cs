using TeaShop.Models;

namespace TeaShop.Business
{
    public interface IProductoService
    {
        IEnumerable<Producto> GetAllProductos(ProductoQueryParameters? queryParameters = null);
        Producto GetProducto(int productoId);
        
        void AddProducto(CrearProductoDTO productoDTO);
        void UpdateProducto(int productoId, CrearProductoDTO productoDTO);
        void DeleteProducto(int productoId);
    }
}