using Gauss.TccUnifaat.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gauss.TccUnifaat.MVC.Areas.Portal.Controllers
{
    public class PortalController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PortalController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var json = await httpClient.GetStringAsync("http://localhost:5032/api/Admin");

            List<Turma> turma = JsonConvert.DeserializeObject<List<Turma>>(json);
            return View("Index", turma);

        }

    }
}
