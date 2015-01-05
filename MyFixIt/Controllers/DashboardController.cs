﻿
using MyFixIt.Persistence;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyFixIt.Controllers
{
     [Authorize]
    public class DashboardController : Controller
    {
        private readonly IFixItTaskRepository _fixItRepository;

        public DashboardController(IFixItTaskRepository repository)
        {
            _fixItRepository = repository;
        }

        // GET: /Dashboard/
        public async Task<ActionResult> Index()
        {
            string currentUser = User.Identity.Name;
            var result = await _fixItRepository.FindOpenTasksByOwnerAsync(currentUser);

            return View(result);
        }

        // GET: /Dashboard/Details/5
        public async Task<ActionResult> Details(int id)
        {
            FixItTask fixItTask = await _fixItRepository.FindTaskByIdAsync(id);

            if (fixItTask == null)
            {
                return HttpNotFound();
            }
            
            return View(fixItTask);
        }

        // GET: /Dashboard/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            FixItTask fixittask = await _fixItRepository.FindTaskByIdAsync(id);
            if (fixittask == null)
            {
                return HttpNotFound();
            }

            // Verify logged in user owns this FixIt task.
            if (User.Identity.Name != fixittask.Owner)
            {
               return HttpNotFound();
            }

            return View(fixittask);
        }

        // POST: /Dashboard/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind(Include = "CreatedBy,Owner,Title,Notes,PhotoUrl,IsDone")]FormCollection form)
        {
            FixItTask fixittask = await _fixItRepository.FindTaskByIdAsync(id);

            // Verify logged in user owns this FixIt task.
            if (User.Identity.Name != fixittask.Owner)
            {
               return HttpNotFound();
            }

            if (TryUpdateModel(fixittask, form))
            {
                await _fixItRepository.UpdateAsync(fixittask);
                return RedirectToAction("Index");
            }

            return View(fixittask);
        }
    }
}
