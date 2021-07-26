using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoGameListMVC.Models;

namespace VideoGameListMVC.Controllers
{
    public class VideoGamesController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public VideoGame VideoGame { get; set; }
        public VideoGamesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        //this is actually an upsert 'get' action
        public IActionResult Upsert(int? id)
        {

            VideoGame = new VideoGame();
            if(id == null)
            {
                //return empty model to be created
                return View(VideoGame);
            }
            //return model from database to show in update view for updating
            VideoGame = _db.VideoGames.FirstOrDefault(u => u.Id == id);

            if (VideoGame == null)
            {
                return NotFound();
            }
            return View(VideoGame);


            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //No need to specify parameter since we are using [BindProperty]
        public IActionResult Upsert()
        {
            //VideoGame = videoGame;

            if (ModelState.IsValid)
            {
                if(VideoGame.Id == 0)
                {
                    //insert
                    _db.VideoGames.Add(VideoGame);
                }
                else
                {
                    //update
                    _db.VideoGames.Update(VideoGame);

                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(VideoGame);

        }

        #region API calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.VideoGames.ToListAsync() });
        }

        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            var videoGamesCount = await _db.VideoGames.CountAsync();

            return Json(new { count = videoGamesCount });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var videoGameFromDb = await _db.VideoGames.FirstOrDefaultAsync(u => u.Id == id);
            if (videoGameFromDb == null)
            {
                return Json(new { success = false, message =  "Error while deleting" });
            }
            _db.VideoGames.Remove(videoGameFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Succesfully deleted!" });
        }




        #endregion
    }
}
