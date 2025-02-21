using System.Text.Json;
using CuestionarioInteractivo.Entities;
using static CuestionarioInteractivo.Entities.ElementoCuestionario;

namespace CuestionarioInteractivo.Services;
public class CuestionarioService
{
    private List<ElementoCuestionario> _elementosCuestionario { get; set; }
    public void CargarCuestionarioDesdeJson()
    {
        string rutaJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cuestionario.json");
        _elementosCuestionario = DeserializarCuestionario(rutaJson);
    }

    private List<ElementoCuestionario> DeserializarCuestionario(string rutaArchivo)
    {
        if (!File.Exists(rutaArchivo))
        {
            Console.WriteLine("El archivo JSON no existe.");
            return new List<ElementoCuestionario>();
        }
        try
        {
            string json = File.ReadAllText(rutaArchivo);
            var opcionesJson = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            List<ElementoCuestionario> lista = JsonSerializer.Deserialize<List<ElementoCuestionario>>(json, opcionesJson) ?? new List<ElementoCuestionario>();

            int id = 1;
            foreach (var elemento in lista)
            {
                elemento.Id = id++;
                char letra = 'A';
                foreach (var opcion in elemento.Opciones)
                {
                    opcion.LetraOpcion = letra++;
                }
            }
            Console.WriteLine($"Cuestionario cargado exitosamente");
            return lista;
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Error cargando el cuestionario: {ex.Message}");
            return new List<ElementoCuestionario>();
        }
    }
    public List<ElementoCuestionario> GetElementosCuestionario()
    { 
        return _elementosCuestionario;
    }

    public ElementoCuestionario? GetElementoCuestionarioPorId(int id)
    {
        return _elementosCuestionario.FirstOrDefault(x => x.Id == id);
    }
}

