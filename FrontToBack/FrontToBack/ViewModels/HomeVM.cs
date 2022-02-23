using FrontToBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.ViewModels
{
    public class HomeVM
    {
        public Slider Slider { get; set; }
        public List<SliderImage> SliderImages { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public About About { get; set; }
        public List<Advantage> Advantages { get; set; }
        public ExpertsHeading ExpertsHeading { get; set; }
        public List<Expert> Experts { get; set; }
        public Subscribe Subscribe { get; set; }
        public BlogHeading BlogHeading { get; set; }
        public List<BlogPost> BlogPosts { get; set; }
        public List<ExpertsComment> ExpertsComments { get; set; }
        public List<InstagramPost> InstagramPosts { get; set; }
        public List<Position> Positions { get; set; }
    }
}
