namespace API.Models.Biblioteca
{
    public class ReservaDB
    {
        public int id_reserva { get; set; }
        public int id_ejemplar { get; set; }
        public int id_usuario { get; set; }
        public int id_prestamo { get; set; }
        public DateTime fechaReserva { get; set; }
        public string estado { get; set; }
    }
}
