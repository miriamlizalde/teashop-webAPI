using Microsoft.EntityFrameworkCore;
using TeaShop.Models;

namespace TeaShop.Data
{
    public class ProductoEFRepository : IProductoRepository
    {
        private readonly TeashopContext _context;

        public ProductoEFRepository(TeashopContext context)
        {
            _context = context;
        }

        public void AddProducto(Producto producto)
        {
            _context.Productos.Add(producto);
        }

        public IEnumerable<Producto> GetAllProductos(ProductoQueryParameters? queryParameters)
        {
            var query = _context.Productos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParameters?.Nombre))
                query = query.Where(p => p.Nombre.Contains(queryParameters.Nombre));

            if (!string.IsNullOrWhiteSpace(queryParameters?.Origen))
                query = query.Where(p => p.Origen.Contains(queryParameters.Origen));    
                
            if (!string.IsNullOrWhiteSpace(queryParameters?.Tipo))
            {
                if (queryParameters.Tipo == "Te")
                    query = query.OfType<Te>();
                else if (queryParameters.Tipo == "Comida")
                    query = query.OfType<Comida>();
            }

            if (queryParameters?.EsOrganico.HasValue == true)
                query = query.Where(p => p.EsOrganico == queryParameters.EsOrganico.Value);

            if (queryParameters?.PrecioMin.HasValue == true)
                query = query.Where(p => p.Precio >= queryParameters.PrecioMin.Value);

            if (queryParameters?.PrecioMax.HasValue == true)
                query = query.Where(p => p.Precio <= queryParameters.PrecioMax.Value);
            

            if (queryParameters?.OrdenarPor?.ToLower() == "precio")
                query = queryParameters.Descendente ? query.OrderByDescending(p => p.Precio) : query.OrderBy(p => p.Precio);
            else 
                query = queryParameters?.Descendente == true ? query.OrderByDescending(p => p.Nombre) : query.OrderBy(p => p.Nombre);
            
            return query.ToList();
        }

        public Producto GetProducto(int productoId)
        {
            return _context.Productos.Find(productoId);
        }

        public void UpdateProducto(Producto producto)
        {
            _context.Entry(producto).State = EntityState.Modified;
        }

        public void DeleteProducto(int productoId)
        {
            var producto = _context.Productos.Find(productoId);
            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {productoId} no encontrado.");
                
            _context.Productos.Remove(producto);
            SaveChanges();
            
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }    

}