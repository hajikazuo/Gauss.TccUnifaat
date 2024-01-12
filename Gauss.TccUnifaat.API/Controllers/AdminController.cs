using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauss.TccUnifaat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Turma>> GetTurmas()
        {
            var turmas = await _context.Turmas.Include(t => t.Usuarios).ToListAsync();

            if (turmas.Count == 0)
            {
                return NotFound("Não foram encontradas turmas.");
            }

            return Ok(turmas);
        }
    }
}
