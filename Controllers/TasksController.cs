using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

public class TasksController : Controller
{
    private readonly ApplicationDbContext _context;

    public TasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Afficher toutes les tâches
    public async Task<IActionResult> Index()
    {
        var tasks = await _context.TodoTasks.ToListAsync();
        return View(tasks);
    }

    // Afficher le formulaire de création
    public IActionResult Create()
    {
        return View();
    }

    // Créer une tâche
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description,DueDate")] TodoTask task)
    {
        if (ModelState.IsValid)
        {
            task.CreatedAt = DateTime.UtcNow;
            task.IsCompleted = false;
            _context.TodoTasks.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(task);
    }

    // Afficher le formulaire d'édition
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var task = await _context.TodoTasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    // Mettre à jour une tâche
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted,DueDate")] TodoTask task)
    {
        if (id != task.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingTask = await _context.TodoTasks.FindAsync(id);
                if (existingTask == null)
                {
                    return NotFound();
                }

                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.IsCompleted = task.IsCompleted;
                existingTask.DueDate = task.DueDate;

                _context.Update(existingTask);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TaskExists(task.Id))
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
        return View(task);
    }

    // Supprimer une tâche (afficher confirmation)
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var task = await _context.TodoTasks
            .FirstOrDefaultAsync(m => m.Id == id);
        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    // Confirmer la suppression
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _context.TodoTasks.FindAsync(id);
        if (task != null)
        {
            _context.TodoTasks.Remove(task);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // Marquer comme terminée/non terminée
    [HttpPost]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        var task = await _context.TodoTasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        task.IsCompleted = !task.IsCompleted;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> TaskExists(int id)
    {
        return await _context.TodoTasks.AnyAsync(e => e.Id == id);
    }
    public IActionResult Calendar()
    {
        var tasks = _context.TodoTasks.ToList(); // Ajoute tes filtres si besoin
        return View(tasks);
    }

 

}
