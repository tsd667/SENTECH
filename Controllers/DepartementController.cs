using GRH_SENTECH.Service;
using GRH_SENTECH.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GRH_SENTECH.Controllers
{
    public class DepartementController : Controller
    {
        private readonly IDepartementService _departementService;
        private readonly ILogger<DepartementController> _logger;

        public DepartementController(IDepartementService departementService, ILogger<DepartementController> logger)
        {
            _departementService = departementService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var departements = _departementService.GetAllDepartements()
                    .Select(d => DepartementViewModel.FromEntity(d));
                return View(departements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur dans Index Departement");
                TempData["Error"] = "Erreur lors du chargement des départements.";
                return View(new List<DepartementViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DepartementViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartementViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var departement = viewModel.ToEntity();
            var result = _departementService.CreateDepartement(departement);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "Erreur.");
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dept = _departementService.GetDepartementById(id);
            if (dept == null)
            {
                TempData["Error"] = "Département introuvable.";
                return RedirectToAction(nameof(Index));
            }
            return View(DepartementViewModel.FromEntity(dept));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DepartementViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(viewModel);

            var dept = viewModel.ToEntity();
            var result = _departementService.UpdateDepartement(dept);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "Erreur.");
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var result = _departementService.DeleteDepartement(id);
            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
