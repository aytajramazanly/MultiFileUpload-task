using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class ExpertsComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int ExpertId { get; set; }
        public Expert Expert { get; set; }
    }
}
