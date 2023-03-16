using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Practical4.Models
{
    public class OrderItems
    {
        [Key]
        [JsonIgnore]
        public int OrderItemId { get; set; }
        [JsonIgnore]
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public double Price { get; set; }
        public bool IsActive { get; set; }
    }
}
