using Microsoft.EntityFrameworkCore;
using HarvestCore.WebApi.Entites;

namespace HarvestCore.WebApi.Data
{
    /// <summary>
    ///  Controlador de la base de datos. Hereda de DbContext para proporcionar funcionalidad de Entity Framework Core.
    ///  Esta clase funciona como un puente entre la base de datos y el modelo de datos para crear la estructura de la base de datos.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Tabla de items de lookup en la base de datos
        /// </summary>
        public DbSet<LookupItem> LookupItems { get; set; }

        // Entidades futuras pueden ser agregadas aqui, e.g.:
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<Harvester> Harvesters { get; set; }
        public DbSet<HarvestTable> HarvestTables { get; set; }
        public DbSet<MacroTunnel> MacroTunnels { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Harvest> Harvests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<Harvest>()
            //     .Property(h => h.QualityLevel)
            //     .HasConversion<string>()
            //     .HasMaxLength(20)
            //     .IsRequired();


            // Country: Solo se configura un indice para codigo de pais
            modelBuilder.Entity<Country>()
                .HasIndex(c => c.CountryCode)
                .IsUnique();

            // State: Se configura la relacion con Country y un indice para codigo de estado
            modelBuilder.Entity<State>(entity =>
            {
                entity.HasOne(s => s.Country)
                    .WithMany(c => c.States)
                    .HasForeignKey(s => s.IdCountry)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasIndex(s => s.StateCode)
                    .IsUnique();
            });

            // Community: Se configura la relacion con State y un indice para codigo de comunidad
            modelBuilder.Entity<Community>(entity =>
            {
                entity.Property(cm => cm.CommunityKey)
                    .HasMaxLength(20);

                entity.HasOne(cm => cm.State)
                    .WithMany(s => s.Communities)
                    .HasForeignKey(s => s.IdState)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasIndex(cm => cm.CommunityKey)
                    .IsUnique();
            });

            // Crew: Se configura la relacion con Community y un indice para codigo de crew
            modelBuilder.Entity<Crew>(entity =>
            {
                entity.Property(cw => cw.CrewKey)
                    .HasMaxLength(20);

                entity.HasOne(cw => cw.CommunityEntity)
                    .WithMany(cm => cm.Crews)
                    .HasForeignKey(cw => cw.IdCommunity)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasIndex(cw => cw.CrewKey)
                    .IsUnique();
            });

            // Harvester: Se configura la relacion con Crew y un indice para codigo de harvester
            modelBuilder.Entity<Harvester>(entity =>
            {
                entity.HasOne(hr => hr.CrewEntity)
                    .WithMany(cr => cr.Harvesters)
                    .HasForeignKey(hr => hr.IdCrew)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasIndex(hr => hr.HarvesterKey)
                    .IsUnique();
            });
        
            // MacroTunnel: Se configura la relacion con HarvestTable y un indice para codigo de macrotunnel
            modelBuilder.Entity<MacroTunnel>(entity =>
            {
                entity.HasOne(mt => mt.HarvestTable)
                    .WithMany(ht => ht.MacroTunnels)
                    .HasForeignKey(mt => mt.IdHarvestTable)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasIndex(mt => mt.MacroTunnelKey)
                    .IsUnique();
            });

            // Crop: Se configura la relacion con HarvestTable y un indice para codigo de crop
            modelBuilder.Entity<Crop>(entity =>
            {
                entity.Property(c => c.Category)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(c => c.Season)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.HasIndex(c => c.CropKey)
                    .IsUnique();
            });


            // Harvest: Se configura la relacion con Harvester, MacroTunnel y Crop y un indice para codigo de harvest
            modelBuilder.Entity<Harvest>(entity =>
                {
                    entity.Property(h => h.QualityLevel)
                        .HasConversion<string>()
                        .HasMaxLength(20)
                        .IsRequired();

                    entity.HasOne(h => h.HarvesterEntity)
                        .WithMany(hr => hr.Harvests)
                        .HasForeignKey(h => h.IdHarvester)
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.HasOne(h => h.MacroTunnelEntity)
                        .WithMany(mt => mt.Harvests)
                        .HasForeignKey(h => h.IdMacroTunnel)
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.HasOne(h => h.CropEntity)
                        .WithMany(c => c.Harvests)
                        .HasForeignKey(h => h.IdCrop)
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.HasIndex(h => h.HarvestKey)
                        .IsUnique();
                }
            );
                

        }

    }
}