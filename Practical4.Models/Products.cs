using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.Models
{
    public class Products
    {
        [Key]
		public int ProductId { get; set; }
        public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }

    }
}
