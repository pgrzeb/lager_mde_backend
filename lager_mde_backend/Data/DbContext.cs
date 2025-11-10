using Microsoft.EntityFrameworkCore;
using lager_mde_backend.Entities;

namespace lager_mde_backend.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Artikel> Artikel { get; set; }
    public DbSet<Lagstamm> Lagstamm { get; set; }
    public DbSet<Personal> Personal { get; set; }
    public DbSet<Stapauf> Stapauf { get; set; }
    public DbSet<Inventur> Inventur { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Define primary key for Personal entity
        
        modelBuilder.Entity<Artikel>().ToTable("artikel").HasKey(a => a.art_id);
        modelBuilder.Entity<Lagstamm>().ToTable("lagstamm").HasKey(l => l.lag_id);
        modelBuilder.Entity<Personal>().ToTable("personal").HasKey(p => p.pers_id);
        modelBuilder.Entity<Stapauf>().ToTable("stapauf").HasKey(s => s.stap_id);
        modelBuilder.Entity<Inventur>().ToTable("inventur").HasKey(i => i.id);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }
}