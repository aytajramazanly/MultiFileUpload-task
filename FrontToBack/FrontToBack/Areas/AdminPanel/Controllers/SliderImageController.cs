using FrontToBack.Areas.AdminPanel.Data;
using FrontToBack.DataAccessLayer;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderImageController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly int _existSliderImagesCount;

        public SliderImageController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _existSliderImagesCount = _dbContext.SliderImages.Count();
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page < 1)
                return NotFound();
            if (((page - 1) * 10) >= await _dbContext.SliderImages.CountAsync())
                page--;
            var totalPageCount = Math.Ceiling((decimal)await _dbContext.SliderImages.CountAsync() / 10);
            if (page > totalPageCount)
                return NotFound();

            ViewBag.totalPageCount = totalPageCount;
            ViewBag.currentPage = page;
            var sliderImages = await _dbContext.SliderImages.Skip((page - 1) * 10).Take(10).ToListAsync();

            return View(sliderImages);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();
            var sliderImage = await _dbContext.SliderImages.FirstOrDefaultAsync(x => x.Id == id);
            if (sliderImage == null)
                return NotFound();

            return View(sliderImage);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderImage sliderImage)
        {
            var createLimite = 5 - _existSliderImagesCount;
            if (!ModelState.IsValid)
                return View();
            if (sliderImage.Photos.Count > createLimite)
            {
                ModelState.AddModelError("Photos", $"You can upload just {createLimite} Photo");
                return View();
            }
            foreach (var photo in sliderImage.Photos)
            {
                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName}-File must to be a Photo!");
                    return View();
                }

                if (!photo.IsAllowedSize(2))
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName}-The size of the photo cannot be more than one MegaByte!");
                    return View();
                }
                var newSliderImage= new SliderImage { Name= await photo.GenerateFile(Constants.ImageFolderPath) } ;

                await _dbContext.SliderImages.AddAsync(newSliderImage);
                await _dbContext.SaveChangesAsync();
            }
           
            return RedirectToAction(nameof(Index));
        }
       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var sliderImage = await _dbContext.SliderImages.FindAsync(id);
            if (sliderImage == null)
                return Json(new { status = 404 });
            var path = Path.Combine(Constants.ImageFolderPath, "img", sliderImage.Name);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.Remove(sliderImage);
            await _dbContext.SaveChangesAsync();
            return Json(new { status = 200 });
        }
    }
}
