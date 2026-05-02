using GRH_SENTECH.Data;
using GRH_SENTECH.Entity;
using GRH_SENTECH.Service;
using GRH_SENTECH.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GRH_SENTECH.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeService _employeService;
        private readonly IDepartementService _departementService;
        private readonly GrhDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IEmployeService employeService, IDepartementService departementService,
            GrhDbContext context, ILogger<HomeController> logger)
        {
            _employeService = employeService;
            _departementService = departementService;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var derniers = _employeService.GetAllEmployes(0, 5);

                var dashboard = new DashboardViewModel
                {
                    TotalEmployes = _employeService.CountEmployes(),
                    TotalDepartements = _departementService.GetAllDepartements().Count(),
                    TotalContratsCDI = _context.Contrats.Count(c => c.IsActif && c.TypeContrat == TypeContrat.CDI),
                    TotalContratsCDD = _context.Contrats.Count(c => c.IsActif && c.TypeContrat == TypeContrat.CDD),
                    TotalStages = _context.Contrats.Count(c => c.IsActif && c.TypeContrat == TypeContrat.Stage),
                    CongesEnAttente = _context.Conges.Count(c => c.Statut == StatutConge.EnAttente),
                    DerniersEmployes = derniers.Select(e => EmployeViewModel.FromEntity(e))
                };

                return View(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur dans Dashboard");
                return View(new DashboardViewModel());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
