
using API.DataBase;
using API.Models;
using API.Models.Biblioteca;
using API.Models.Usuario;
using API.Models.UtilidadGeneral;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentosController : ControllerBase
    {
        private SitioDB db;
        public DocumentosController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public Respuesta Crear(DocumentoDB doc)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Documentos.Add(doc);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Documento ingresado exitosamente";
                    respuesta.item = doc.id_documento;
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
        public async Task<Respuesta> Get(int id_documento)
        {
            Respuesta respuesta = new Respuesta();
            DocumentoDB? doc = await db.Documentos.FindAsync(id_documento);
            if (doc == null)
            {
                doc = new DocumentoDB();
                respuesta.codigo = 200;
                respuesta.mensaje = "Documento no encontrado";
                respuesta.item = doc;
                respuesta.status = true;
            }
            else
            {
                respuesta.codigo = 200;
                respuesta.mensaje = "Documento ingresado exitosamente";
                respuesta.item = doc.id_documento;
                respuesta.status = true;
            }
            return respuesta;

        }
        [HttpGet]
        [Route("List")]
        public Respuesta Listar() // Solo 3 niveles de profundidad
        {
            Respuesta respuesta = new Respuesta();
            List<DocumentoDB> list = db.Documentos.ToList();
            respuesta.status = true;
            respuesta.item = list;
            respuesta.mensaje = "Lista obtenida con éxito";
            respuesta.codigo = 200;
            return respuesta;
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(DocumentoDB doc)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                DocumentoDB? Original = db.Documentos.Find(doc.id_documento);
                if (Original == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Documento no encontrado";
                    respuesta.status = false;
                    return respuesta;
                }
                else
                {
                    Original.autor = doc.autor;
                    Original.cantidad = doc.cantidad;
                    Original.fechaPublicacion = doc.fechaPublicacion;
                    Original.codigoUbicacion = doc.codigoUbicacion;
                    Original.editorial = doc.editorial;

                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        respuesta.codigo = 200;
                        respuesta.mensaje = "Documento editado exitosamente";
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
        public Respuesta Eliminar(Int64 id_documento)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                DocumentoDB? Eldocinado = db.Documentos.Find(id_documento);
                if (Eldocinado == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Documento no encontrado";
                    respuesta.status = true;
                }
                else
                {
                    db.Remove(Eldocinado);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Documento eliminado exitosamente";
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