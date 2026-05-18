using TeaShop.Models;

namespace TeaShop.Data
{
    public interface IProductoRepository
    {
        void AddProducto(Producto producto);
        IEnumerable<Producto> GetAllProductos(ProductoQueryParameters? queryParameters = null);
        Producto GetProducto(int productoId);
        void UpdateProducto(Producto producto);
        void DeleteProducto(int productoId);
        void SaveChanges();
    }
}