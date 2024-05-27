namespace FechasFestivas.Dominio.Entidades
{
    [Table("Tipo")]
    public class Tipo
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Tipo"), StringLength(100)]
        public required String Nombre { get; set; }
    }
}