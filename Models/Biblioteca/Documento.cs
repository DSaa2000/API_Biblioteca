using System.ComponentModel.DataAnnotations;

namespace API.Models.Biblioteca
{
    public class DocumentoDB
    {
        [Key]
        public int id_documento { get; set; }
        public string nombre { get; set; }
        public string autor { get; set; }
        public int cantidad { get; set; }
        public string descripcion { get; set; }
        public string foto { get; set; }
        public string editorial { get; set; }
        public DateTime fechaPublicacion { get; set; }
        public string codigoUbicacion { get; set; }
        public DocumentoDB()
        {
            
        }

    }
}
