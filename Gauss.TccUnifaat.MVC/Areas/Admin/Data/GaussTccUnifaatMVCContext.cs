using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gauss.TccUnifaat.Common.Models;

namespace Gauss.TccUnifaat.MVC.Data
{
    /// <summary>
    /// usado apenas para scaffolding
    /// </summary>
    [Obsolete]
    
    public class GaussTccUnifaatMVCContext : DbContext
    {
        public GaussTccUnifaatMVCContext (DbContextOptions<GaussTccUnifaatMVCContext> options)
            : base(options)
        {
        }

        public DbSet<Gauss.TccUnifaat.Common.Models.Noticia> Noticia { get; set; } = default!;
    }
}
