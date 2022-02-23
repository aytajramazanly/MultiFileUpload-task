using FrontToBack.DataAccessLayer;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class BasketController : Controller
    {

        private readonly AppDbContext _dbContext;

        public BasketController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var basket = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basket))
                return Content("Empty");

            var basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            var newBasket = new List<BasketVM>();

            foreach (var basketVM in basketVMs)
            {
                var product = await _dbContext.Products.FindAsync(basketVM.Id);
                if (product==null)
                {
                    continue;
                }
                newBasket.Add(new BasketVM
                {
                    Id= product.Id,
                    Image= product.Image,
                    Price= product.Price,
                    Name= product.Name,
                    Count=basketVM.Count,
                    TotalPrice=product.Price*basketVM.Count
                });
            }
            basket = JsonConvert.SerializeObject(basketVMs);
            Response.Cookies.Append("basket", basket);
            return View(newBasket);
        }

        //public IActionResult DecreaseProductBasketCount(int? id)
        //{
        //    if (id == null)
        //        return BadRequest();
        //    var basket = Request.Cookies["basket"];
        //    if (string.IsNullOrEmpty(basket))
        //        return BadRequest();
        //    var products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
        //    foreach (var item in products)
        //    {
        //        if (item.Id==id)
        //        {
        //            item.Count--;
        //            if (item.Count == 0)
        //                products = products.Where(x => x.Id != id).ToList();
        //        }
        //    }
        //    Response.Cookies.Append("basket", JsonConvert.SerializeObject(products));
        //    return RedirectToAction(nameof(Index));
        //}
        //public IActionResult IncreaseProductBasketCount(int? id)
        //{
        //    if (id == null)
        //        return BadRequest();
        //    var basket = Request.Cookies["basket"];
        //    if (string.IsNullOrEmpty(basket))
        //        return BadRequest();
        //    var products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
        //    foreach (var item in products)
        //    {
        //        if (item.Id == id)
        //        {
        //            item.Count++;
        //        }
        //    }
        //    Response.Cookies.Append("basket", JsonConvert.SerializeObject(products));
        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> ChangeCount(int? id,char operation)
        {
            if (id == null)
                return BadRequest();
            var basket = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basket))
                return BadRequest();
            var products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
        

            foreach (var item in products)
            {
                if (item.Id == id)
                {
                    switch (operation)
                    {
                        case '-':
                            item.Count--;
                            if (item.Count == 0)
                                products = products.Where(x => x.Id != id).ToList();
                            break;
                        case '+':
                            item.Count++;
                            break;
                        default:
                            break;
                    }
                }
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products));
            var newBasket = new List<BasketVM>();

            foreach (var item in products)
            {
                var product = await _dbContext.Products.FindAsync(item.Id);
                if (product == null)
                {
                    continue;
                }
                newBasket.Add(new BasketVM
                {
                    Id = product.Id,
                    Image = product.Image,
                    Price = product.Price,
                    Name = product.Name,
                    Count = item.Count,
                    TotalPrice = product.Price * item.Count
                });
            }
            return PartialView("_BasketPartial", newBasket);
        }
        public async Task<IActionResult> DeleteProductFromBasket(int? id)
        {
            if (id == null)
                return BadRequest();
            var basket = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basket))
                return BadRequest();
            var products = JsonConvert.DeserializeObject<List<BasketVM>>(basket).Where(x=>x.Id!=id).ToList();
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products));
            var newBasket = new List<BasketVM>();
           
            double totalCart = 0;
            foreach (var item in products)
            {
                var product = await _dbContext.Products.FindAsync(item.Id);
                if (product == null)
                {
                    continue;
                }
                newBasket.Add(new BasketVM
                {
                    Id = product.Id,
                    Image = product.Image,
                    Price = product.Price,
                    Name = product.Name,
                    Count = item.Count,
                    TotalPrice = product.Price * item.Count
                });
                totalCart += item.Count * product.Price;
            }
            return PartialView("_BasketPartial", newBasket);
        }

        public async Task<List<BasketVM>> TakeProductInfo(List<BasketVM> products)
        {
            var newBasket = new List<BasketVM>();

            foreach (var item in products)
            {
                var product = await _dbContext.Products.FindAsync(item.Id);
                if (product == null)
                {
                    continue;
                }
                newBasket.Add(new BasketVM
                {
                    Id = product.Id,
                    Image = product.Image,
                    Price = product.Price,
                    Name = product.Name,
                    Count = item.Count,
                    TotalPrice = product.Price * item.Count
                });
            }
            return newBasket;
        }
    }
}
