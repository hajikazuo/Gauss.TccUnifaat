using Gauss.TccUnifaat.Common.Extensions;
using Gauss.TccUnifaat.Common.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Gauss.TccUnifaat.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Gauss.TccUnifaat-ee3b3716-0578-47a0-a21c-95ad9422bc57;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

public class ApplicationDbContext : IdentityDbContext<Usuario, Funcao, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Noticia> Noticias { get; set; }
    public DbSet<Turma> Turmas { get; set; }  

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entities<IStatusModificacao>(this, nameof(this.ModelStatusModificacao));
    }

    private void ModelStatusModificacao<TEntity>(EntityTypeBuilder<TEntity> entity) where TEntity : class, IStatusModificacao
    {
        entity.HasQueryFilter(x => !x.Excluido);
        entity.Property(x => x.DataExcluido).HasColumnType("datetime2(2)");
        entity.Property(x => x.DataCadastro).HasColumnType("datetime2(2)");
        entity.Property(x => x.DataUltimaModificacao).HasColumnType("datetime2(2)");
    }


    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        PreencheIStatusModificacao();
        return base.SaveChanges();
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        PreencheIStatusModificacao();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void PreencheIStatusModificacao()
    {

        foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity != null
                    && typeof(IStatusModificacao).IsAssignableFrom(e.Entity.GetType())))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            }
            if (entry.State == EntityState.Modified)
            {
                entry.Property("DataUltimaModificacao").CurrentValue = DateTime.Now;
                entry.Property("DataCadastro").IsModified = false;
            }
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Property("DataExcluido").CurrentValue = DateTime.Now;
                entry.Property("Excluido").CurrentValue = true;
                entry.Property("DataCadastro").IsModified = false;
            }
        }

    }

}