using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Db.CommomLotteryData.Models
{
    public partial class Common_LotteryDataContext : DbContext
    {
        public Common_LotteryDataContext()
        {
        }

        public Common_LotteryDataContext(DbContextOptions<Common_LotteryDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CGameIssuseNew> CGameIssuseNew { get; set; }
        public virtual DbSet<CqsscGameWinNumber> CqsscGameWinNumber { get; set; }


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

            modelBuilder.Entity<CqsscGameWinNumber>(entity =>
            {
                entity.ToTable("CQSSC_GameWinNumber");

                entity.HasIndex(e => e.IssuseNumber);

                entity.Property(e => e.GameCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IssuseNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.WinNumber)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
