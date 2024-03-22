using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gauss.TccUnifaat.MVC.ViewModels
{
    public class HorarioViewModel
    {
        public string Usuario { get; set; }
        public string Disciplina { get; set; }
        public DateTime DataAula { get; set; }
    }
}
