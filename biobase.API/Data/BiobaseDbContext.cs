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
        public DbSet<Taxa> taxa { get; set; }
        public DbSet<TaxaGroups> taxa_groups { get; set; }
        public DbSet<TraitsCategories> traits_categories { get; set; }
        public DbSet<User> authentication { get; set; }
        public DbSet<HabitatClassesTaxa> habitat_classes_taxa { get; set; }
        public DbSet<HabitatCodes> habitat_classes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HabitatClassesTaxa>()
                .HasNoKey()
                .HasIndex(h => h.taxon_category);

            modelBuilder.Entity<HabitatClassesTaxa>()
                .HasIndex(h => h.habitat_code);

            modelBuilder.Entity<HabitatClassesTaxa>()
                .HasIndex(h => h.rl);

            modelBuilder.Entity<HabitatClassesTaxa>()
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

            modelBuilder.Entity<HabitatCodes>()
                .HasNoKey();

            modelBuilder.Entity<TaxaGroups>()
                .HasNoKey();
        }
    }
}