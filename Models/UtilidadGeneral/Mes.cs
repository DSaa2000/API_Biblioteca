using System.ComponentModel.DataAnnotations;

namespace API.Models.UtilidadGeneral
{
    public class MesDB
    {
        [Key]
        public long id_mes { get; set; }
        public string nombre { get; set; }
    }
}
