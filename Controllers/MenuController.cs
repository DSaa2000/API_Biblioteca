
using API.DataBase;
using API.Models;
using API.Models.Usuario;
using API.Models.UtilidadGeneral;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private SitioDB db;
        public MenuController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public Respuesta Crear(ItemMenuDB im)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    db.ItemsMenu.Add(im);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "ItemMenu creado exitosamente";
                    respuesta.item = im.id_itemMenu;
                    respuesta.status = true;
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
        public async Task<ItemMenuDB> Get(Int64 id_itemMenu)
        {
            ItemMenuDB im = await db.ItemsMenu.FindAsync(id_itemMenu);
            if (im == null)
            {
                im = new ItemMenuDB();
                im.id_itemMenu = 0;
                im.id_itemPadre = 0;
                im.titulo = "";
                im.url = "";
                im.icon = "";                
            }
            return im;

        }
        [HttpGet]
        [Route("List")]
        public List<ItemMenu> Listar(Int64 ID_Rol) // Solo 3 niveles de profundidad
        {
            List<Int64> permisos = new List<Int64>();
            List<PermisoAccesoDB> listapermisosDB = db.PermisosAcceso.ToList().Where(p => p.id_rol == ID_Rol).ToList();
            List<ItemMenuDB> list = db.ItemsMenu.ToList();
           
            foreach (PermisoAccesoDB p in listapermisosDB)
            {
                permisos.Add(p.id_itemMenu);
            }
            List<ItemMenu> listOrdenada = new List<ItemMenu>();
            list = list.Where(i => permisos.Contains(i.id_itemMenu)).ToList();
            List<Int64> ids = new List<Int64>();
            foreach (ItemMenuDB item in list)
            {
                if (!ids.Contains(item.id_itemMenu) && item.id_itemPadre == 0)
                {
                    ids.Add(item.id_itemMenu);
                    ItemMenu im = new ItemMenu();
                    im.id_itemMenu = item.id_itemMenu;
                    im.id_itemPadre = item.id_itemPadre;
                    im.label = item.titulo;
                    im.url = item.url;
                    im.icon = item.icon;
                    im.permiso = true;
                    im.prioridad = item.prioridad;
                    im.children = new List<ItemMenu>();
                    List<ItemMenuDB> l = list.Where(i => i.id_itemPadre == im.id_itemMenu).ToList();
                    foreach (ItemMenuDB item2 in l)
                    {
                        if (!ids.Contains(item2.id_itemMenu))
                        {
                            ids.Add(item2.id_itemMenu);
                            ItemMenu im2 = new ItemMenu();
                            im2.id_itemMenu = item2.id_itemMenu;
                            im2.id_itemPadre = item2.id_itemPadre;
                            im2.label = item2.titulo;
                            im2.url = item2.url;
                            im2.permiso = true;
                            im2.prioridad = item2.prioridad;
                            im2.icon = item2.icon;
                            im2.children = new List<ItemMenu>();
                            List<ItemMenuDB> l2 = list.Where(i => i.id_itemPadre == im2.id_itemMenu).ToList();
                            foreach (ItemMenuDB item3 in l2)
                            {
                                if (!ids.Contains(item3.id_itemMenu))
                                {
                                    ids.Add(item3.id_itemMenu);
                                    ItemMenu im3 = new ItemMenu();
                                    im3.id_itemMenu = item3.id_itemMenu;
                                    im3.id_itemPadre = item3.id_itemPadre;
                                    im3.label = item3.titulo;
                                    im3.url = item3.url;
                                    im3.permiso = true;
                                    im3.prioridad = item3.prioridad;
                                    im3.icon = item3.icon;
                                    im3.children = new List<ItemMenu>();
                                    List<ItemMenuDB> l3 = list.Where(i => i.id_itemPadre == im3.id_itemMenu).ToList();
                                    /* CONTINUAR EN 4TO NIVEL */
                                    foreach (ItemMenuDB item4 in l3)
                                    {
                                        if (!ids.Contains(item4.id_itemMenu) )
                                        {
                                            ids.Add(item4.id_itemMenu);
                                            ItemMenu im4 = new ItemMenu();
                                            im4.id_itemMenu = item4.id_itemMenu;
                                            im4.id_itemPadre = item4.id_itemPadre;
                                            im4.label = item4.titulo;
                                            im4.permiso = true;
                                            im4.url = item4.url;
                                            im4.prioridad = item4.prioridad;
                                            im4.icon = item4.icon;
                                            im4.children = new List<ItemMenu>();
                                            /* CONTINUAR EN 4TO NIVEL */
                                            //im3.children = im3.children.OrderBy(i => i.prioridad).ToList();
                                            im3.children.Add(im4);
                                        }
                                    }
                                    im2.children.Add(im3);
                                }
                            }
                            im2.children = im2.children.OrderBy(i => i.prioridad).ToList();
                            im.children.Add(im2);

                        }                    
                    }
                    im.children = im.children.OrderBy(i => i.prioridad).ToList();
                    listOrdenada.Add(im);
                }               
            }
            return listOrdenada.OrderBy(i => i.prioridad).ToList();
        }
        [HttpGet]
        [Route("Listar_Items")]
        public async Task<List<ItemMenu>> Listar_Items() // Solo 3 niveles de profundidad
        {
            List<ItemMenuDB> list = await db.ItemsMenu.ToListAsync();
            List<ItemMenu> lista = new List<ItemMenu>();
            foreach (ItemMenuDB item in list)
            {
                ItemMenu im = new ItemMenu();
                im.id_itemMenu = item.id_itemMenu;
                im.id_itemPadre = item.id_itemPadre;
                im.label = item.titulo;
                im.url = item.url;
                im.icon = item.icon;
                im.prioridad = item.prioridad;
                im.children = new List<ItemMenu>();
                lista.Add(im);
                
            }
            return lista.OrderBy(i => i.id_itemMenu).ToList();
        }
        [HttpGet]
        [Route("Listar_Modulos")]
        public async Task<List<ItemMenu>> ListarModulos() // Solo 3 niveles de profundidad
        {
            List<ItemMenuDB> list = await db.ItemsMenu.ToListAsync();
            List<ItemMenu> listOrdenada = new List<ItemMenu>();
            List<Int64> ids = new List<Int64>();
            foreach (ItemMenuDB item in list)
            {
                if (!ids.Contains(item.id_itemMenu) && item.id_itemPadre == 0)
                {
                    ids.Add(item.id_itemMenu);
                    ItemMenu im = new ItemMenu();
                    im.id_itemMenu = item.id_itemMenu;
                    im.id_itemPadre = item.id_itemPadre;
                    im.label = item.titulo;
                    im.url = item.url;
                    im.icon = item.icon;
                    im.prioridad = item.prioridad;
                    im.children = new List<ItemMenu>();
                    listOrdenada.Add(im);
                }
            }
            return listOrdenada.OrderBy(i => i.prioridad).ToList();
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(ItemMenuDB im)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                ItemMenuDB? Original = db.ItemsMenu.Find(im.id_itemMenu);
                if (Original == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "ItemMenu no encontrado";
                    respuesta.status = false;
                    return respuesta;
                }
                else
                {
                    Original.titulo = im.titulo;
                    Original.id_itemPadre = im.id_itemPadre;
                    Original.url = im.url;
                    Original.icon = im.icon;
                    Original.prioridad = im.prioridad;

                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        respuesta.codigo = 200;
                        respuesta.mensaje = "ItemMenu editado exitosamente";
                        respuesta.status = true;
                    }
                    else
                    {
                        respuesta.codigo = 200;
                        respuesta.mensaje = "Error al editar el producto";
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
        public Respuesta Eliminar(Int64 id_itemMenu)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                ItemMenuDB? Eliminado = db.ItemsMenu.Find(id_itemMenu);
                if (Eliminado == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "ItemMenu no encontrado";
                    respuesta.status = true;
                }
                else
                {
                    db.Remove(Eliminado);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "ItemMenu eliminado exitosamente";
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