using mapa_back.Models;
using mapa_back.Models.DTO;
using mapa_back.Models.RSPOApi;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace mapa_back
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<SchoolFromRSPO> SchoolsFromRSPO { get; set; }
        public DbSet<SchoolFromMap> SchoolsFromMapDatabase { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolFromRSPO>().ToTable("rspo_cache");
            modelBuilder.Entity<SchoolFromMap>().ToTable("private_schools");

            modelBuilder.Entity<SchoolFromRSPO>()
                .Property(s => s.BusinessData)
                .HasColumnType("jsonb");

            modelBuilder.Entity<SchoolFromMap>()
                .Property(s => s.BusinessData)
                .HasColumnType("jsonb");
        }
    }
}
