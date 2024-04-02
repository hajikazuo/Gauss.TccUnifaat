using Gauss.TccUnifaat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gauss.TccUnifaat.Controllers
{
    [Authorize]
    public class ControllerBase<TContext, TComb> : Controller
    {
        protected readonly TContext _context;
        protected readonly TComb _comb;
        public Guid UserGuid { get; set; }

        public ControllerBase(TContext context, TComb comb)
        {
            _context = context;
            _comb = comb;
        }
    }
}