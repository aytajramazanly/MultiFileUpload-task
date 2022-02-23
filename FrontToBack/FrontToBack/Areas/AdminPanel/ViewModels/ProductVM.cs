using FrontToBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public List<Category> categories { get; set; }
    }
}
