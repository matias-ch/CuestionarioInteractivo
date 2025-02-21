using static CuestionarioInteractivo.Entities.ElementoCuestionario;

namespace CuestionarioInteractivo.Entities;
public static class Utilidades
{
    public static string EscribirTextoSolucion(ElementoCuestionario elementoCuestionario, bool usaHtml = true)
    {
        List<Opcion> listaRespuestas = elementoCuestionario.Opciones.Where(x => x.EsCorrecta).ToList();
        if (listaRespuestas.Count == 0)
            return $"Esta pregunta no tiene ninguna respuesta correcta";
        string resultado = string.Empty;
        if (listaRespuestas.Count == 1)
            resultado = $"La respuesta correcta para la pregunta {elementoCuestionario.Id} es:{(usaHtml ? "<br/>" : Environment.NewLine)}";
        else
            resultado = $"La respuestas correctas para la pregunta {elementoCuestionario.Id} son:{(usaHtml ? "<br/>" : Environment.NewLine)}";
        foreach (var respuesta in listaRespuestas)
        {
            resultado += $"{respuesta.LetraOpcion}. {respuesta.Texto}{(usaHtml ? "<br/>" : Environment.NewLine)}";
        }
        return resultado;
    }
}

