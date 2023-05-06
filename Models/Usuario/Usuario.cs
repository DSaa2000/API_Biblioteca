using System.ComponentModel.DataAnnotations;

namespace API.Models.Usuario
{
    public class UsuarioDB
    {
        [Key]
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string password { get; set; }
        public string correo { get; set; }
        public int id_rol { get; set; }
       
    }
    public class Usuario
    {
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string password { get; set; }
        public string correo { get; set; }
        public int id_rol { get; set; }
        public RolDB rol { get; set; }
    } 
    public class NuevoPerfil
    {
        public string user { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int id_rol { get; set; }
    } 

}
