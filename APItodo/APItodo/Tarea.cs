namespace APItodo
{
    public class Tarea
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public bool Completado { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}
