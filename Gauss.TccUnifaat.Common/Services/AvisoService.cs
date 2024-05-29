using Gauss.TccUnifaat.Common.Services.Interface;
using Gauss.TccUnifaat.Data;
using Microsoft.EntityFrameworkCore;
namespace Gauss.TccUnifaat.Common.Services
{
    public class AvisoService : IAvisoService
    {
        private readonly ApplicationDbContext _context;

        public AvisoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExcluirAvisosAntigosAsync()
        {
            var avisosAntigos = await _context.Avisos
                .Where(a => a.DataAviso < DateTime.Now.Date)
                .ToListAsync();

            if (avisosAntigos.Any())
            {
                _context.Avisos.RemoveRange(avisosAntigos);
                await _context.SaveChangesAsync();
            }
        }
    }
}
