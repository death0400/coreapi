using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DTestContext.Models
{
    public partial class DTestContext : DbContext
    {
        public DTestContext()
        {
        }

        public DTestContext(DbContextOptions<DTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CGameIssuseNew> CGameIssuseNew { get; set; }
        public virtual DbSet<Region> Region { get; set; }

 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CGameIssuseNew>(entity =>
            {
                entity.ToTable("C_Game_Issuse_New");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Issue)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LogTime).HasColumnType("datetime");

                entity.Property(e => e.Lottery)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ServerTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });
        }
    }
}
