using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.Models
{
    public class usercart
    {
        [Key]
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int qty { get; set; }
    }
}
