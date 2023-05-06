namespace API.Models
{
    public class Respuesta
    {
        public int? codigo { get; set; }
        public string? mensaje { get; set; }
        public bool status { get; set; }
        public object? item { get; set; }
    }
}
