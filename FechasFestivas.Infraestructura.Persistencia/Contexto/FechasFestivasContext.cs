namespace FechasFestivas.Infraestructura.Persistencia.Contexto
{
    public class FechasFestivasContext:DbContext
    {

        public FechasFestivasContext(DbContextOptions<FechasFestivasContext> options)
            : base(options)
        {

        }

        public DbSet<Festivos> Festivos { get; set; }
        public DbSet<Tipo> Tipos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Festivos>(entidad =>
            {
                entidad.HasKey(e => e.Id);
                entidad.HasIndex(e => e.Nombre).IsUnique();
            });

            builder.Entity<Tipo>(entidad =>
            {
                entidad.HasKey(e => e.Id);
                entidad.HasIndex(e => e.Nombre).IsUnique();
            });

            builder.Entity<Festivos>()
                .HasOne(e => e.Tipo)
                .WithMany()
                .HasForeignKey(e => e.IdTipo);

        }

    }
}
