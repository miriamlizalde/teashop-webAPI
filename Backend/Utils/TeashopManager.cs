using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TeaShop.Models;

namespace TeaShop.Utils
{
    public static class TeashopManager
    {
        private static string DATOS = "Data";
        private static string ARCHIVO_LOG = Path.Combine(DATOS, "error_log.txt");

        static TeashopManager()
        {
            if (!Directory.Exists(DATOS))
            {
                Directory.CreateDirectory(DATOS);
            }
        }

        // LOG DE ERRORES
        public static void GuardarLog(string mensajeError)
        {
            try
            {
                string contenido = $"[{DateTime.Now}] ERROR: {mensajeError}\n";
                File.AppendAllText(ARCHIVO_LOG, contenido);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fallo al guardar log: " + ex.Message);
            }
        }

        // GUARDAR DATOS EN JSON
        public static void GuardarDatos<T>(List<T> datos, string nombreArchivo)
        {
            try
            {
                string ruta = Path.Combine(DATOS, nombreArchivo);
                
                var opciones = new JsonSerializerOptions { 
                    WriteIndented = true 
                };

                string jsonString = JsonSerializer.Serialize(datos, opciones);
                File.WriteAllText(ruta, jsonString);
                
                Console.WriteLine($"Datos guardados correctamente en {ruta}");
            }
            catch (Exception ex)
            {
                GuardarLog($"Error al guardar {nombreArchivo}: {ex.Message}");
            }
        }

        // CARGAR DATOS DE JSON 
        public static List<T> CargarDatos<T>(string nombreArchivo)
        {
            string ruta = Path.Combine(DATOS, nombreArchivo);
            
            try
            {
                if (!File.Exists(ruta))
                {
                    return new List<T>();
                }
                string jsonString = File.ReadAllText(ruta);
                return JsonSerializer.Deserialize<List<T>>(jsonString) ?? new List<T>();
            }
            catch (Exception ex)
            {
                GuardarLog($"Error al cargar {nombreArchivo}: {ex.Message}");
                return new List<T>();
            }
        }
    }
}