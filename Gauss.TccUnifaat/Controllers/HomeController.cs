using Gauss.TccUnifaat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gauss.TccUnifaat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Usuario> _userManager;
        public RT.Comb.ICombProvider _comb;

        public HomeController(ILogger<HomeController> logger, UserManager<Usuario> userManager, RT.Comb.ICombProvider comb)
        {
            _logger = logger;
            _userManager = userManager;
            _comb = comb;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}