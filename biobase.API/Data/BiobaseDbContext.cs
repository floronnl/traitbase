using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Data
{
    public class BiobaseDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiobaseDbContext"/> class.
        /// </summary>
        /// 
        public BiobaseDbContext(DbContextOptions<BiobaseDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        /// <summary>
        /// Gets the tables from the MySQL database.
        /// </summary>
        /// 
        public DbSet<HabitatClasses> habitat_classes { get; set; }
        public DbSet<EuHabitatClasses> eu_habitat_classes { get; set; }

        public DbSet<HabitatTaxa> habitat_taxa { get; set; }
        public DbSet<EuHabitatTaxa> eu_habitat_taxa { get; set; }

        public DbSet<TaxaGroups> taxa_groups { get; set; }
        public DbSet<Taxa> taxa { get; set; }
        public DbSet<EuTaxa> eu_bhd_species { get; set; }

 
        public DbSet<TraitsCategories> traits_categories { get; set; }

        public DbSet<User> authentication { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HabitatTaxa>()
                .HasNoKey()
                .HasIndex(h => h.taxon_category);

            modelBuilder.Entity<HabitatTaxa>()
                .HasIndex(h => h.habitat_code);

            modelBuilder.Entity<HabitatTaxa>()
                .HasIndex(h => h.rl);

            modelBuilder.Entity<HabitatTaxa>()
                .HasIndex(h => h.soortgroep);


            // Index on 'soortnummer' for Taxa
            modelBuilder.Entity<Taxa>()
                .HasIndex(t => t.soortnummer)
                .IsUnique() // Each soortnummer must be unique
                .HasDatabaseName("IX_Taxa_Soortnummer");

            // Index on 'groep' for Taxa (not unique)
            modelBuilder.Entity<Taxa>()
                .HasIndex(t => t.groep)
                .HasDatabaseName("IX_Taxa_Groep")
                .IsUnique(false);

            modelBuilder.Entity<HabitatClasses>()
                .HasNoKey();

            modelBuilder.Entity<TaxaGroups>()
                .HasNoKey();

            modelBuilder.Entity<EuHabitatClasses>()
                .HasNoKey();

            modelBuilder.Entity<EuTaxa>()
                .HasNoKey();

            modelBuilder.Entity<EuHabitatTaxa>()
                .HasNoKey();
        }
    }
}