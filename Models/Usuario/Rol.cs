using API.Models.UtilidadGeneral;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Usuario
{
    public class RolDB
    {
        [Key]
        public int id_rol { get; set; }
        public string nombre { get; set; }
        public RolDB()
        { 
        }
    }
    public class Rol
    {
        public RolDB rolUsuario { get; set; }
        public List<PermisoAcceso> permisos { get; set; }
        public Rol()
        {
            this.rolUsuario = new RolDB();
            this.permisos = new List<PermisoAcceso>();
        }
    }
    [Keyless]
    public class PermisoAccesoDB
    {
        public int id_itemMenu { get; set; }
        public int id_rol { get; set; }
    }
    public class PermisoAcceso
    {
        public int id_itemMenu { get; set; }
        public int id_itemPadre { get; set; }
        public string nombre { get; set; }
        public string path { get; set; }
        public bool permiso { get; set; }
        public PermisoAcceso()
        {

        }
    }       
}
