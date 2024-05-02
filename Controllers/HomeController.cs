using Microsoft.AspNetCore.Mvc;
using projett.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Diagnostics; // Ajout de ce using pour pouvoir utiliser ToList()


namespace projett.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly projettestContext _context; // Renommer projettestContext pour suivre les conventions C#

        public HomeController(ILogger<HomeController> logger, projettestContext context)
        {
            _logger = logger;
            _context = context; // Affecter le contexte de base de données local au champ de classe
        }

        public IActionResult Index()
        {
            var commandes = _context.Commandes.ToList(); // Charger les commandes depuis la base de données
           // return View(commandes);
           return RedirectToAction("Login");

           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View(); // Retourner la vue de connexion
        }

        [HttpPost] // Décorer avec [HttpPost] pour gérer la soumission du formulaire
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Veuillez saisir un nom d'utilisateur et un mot de passe.");
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Nom == username && u.MotsDePasse == password); // Rechercher l'utilisateur dans la base de données

            if (user != null)
            {
                // Utilisateur authentifié, stocker son ID dans la session et rediriger vers la page des commandes
                HttpContext.Session.SetInt32("UserId", user.UserId);

                // Utilisateur authentifié, redirigez-le vers une page sécurisée
                return RedirectToAction("Index","Commandes");
            }
            else
            {
                // Informez l'utilisateur que les informations de connexion sont incorrectes
                ModelState.AddModelError("", "Nom d'utilisateur ou mot de passe incorrect.");
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
