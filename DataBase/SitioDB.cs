using Microsoft.EntityFrameworkCore;
using API.Models;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using API.Models.UtilidadGeneral;
using API.Models.Usuario;
using API.Models.Biblioteca;

namespace API.DataBase
{
    public class SitioDB : DbContext
    {
        public SitioDB(DbContextOptions<SitioDB> options) : base(options)
        {

        }
        /* Utilidad */
        public DbSet<MesDB> Meses { get; set; }
        /* Usuarios */
        public DbSet<RolDB> RolesUsuarios { get; set; }
        public DbSet<UsuarioDB> Usuarios { get; set; }
        /* Menu */
        public DbSet<ItemMenuDB> ItemsMenu { get; set; }
        public DbSet<PermisoAccesoDB> PermisosAcceso { get; set; }
        /* Biblioteca */
        public DbSet<DocumentoDB> Documentos { get; set; }
        // public DbSet<ReservaDB> Reservas { get; set; }
        ///public DbSet<EjemplarDB> Ejemplar { get; set; }
        //public DbSet<PrestamoDB> Prestamos { get; set; }
    }
}
