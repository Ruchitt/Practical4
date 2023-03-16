using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.Models
{
    public class Address
    {

		public int AddressId { get; set; }
        //public int? UserId { get; set; }
        public AddressType Type { get; set; }
        public string AddressDetail { get; set; }
		public string Country { get; set; }
		public string State { get; set; }
		public string City { get; set; }
        public string ZipCode { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNo { get; set; }

    }
    public enum AddressType
    {
        Shipping,
        Billing
    }
}
