using System.ComponentModel.DataAnnotations;

namespace API.Models.Biblioteca
{
    public class EjemplarDB
    {
        [Key]
        public int id_ejemplar { get; set; }
        public int id_documento { get; set; }
        public bool disponibilidad { get; set; }
        public string codigoBarras { get; set; }
        public EjemplarDB () 
        {

        }
    }
}
