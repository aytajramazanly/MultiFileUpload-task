using FrontToBack.DataAccessLayer;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly int _productsCount;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _productsCount = _dbContext.Products.Count();
        }

        public IActionResult Index()
        {
            ViewBag.ProductsCount = _productsCount;
            return View(new HomeVM
            {
                Slider = _dbContext.Sliders.SingleOrDefault(),
                SliderImages = _dbContext.SliderImages.ToList(),
                Categories = _dbContext.Categories.ToList(),
                Products = _dbContext.Products.Include(x => x.Category).Take(4).ToList(),
                About = _dbContext.Abouts.SingleOrDefault(),
                Advantages = _dbContext.Advantages.ToList(),
                ExpertsHeading = _dbContext.ExpertsHeadings.SingleOrDefault(),
                Experts = _dbContext.Experts.Include(x=>x.Position).ToList(),
                Subscribe = _dbContext.Subscribes.SingleOrDefault(),
                BlogHeading = _dbContext.BlogHeadings.SingleOrDefault(),
                BlogPosts = _dbContext.BlogPosts.ToList(),
                ExpertsComments = _dbContext.ExpertsComments.Include(x => x.Expert).ToList(),
                InstagramPosts = _dbContext.InstagramPosts.ToList(),
                Positions=_dbContext.Positions.ToList()
            });
        }

        public IActionResult Load(int skip)
        {
            if (skip >= _productsCount)
            {
                return BadRequest();
            }

            var products = _dbContext.Products.Include(x => x.Category).Skip(skip).Take(4).ToList();

            return PartialView("_ProductPartial", products);
        }

        public async Task<IActionResult> Search(string searcedProduct)
        {
            if (string.IsNullOrEmpty(searcedProduct))
            {
                return NoContent();
            }

            var products = await _dbContext.Products.Where(x => x.Name.ToLower().Contains(searcedProduct.ToLower())).ToListAsync();

            return PartialView("_SearchedProductsPartial",products);
        }

        public async Task<IActionResult> AddToBasket(int? id)
        {
            if (id==null)
            {
                return BadRequest();
            }
           
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            List<BasketVM> basketVMs;
            var existBasket = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(existBasket))
            {
                basketVMs = new List<BasketVM>();
            }
            else
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(existBasket);
            }
            var existBasketVM = basketVMs.FirstOrDefault(x => x.Id == id);
            if (existBasketVM==null)
            {
                existBasketVM = new BasketVM
                {
                    Id = product.Id,
                    Price=product.Price,
                    
                };
                basketVMs.Add(existBasketVM);
            }
            else
            {
                existBasketVM.Count++;
            }

            var basket = JsonConvert.SerializeObject(basketVMs);
            Response.Cookies.Append("basket", basket);
            double totalCart = 0;
            foreach (var item in basketVMs)
            {
                totalCart += item.Count * item.Price;
            }
            var response = new List<double>();
            response.Add(totalCart);
            response.Add(basketVMs.Count());
            return Json(response);
        }
    }
}
