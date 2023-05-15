#nullable disable
using Microsoft.EntityFrameworkCore;

namespace LudusStadium.Models
{
    public partial class LudusStadiumsContext : DbContext
    {
        public LudusStadiumsContext()
        {
        }

        public LudusStadiumsContext(DbContextOptions<LudusStadiumsContext> options) : base(options)
        {
        }

        public virtual DbSet<address> addresses { get; set; }
        public virtual DbSet<stadium> stadia { get; set; }
        public virtual DbSet<schedule> schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<address>(entity =>
            {
                entity.ToTable("address");

                entity.Property(a => a.ID).ValueGeneratedOnAdd();

                entity.Property(a => a.street)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(a => a.city)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(a => a.state)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(a => a.number).IsRequired();

                entity.Property(a => a.zip)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<stadium>(entity =>
            {
                entity.ToTable("stadium");

                entity.Property(st => st.ID).ValueGeneratedOnAdd();

                entity.Property(st => st.name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(st => st.capacity).IsRequired();

                entity.HasOne(st => st.Address)
                    .WithMany()
                    .HasForeignKey(st => st.FK_Address_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_ID");
            });

            modelBuilder.Entity<schedule>(entity =>
            {
                entity.ToTable("schedule");

                entity.Property(sc => sc.ID).ValueGeneratedOnAdd();

                entity.Property(sc => sc.matchDate).HasColumnType("datetime");

                entity.Property(sc => sc.FK_Match_ID).IsRequired();

                entity.HasOne(sc => sc.Stadium)
                 .WithMany()
                 .HasForeignKey(sc => sc.FK_Stadium_ID)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_Stadium_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}