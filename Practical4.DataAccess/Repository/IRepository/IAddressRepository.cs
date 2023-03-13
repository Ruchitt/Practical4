using Practical4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.DataAccess.Repository.IRepository
{
    public interface IAddressRepository
    {
       public IEnumerable<Address> GetAll();
       Task<Address> Add(Address address);

    }
}
