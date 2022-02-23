using FrontToBack.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Advantage> Advantages { get; set; }
        public DbSet<ExpertsHeading> ExpertsHeadings { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<BlogHeading> BlogHeadings { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<ExpertsComment> ExpertsComments { get; set; }
        public DbSet<InstagramPost> InstagramPosts { get; set; }
        public DbSet<Bio> Bios { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
