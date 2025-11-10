using HallOfFame.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Data;

public partial class HallOfFameDbContext : DbContext
{
    public HallOfFameDbContext() { }

    public HallOfFameDbContext(DbContextOptions<HallOfFameDbContext> options) : base(options) { }

    public DbSet<Person> Persons { get; set; }

    public DbSet<Skill> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("persons");

            entity.HasKey(p => p.Id).HasName("persons_pkey");

            entity.Property(p => p.Id).HasColumnName("id");

            entity.Property(p => p.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.DisplayName)
                .HasColumnName("display_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.HasMany(p => p.Skills)
                .WithOne(s => s.Person)
                .HasForeignKey(s => s.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.ToTable("skills");

            entity.HasKey(s => s.Id).HasName("skills_pkey");

            entity.Property(s => s.Id).HasColumnName("id");

            entity.Property(s => s.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(s => s.Level)
                .HasColumnName("level")
                .IsRequired();

            entity.Property(s => s.PersonId).HasColumnName("person_id");

            entity.ToTable(s => s.HasCheckConstraint("ck_skill_level", "level BETWEEN 1 AND 10"));
        });
    }
}
