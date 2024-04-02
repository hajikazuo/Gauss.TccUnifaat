using Gauss.TccUnifaat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gauss.TccUnifaat.MVC.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class ControllerBase : Controller
    {
        public readonly ApplicationDbContext _context;
        public RT.Comb.ICombProvider _comb;
        public Guid UserGuid { get; set; }

        public ControllerBase(ApplicationDbContext context, RT.Comb.ICombProvider comb)
        {
            _comb = comb;
            _context = context;

        }
    }
}
