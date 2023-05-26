using Practical4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.DataAccess.Repository.IRepository
{
    public interface IProductRepository
    {
       public IEnumerable<Products> GetAll();
       Task<Products> Add(Products products);
        //Address GetFirstOrDefault(Expression<Func<Address, bool>> filter);
        //void Update(Address obj);


    }
}
