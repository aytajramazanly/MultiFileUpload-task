using FrontToBack.DataAccessLayer;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            if (page < 1)
                return BadRequest();

            var totalPageCount = Math.Ceiling((decimal)await _dbContext.Categories.CountAsync() / 2);
            if (page > totalPageCount)
                return NotFound();

            ViewBag.totalPageCount = totalPageCount;
            ViewBag.currentPage = page;
            var categories = await _dbContext.Categories.Skip((page-1)*2).Take(2).ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View();
            if (await _dbContext.Categories.AnyAsync(x => x.Name.ToLower().Trim() == category.Name.ToLower().Trim()))
            {
                ModelState.AddModelError("Name", "Category with this name already exist!");
                return View();
            }
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View();

            var existCtegory = await _dbContext.Categories.FindAsync(category.Id);
            if (existCtegory == null)
                return NotFound();

            var isNameExist = await _dbContext.Categories.AnyAsync(x => x.Name.ToLower().Trim() == category.Name.ToLower().Trim() && x.Id!=category.Id);
            if (isNameExist)
            {
                ModelState.AddModelError("name", "Category with this name already exist!");
                return View();
            }
            existCtegory.Name = category.Name;
            existCtegory.Description = category.Description;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
                return Json(new { status = 404});

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return Json(new { status = 200 });
        }
    }
}
