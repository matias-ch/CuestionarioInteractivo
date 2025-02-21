namespace CuestionarioInteractivo.Entities;

public class ElementoCuestionario
{
    public int Id { get; set; }
    public string Pregunta { get; set; }
    public List<Opcion> Opciones { get; set; } = new List<Opcion>();
    public SolucionEn MostrarSolucionEn { get; set; } = SolucionEn.MismaVentana;

    public class Opcion
    {
        public char LetraOpcion { get; set; }
        public string Texto { get; set; }
        public bool EsCorrecta { get; set; }
        public bool ElegidaPorUsuario { get; set; } = false;
    }

    public enum SolucionEn
    {
        PopUp = 0,
        NuevaVentana = 1,
        MismaVentana = 2,
        Email = 3
    }
}