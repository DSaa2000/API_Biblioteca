using System.ComponentModel.DataAnnotations;

namespace API.Models.UtilidadGeneral
{
    public class ItemMenuDB
    {
        [Key]
        public int id_itemMenu { get; set; }
        public int id_itemPadre { get; set; }
        public string titulo { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
        public int prioridad { get; set; }
    }
    public class ItemMenu
    {
        public int id_itemMenu { get; set; }
        public int id_itemPadre { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
        public List<ItemMenu> children { get; set;}
        public int prioridad { get; set; }
        public bool permiso { get; set; }
    }
    
}
