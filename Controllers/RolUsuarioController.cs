using API.DataBase;
using API.Models.Usuario;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models.UtilidadGeneral;
using API.BLL;
using API.DAL;
using Microsoft.Extensions.WebEncoders.Testing;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolUsuarioController : ControllerBase
    {
        private SitioDB db;
        public RolUsuarioController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<Respuesta> Crear(Rol r)
        {
            Respuesta respuesta = new Respuesta();
            usuarioDAL DAL = new usuarioDAL(db);
            try
            {
                if (ModelState.IsValid)
                {
                    List<PermisoAccesoDB> permisos = new List<PermisoAccesoDB>();
                                       
                    db.RolesUsuarios.Add(r.rolUsuario);
                    db.SaveChanges();
                    foreach (PermisoAcceso permiso in r.permisos)
                    {
                        if (permiso.permiso)
                        {
                            PermisoAccesoDB p = new PermisoAccesoDB();
                            p.id_itemMenu = permiso.id_itemMenu;
                            p.id_rol = r.rolUsuario.id_rol;
                            await DAL.Insertar_PermisosAcceso(p);
                        }
                    }
                    db.PermisosAcceso.AddRange(permisos);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Rol creado exitosamente";
                    respuesta.status = true;
                    respuesta.item = r.rolUsuario.id_rol;
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
        public async Task<RolDB> Get(Int64 id_rol)
        {
            RolDB? r = await db.RolesUsuarios.FindAsync(id_rol);
            if (r == null)
            {
                r = new RolDB();
                r.id_rol = 0;
                r.nombre = "";
            }
            return r;

        }
        [HttpGet]
        [Route("List")]
        public async Task<List<RolDB>> Listar()
        {
            List<RolDB> list = await db.RolesUsuarios.ToListAsync();
            return list;
        }
        [HttpGet]
        [Route("Listar_PermisosByRol")]
        public Rol Listar_PermisosByRol(int id_rol)
        {
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
                permisos.Add(pa);
            }
            r.rolUsuario = rol;
            r.permisos = permisos;
            return r;
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(Rol r)
        {
            usuarioDAL DAL = new usuarioDAL(db);
            Respuesta respuesta = new Respuesta();
            try
            {
                RolDB? Original = db.RolesUsuarios.Find(r.rolUsuario.id_rol);
                if (Original == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Rol no encontrado";
                    respuesta.status = false;
                    return respuesta;
                }
                else
                {
                    Original.nombre = r.rolUsuario.nombre;

                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        await DAL.Eliminar_PermisosAcceso(r.rolUsuario.id_rol);
                        foreach (PermisoAcceso p in r.permisos)
                        {
                            if (p.permiso)
                            {
                                PermisoAccesoDB pdb = new PermisoAccesoDB();
                                pdb.id_rol = r.rolUsuario.id_rol;
                                pdb.id_itemMenu = p.id_itemMenu;
                                await DAL.Insertar_PermisosAcceso(pdb);
                            }
                        }
                        respuesta.codigo = 200;
                        respuesta.mensaje = "Rol editado exitosamente";
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
        public Respuesta Eliminar(Int64 id_rol)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                RolDB? Eliminado = db.RolesUsuarios.Find(id_rol);
                if (Eliminado == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Rol no encontrado";
                    respuesta.status = true;
                }
                else
                {
                    db.Remove(Eliminado);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Rol eliminado exitosamente";
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
    }
}
