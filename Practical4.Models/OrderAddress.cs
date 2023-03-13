using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.Models
{
    public class OrderAddress
    {
        [Key]
        public int OrderAddId { get; set; }
        public int OrderId { get; set; }
        public int AddressId { get; set; }
    }
}
