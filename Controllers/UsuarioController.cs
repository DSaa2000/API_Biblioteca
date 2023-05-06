using API.DataBase;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using API.BLL;
using API.Models.UtilidadGeneral;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private SitioDB db;
        public UsuarioController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<Respuesta> Crear(UsuarioDB u)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Usuarios.Add(u);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Usuario creado exitosamente";
                    respuesta.status = true;
                    respuesta.item = u.id_usuario;
                }
                else
                {
                    respuesta.codigo = 400;
                    respuesta.mensaje = "Modelo Inválido";
                    respuesta.status = false;
                }
            }
            catch (Exception ex)
            {
                respuesta.codigo = 500;
                respuesta.mensaje = "Error: " + ex.Message;
                respuesta.status = false;
            }
            return respuesta;
        }
        [HttpPost]
        [Route("Create_Perfil")]
        public Respuesta Crear_Perfil(NuevoPerfil p)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioDB nu = new UsuarioDB();
                    nu.nombre = p.user;
                    nu.correo = p.email;
                    nu.password = p.password;
                    nu.id_rol = p.id_rol;
                    db.Usuarios.Add(nu);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Usuario creado exitosamente";
                    respuesta.status = true;
                    respuesta.item = nu.id_usuario;
                }
                else
                {
                    respuesta.codigo = 400;
                    respuesta.mensaje = "Modelo Inválido";
                    respuesta.status = false;
                }
            }
            catch (Exception ex)
            {
                respuesta.codigo = 500;
                respuesta.mensaje = "Error: " + ex.Message;
                respuesta.status = false;
            }
            return respuesta;
        }
        [HttpGet]
        [Route("Get")]
        public async Task<UsuarioDB> Get(Int64 ID_Usuario)
        {
            UsuarioDB u = await db.Usuarios.FindAsync(ID_Usuario);
            if (u == null)
            {
                u = new UsuarioDB();
                u.id_usuario = 0;
                u.nombre = "";
                u.correo = "";
                u.password = "";
                u.id_rol = 0;
            }
            return u;

        }
        [HttpGet]
        [Route("List")]
        public async Task<List<UsuarioDB>> Listar()
        {
            List<UsuarioDB> list = await db.Usuarios.ToListAsync();
            return list;
        }
        [HttpGet]
        [Route("ListSP")]
        public async Task<List<Usuario>> ListarSP()
        {
            usuarioBLL BLL = new usuarioBLL(db);
            List<Usuario> List = new List<Usuario>();
            try
            {
                Respuesta respuesta = new Respuesta();
                respuesta = await BLL.Listar_Usuarios();
                if (respuesta.status)
                {
                    List = (List<Usuario>)respuesta.item;
                }
            }
            catch (Exception ex)
            {

            }
            return List;
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(Usuario u)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                UsuarioDB Original = db.Usuarios.Find(u.id_usuario);
                if (Original == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Usuario no encontrado";
                    respuesta.status = false;
                    return respuesta;
                }
                else
                {
                    Original.nombre = u.nombre;
                    Original.correo = u.correo;
                    Original.password = u.password;

                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        respuesta.codigo = 200;
                        respuesta.mensaje = "Usuario editado exitosamente";
                        respuesta.status = true;
                    }
                    else
                    {
                        respuesta.codigo = 200;
                        respuesta.mensaje = "Error al editar el usuario";
                        respuesta.status = false;
                        return respuesta;
                    }

                }
            }
            catch (Exception ex)
            {
                respuesta.codigo = 500;
                respuesta.mensaje = "Error: " + ex.Message;
                respuesta.status = false;
            }
            return respuesta;

        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<Respuesta> Eliminar(Int64 ID_Usuario)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                UsuarioDB Eliminado = db.Usuarios.Find(ID_Usuario);
                if (Eliminado == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Usuario no encontrado";
                    respuesta.status = true;
                }
                else
                {
                    db.Remove(Eliminado);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Usuario eliminado exitosamente";
                    respuesta.status = false;
                }
            }
            catch (Exception ex)
            {
                respuesta.codigo = 500;
                respuesta.mensaje = "Error: " + ex.Message;
                respuesta.status = false;
            }
            return respuesta;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<Respuesta> Login(UsuarioDB user)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                List<UsuarioDB> lista = db.Usuarios.Where(u => (u.nombre == user.nombre || u.correo == user.nombre) && u.password == user.password).ToList();
                if (lista.Count == 0)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Acceso inválido. Por favor, inténtelo otra vez.";
                    respuesta.status = false;
                }
                else
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Iniciando Sesión";
                    respuesta.status = true;
                    Perfil perfil = new Perfil();
                    UsuarioDB userDB = lista.First();
                    perfil.Usuario = userDB;
                    int id_rol = userDB.id_rol;
                    
                    RolDB rol = db.RolesUsuarios.Find(id_rol);
                    List<PermisoAcceso> permisos = new List<PermisoAcceso>();
                    List<PermisoAccesoDB> list = db.PermisosAcceso.ToList();
                    List<ItemMenuDB> items = db.ItemsMenu.ToList();
                    Rol r = new Rol();
                    foreach (ItemMenuDB item in items)
                    {
                        PermisoAcceso pa = new PermisoAcceso();
                        pa.id_itemPadre = item.id_itemPadre;
                        pa.id_itemMenu = item.id_itemMenu;
                        pa.nombre = item.titulo;
                        pa.permiso = list.Where(i => i.id_itemMenu == item.id_itemMenu && i.id_rol == id_rol).ToList().Count == 1;
                        pa.path = item.url;
                        permisos.Add(pa);
                    }
                    r.rolUsuario = rol;
                    r.permisos = permisos;
                    perfil.Rol = r;
                    respuesta.item = perfil;
                }
            }
            catch (Exception ex) {
                respuesta.codigo = 500;
                respuesta.mensaje = "Error: " + ex.Message;
                respuesta.status = false;
            }
            
            return respuesta;

        }

    }
}