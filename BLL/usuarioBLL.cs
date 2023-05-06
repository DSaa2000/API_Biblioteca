using API.DAL;
using API.DataBase;
using API.Models;
using System.Data;
using API.Models.Usuario;

namespace API.BLL
{
    public class usuarioBLL
    {
        private SitioDB db;
        public usuarioBLL(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        public async Task<Respuesta> Listar_Usuarios()
        {
            Respuesta respuesta = new Respuesta();
            usuarioDAL DAO = new usuarioDAL(db);
            List<Usuario> lista = new List<Usuario>();

            DataSet dataSet;
            try
            {
                dataSet = await DAO.Listar_Usuarios();
                if (dataSet.Tables.Count > 0)
                {
                    DataTable tabla = dataSet.Tables[0];
                    if (tabla.Rows.Count > 0)
                    {
                        Usuario u; ;
                        foreach (DataRow fila in tabla.Rows)
                        {
                            u = new Usuario();
                            u.id_usuario = Convert.ToInt32(fila["ID_Usuario"]);
                            u.id_rol = Convert.ToInt32(fila["ID_Rol"]);
                            u.nombre = fila["Nombre"].ToString();
                            u.correo = fila["Correo"].ToString();
                            u.password = "";// fila["Password"].ToString();
                            u.rol = new RolDB();
                            u.rol.id_rol = Convert.ToInt32(fila["ID_Rol"]);
                            u.rol.nombre = fila["NombreRol"].ToString();
                            lista.Add(u);
                        }
                        respuesta.codigo = 200;
                        respuesta.mensaje = "ok";
                        respuesta.status = true;
                        respuesta.item = lista;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.codigo = 1;
                respuesta.status = false;
                respuesta.mensaje = ex.Message;
            }
            return respuesta;
        }
        public async Task<Respuesta> Insertar_PermisosAcceso(List<PermisoAccesoDB> permisos)
        {
            Respuesta respuesta = new Respuesta();
            usuarioDAL DAO = new usuarioDAL(db);
            List<Usuario> lista = new List<Usuario>();

            //DataSet dataSet;
            try
            {
                foreach (PermisoAccesoDB p in permisos)
                {
                    await DAO.Insertar_PermisosAcceso(p);
                }
                respuesta.codigo = 200;
                respuesta.status = true;
                respuesta.mensaje = "Permisos de Acceso registrados Exitosamente";
            }
            catch (Exception ex)
            {
                respuesta.codigo = 1;
                respuesta.status = false;
                respuesta.mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}
