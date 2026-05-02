using GRH_SENTECH.Entity;
using GRH_SENTECH.Repository;
using GRH_SENTECH.Service;
using GRH_SENTECH.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GRH_SENTECH.Controllers
{
    public class EmployeController : Controller
    {
        private readonly IEmployeService _employeService;
        private readonly IDepartementService _departementService;
        private readonly IPosteRepository _posteRepository;
        private readonly IContratRepository _contratRepository;
        private readonly ILogger<EmployeController> _logger;

        public EmployeController(
            IEmployeService employeService,
            IDepartementService departementService,
            IPosteRepository posteRepository,
            IContratRepository contratRepository,
            ILogger<EmployeController> logger)
        {
            _employeService = employeService;
            _departementService = departementService;
            _posteRepository = posteRepository;
            _contratRepository = contratRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, int limit = 10, string? search = null, int? departementId = null)
        {
            try
            {
                int offset = (page - 1) * limit;
                var employes = _employeService.GetAllEmployes(offset, limit);
                int total = _employeService.CountEmployes();

                if (!string.IsNullOrEmpty(search))
                {
                    employes = employes.Where(e =>
                        e.Nom.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        e.Prenom.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        e.Matricule.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        e.Email.Contains(search, StringComparison.OrdinalIgnoreCase)
                    );
                }

                if (departementId.HasValue && departementId > 0)
                {
                    employes = employes.Where(e => e.DepartementId == departementId.Value);
                }

                var viewModel = new EmployeListViewModel
                {
                    Employes = employes.Select(e => EmployeViewModel.FromEntity(e)),
                    TotalCount = total,
                    Page = page,
                    Limit = limit,
                    SearchTerm = search,
                    DepartementFilter = departementId,
                    Departements = _departementService.GetAllDepartements()
                        .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom })
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur dans Index Employe");
                TempData["Error"] = "Une erreur est survenue lors du chargement des employés.";
                return View(new EmployeListViewModel());
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                var employe = _employeService.GetEmployeById(id);
                if (employe == null)
                {
                    TempData["Error"] = "Employé introuvable.";
                    return RedirectToAction(nameof(Index));
                }
                return View(EmployeViewModel.FromEntity(employe));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur dans Details Employe {Id}", id);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new EmployeViewModel
            {
                DateEmbauche = DateTime.Now,
                Departements = _departementService.GetAllDepartements()
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom })
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Departements = _departementService.GetAllDepartements()
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom });
                return View(viewModel);
            }

            try
            {
                var employe = viewModel.ToEntity();
                var result = _employeService.CreateEmploye(employe);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, result.Message ?? "Erreur lors de la création.");
                viewModel.Departements = _departementService.GetAllDepartements()
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom });
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création d'un employé");
                ModelState.AddModelError(string.Empty, "Une erreur inattendue est survenue.");
                viewModel.Departements = _departementService.GetAllDepartements()
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom });
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var employe = _employeService.GetEmployeById(id);
                if (employe == null)
                {
                    TempData["Error"] = "Employé introuvable.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = EmployeViewModel.FromEntity(employe);
                viewModel.Departements = _departementService.GetAllDepartements()
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom });
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur dans Edit GET Employe {Id}", id);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EmployeViewModel viewModel)
        {
            if (id != viewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                viewModel.Departements = _departementService.GetAllDepartements()
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom });
                return View(viewModel);
            }

            try
            {
                var employe = viewModel.ToEntity();
                var result = _employeService.UpdateEmploye(employe);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, result.Message ?? "Erreur lors de la modification.");
                viewModel.Departements = _departementService.GetAllDepartements()
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom });
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur dans Edit POST Employe {Id}", id);
                ModelState.AddModelError(string.Empty, "Une erreur inattendue est survenue.");
                viewModel.Departements = _departementService.GetAllDepartements()
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Nom });
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var employe = _employeService.GetEmployeById(id);
                if (employe == null)
                {
                    TempData["Error"] = "Employé introuvable.";
                    return RedirectToAction(nameof(Index));
                }
                return View(EmployeViewModel.FromEntity(employe));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur dans Delete GET Employe {Id}", id);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var result = _employeService.DeleteEmploye(id);
                if (result.Success)
                    TempData["Success"] = result.Message;
                else
                    TempData["Error"] = result.Message;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur dans Delete POST Employe {Id}", id);
                TempData["Error"] = "Erreur lors de la suppression.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult AjouterContrat(int id)
        {
            var employe = _employeService.GetEmployeById(id);
            if (employe == null)
            {
                TempData["Error"] = "Employé introuvable.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new ContratViewModel
            {
                EmployeId = id,
                EmployeNomComplet = employe.NomComplet,
                DateDebut = DateTime.Now,
                Postes = _posteRepository.FindAll()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.Intitule} (N{p.NiveauHierarchique}) - {p.SalaireMin:N0} à {p.SalaireMax:N0} FCFA"
                    })
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AjouterContrat(ContratViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Postes = _posteRepository.FindAll()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.Intitule} (N{p.NiveauHierarchique}) - {p.SalaireMin:N0} à {p.SalaireMax:N0} FCFA"
                    });
                var emp = _employeService.GetEmployeById(viewModel.EmployeId);
                viewModel.EmployeNomComplet = emp?.NomComplet;
                return View(viewModel);
            }

            var contrat = new Contrat
            {
                EmployeId = viewModel.EmployeId,
                PosteId = viewModel.PosteId,
                TypeContrat = viewModel.TypeContrat,
                DateDebut = viewModel.DateDebut,
                DateFin = viewModel.DateFin,
                SalaireBase = viewModel.SalaireBase,
                PeriodeEssai = viewModel.PeriodeEssai,
                IsActif = true
            };

            var result = _employeService.AjouterContrat(contrat);
            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Details), new { id = viewModel.EmployeId });
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "Erreur lors de l'ajout du contrat.");
            viewModel.Postes = _posteRepository.FindAll()
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Intitule });
            return View(viewModel);
        }
    }
}
