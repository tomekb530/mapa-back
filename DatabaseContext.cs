using mapa_back.Models;
using mapa_back.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace mapa_back
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<SchoolFromRSPO> SchoolsFromRSPO { get; set; }
        public DbSet<School> Schools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolFromRSPO>().ToTable("rspo_cache").Property(e => e.PodmiotProwadzacy)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<ManagingEntity>>(v) ?? new List<ManagingEntity>())
            .HasColumnType("jsonb");

            modelBuilder.Entity<School>().ToTable("private_schools").Property(e => e.PodmiotProwadzacy)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<ManagingEntity>>(v) ?? new List<ManagingEntity>())
            .HasColumnType("jsonb");
        }
    }
}
