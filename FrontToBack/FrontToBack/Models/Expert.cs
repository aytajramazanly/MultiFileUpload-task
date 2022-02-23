using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Expert
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public int Age { get; set; }
        public int? PositionId { get; set; }
        public Position Position { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
