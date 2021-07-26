using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoGameListMVC.Models;

namespace VideoGameListMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Movie Movie { get; set; }
        public MoviesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Movie = new Movie();
            if (id == null)
            {
                //insert
                return View(Movie);
            }
            //update
            Movie = _db.Movies.FirstOrDefault(u => u.Id == id);

            if (Movie == null)
            {
                return NotFound();
            }
            return View(Movie);



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //No need to specify parameter since we are using [BindProperty]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Movie.Id == 0)
                {
                    //insert
                    _db.Movies.Add(Movie);
                }
                else
                {
                    //update
                    _db.Movies.Update(Movie);

                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Movie);

        }

        #region API calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Movies.ToListAsync() });
        }

        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            var moviesCount = await _db.Movies.CountAsync();

            return Json(new { count = moviesCount });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var MovieFromDb = await _db.Movies.FirstOrDefaultAsync(u => u.Id == id);
            if (MovieFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _db.Movies.Remove(MovieFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Succesfully deleted!" });
        }




        #endregion
    }
}
