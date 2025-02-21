using CuestionarioInteractivo.Entities;
using CuestionarioInteractivo.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.JSInterop;
using static CuestionarioInteractivo.Components.Components.SolucionPregunta;

namespace CuestionarioInteractivo.Components.Pages;
public partial class Home : ComponentBase
{
    [Inject] private CuestionarioService CuestionarioService { get; set; }
    [Inject] private IJSRuntime JS { get; set; }

    private List<ElementoCuestionario> cuestionario;
    private bool primeraCarga = true;
    private const long tamañoMaxArchivo = 2 * 1024 * 1024; // 2MB de límite

    protected override void OnInitialized()
    {
        cuestionario = CuestionarioService.GetElementosCuestionario();
        primeraCarga = false;
    }

    private async Task DescargarArchivo()
    {
        string contenido = CuestionarioAString(cuestionario);
        string nombreArchivo = $"MisRespuestas{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(contenido);
        await JS.InvokeVoidAsync("downloadFile", nombreArchivo, Convert.ToBase64String(bytes));
    }

    private string CuestionarioAString(List<ElementoCuestionario> cuestionario)
    {
        string resultado = $"Mis respuestas del cuestionario{Environment.NewLine}Atención: Escribir únicamente en las secciones indicadas. Ignorar esta advertencia puede hacer que el archivo quede ilegible para la aplicación.{Environment.NewLine}{Environment.NewLine}";
        foreach (var elementoCuestionario in cuestionario)
        {
            resultado += $"Pregunta N°{elementoCuestionario.Id}: {elementoCuestionario.Pregunta}{Environment.NewLine}";
            
            resultado += $"Nota: Una o varias opciones pueden ser correctas. Marcarla/s con una X entre los paréntesis.{Environment.NewLine}";
            foreach (var opcion in elementoCuestionario.Opciones)
            {
                resultado += $"{elementoCuestionario.Id}.{opcion.LetraOpcion}. {opcion.Texto} ({(opcion.ElegidaPorUsuario ? "X" : "")}){Environment.NewLine}";
            }
                    
            resultado += Environment.NewLine; 
        }
        return resultado;
    }

    private async Task TxtAOpcionesElegidas(InputFileChangeEventArgs e)
    {
        var archivo = e.File;
        string extensionArchivo = Path.GetExtension(Path.GetFileName(e.File.Name)).ToLowerInvariant();
        string tipoMime = e.File.ContentType;
        
        if (archivo == null)
        {
            await JS.InvokeVoidAsync("alert", "No se seleccionó ningún archivo.");
            return;
        }
        if (extensionArchivo != ".txt" || tipoMime != "text/plain")
        {
            await JS.InvokeVoidAsync("alert", "El formato o tipo de archivo es incorrecto. Debe ser un archivo de texto y tener el formato .txt.");
            return;
        }
        if (archivo.Size > tamañoMaxArchivo)
        {
            await JS.InvokeVoidAsync("alert", "El archivo es demasiado grande. El tamaño máximo es de 2MB.");
            return;
        }

        try
        {
            List<string> lineas = new();

            using var stream = archivo.OpenReadStream(tamañoMaxArchivo);
            using var reader = new StreamReader(stream);
            string contenido = await reader.ReadToEndAsync();

            lineas.AddRange(contenido.Split(Environment.NewLine));
            var lineasOpcionesElegidas = lineas.Where(x => x.IndexOf("(X)", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            foreach (var lineaOpcionElegida in lineasOpcionesElegidas)
            {
                var preguntaOpcion = lineaOpcionElegida.Split('.', StringSplitOptions.RemoveEmptyEntries);
                if (preguntaOpcion.Count() < 2)
                    continue;

                bool resultadoParseo1 = int.TryParse(preguntaOpcion[0], out int idPregunta);
                bool resultadoParseo2 = char.TryParse(preguntaOpcion[1], out char letraOpcion);
                if (!resultadoParseo1 || !resultadoParseo2)
                    continue;

                var elementoCuestionario = cuestionario.FirstOrDefault(x => x.Id == idPregunta);
                if (elementoCuestionario == null)
                    continue;
                var opcion = elementoCuestionario.Opciones.FirstOrDefault(x => x.LetraOpcion == letraOpcion);
                if (opcion == null)
                    continue;
                opcion.ElegidaPorUsuario = true;
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {
            await JS.InvokeVoidAsync("alert", $"Error al leer el archivo: {ex.Message}");
        }
    }
}

