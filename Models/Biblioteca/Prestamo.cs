using System.ComponentModel.DataAnnotations;

namespace API.Models.Biblioteca
{
    public class PrestamoDB
    {
        [Key]
        public int id_prestamo { get; set; }    
        public int id_ejemplar { get; set; }
        public int id_usuario { get; set; }
        public int plazo { get; set; }
        public DateTime fechaPrestamo { get; set; }
        public DateTime fechaLimite { get; set; }
        public string estado { get; set; }
    }
}
