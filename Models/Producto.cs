using System;
using System.Text.Json.Serialization;

namespace TeaShop.Models

{
    [JsonDerivedType(typeof(Te), typeDiscriminator:"Te")]
    [JsonDerivedType(typeof(Comida), typeDiscriminator:"Comida")]
    public abstract class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Origen { get; set; } 
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool EsOrganico { get; set; }
        public DateTime FechaCaducidad { get; set; }

        public Producto(int id, string nombre, string origen, decimal precio, int stock, bool organico, DateTime? fechaCaducidad = null)
        {
            if(precio < 0) {
            throw new ArgumentException("El precio no puede ser negativo");
            }
            if (String.IsNullOrWhiteSpace(nombre)) {
            throw new ArgumentException("El nombre no puede estar vacio");
            } 

            Id = id;
            Nombre = nombre;
            Origen = origen;
            Precio = precio;
            Stock = stock;
            EsOrganico = organico;
            FechaCaducidad = fechaCaducidad ?? DateTime.Now.AddMonths(6);
        }

        public abstract void MostrarDetalles();
    }

    public class Te : Producto
    {
        public string TipoHoja { get; set; } 

        public Te(int id, string nombre, string origen, decimal precio, int stock, bool organico, string tipoHoja, DateTime? fechaCaducidad = null)
            : base(id, nombre, origen, precio, stock, organico, fechaCaducidad)
        {
            TipoHoja = tipoHoja;
        }

        public override void MostrarDetalles()
        {
            string organicoStatus = EsOrganico ? "Sí" : "No";
            Console.WriteLine($"Té: {Nombre} | Origen: {Origen} | Tipo de Hoja: {TipoHoja} | Precio: ${Precio} por kg | Stock: {Stock}g | Orgánico: {organicoStatus} | Caducidad: {FechaCaducidad.ToShortDateString()}");
        }
    }    

    public class Comida : Producto
    {
        public string TipoComida { get; set; }
        public bool ContieneGluten { get; set; } 

        public Comida(int id, string nombre, string origen, decimal precio, int stock, bool organico, string tipoComida, bool gluten, DateTime? fechaCaducidad = null)
            : base(id, nombre, origen, precio, stock, organico, fechaCaducidad)
        {
            TipoComida = tipoComida;
            ContieneGluten = gluten;
        }

        public override void MostrarDetalles()
        {
            string organicoStatus = EsOrganico ? "Sí" : "No";
            string glutenStatus = ContieneGluten ? "Sí" : "No";
            Console.WriteLine($"Comida: {Nombre} | Origen: {Origen} | Tipo de Comida: {TipoComida} | Precio: ${Precio} por kg | Orgánico: {organicoStatus} | Contiene Gluten: {glutenStatus} | Stock: {Stock}g | Caducidad: {FechaCaducidad.ToShortDateString()}");
        }
    }
       
} 
