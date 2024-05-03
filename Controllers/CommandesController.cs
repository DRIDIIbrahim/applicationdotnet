using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projett.Models;
using Microsoft.AspNetCore.Http;

namespace projett.Controllers
{
    
    public class CommandesController : Controller
    {
        private readonly projettestContext _context;

        public CommandesController(projettestContext context)
        {
            _context = context;
        }

        // GET: Commandes
        public async Task<IActionResult> Index()
        {

            
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                // Redirigez vers la page de connexion si l'utilisateur n'est pas authentifié
                return RedirectToAction("Login", "Home");
            }
            IQueryable<Commande> commandesQuery;

            if (userId == 3)
            {
                // Si l'ID utilisateur est 3, récupérez toutes les commandes
                commandesQuery = _context.Commandes;
            }
            else
            {
                // Sinon, récupérez les commandes de l'utilisateur spécifique
                commandesQuery = _context.Commandes.Where(c => c.UserId == userId);
            }

            var commandes = await commandesQuery.ToListAsync();

            return View(commandes);

            
        }

        // GET: Commandes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Commandes == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        // GET: Commandes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Commandes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("OrderId,UserId,Article,Qte,Prix")] Commande commande)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(commande);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", commande.UserId);
        //    return View(commande);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,Article,Qte,Prix,Status")] Commande commande)
        {
            if (ModelState.IsValid)
            {
                // Récupérer l'ID de l'utilisateur à partir de la session
                var userId = HttpContext.Session.GetInt32("UserId");

                // Vérifier si l'ID de l'utilisateur est disponible
                if (userId.HasValue)
                {
                    // Assigner l'ID de l'utilisateur à la commande
                    commande.UserId = userId.Value;

                    // Ajouter la commande à la base de données et sauvegarder les modifications
                    _context.Add(commande);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Rediriger vers la page de connexion si l'ID de l'utilisateur n'est pas disponible
                    return RedirectToAction("Login", "Home");
                }
            }
            return View(commande);
        }


        // GET: Commandes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Commandes == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", commande.UserId);
            return View(commande);
        }

        // POST: Commandes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,UserId,Article,Qte,Prix")] Commande commande)
        {
            if (id != commande.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commande);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommandeExists(commande.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", commande.UserId);
            return View(commande);
        }

        // GET: Commandes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Commandes == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        // POST: Commandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Commandes == null)
            {
                return Problem("Entity set 'projettestContext.Commandes'  is null.");
            }
            var commande = await _context.Commandes.FindAsync(id);
            if (commande != null)
            {
                _context.Commandes.Remove(commande);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Valider(int orderId)
        {
            var commande = await _context.Commandes.FindAsync(orderId);
            if (commande != null)
            {
                commande.Status = "Validée";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Refuser(int orderId)
        {
            var commande = await _context.Commandes.FindAsync(orderId);
            if (commande != null)
            {
                commande.Status = "Refuser";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }



        private bool CommandeExists(int id)
        {
          return (_context.Commandes?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
