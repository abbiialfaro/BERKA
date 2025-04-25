using Microsoft.EntityFrameworkCore;

namespace BERKA.Models
{
    public class BERKAcontext : DbContext
    {
        public BERKAcontext(DbContextOptions<BERKAcontext> options)
            : base(options)
        {
        }

        public DbSet<Cita> Citas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Combustible> Combustibles { get; set; }
        public DbSet<Detalle_Revision> Detalle_Revisiones { get; set; }
        public DbSet<Estacion> Estaciones { get; set; }
        public DbSet<Inspector> Inspectores { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Revision> Revisiones { get; set; }
        public DbSet<Tipos_Placa> Tipos_Placas { get; set; }
        public DbSet<Tipos_Vehiculo> Tipos_Vehiculos { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.ID_Cliente)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Inspector)
                .WithMany()
                .HasForeignKey(c => c.ID_Inspector)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Vehiculo)
                .WithMany()
                .HasForeignKey(c => c.ID_Vehiculo)
                .OnDelete(DeleteBehavior.NoAction); // 👈 Cambiado para evitar múltiples rutas
        }


    }

}
  
