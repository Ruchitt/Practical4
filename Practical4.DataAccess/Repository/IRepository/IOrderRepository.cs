using Practical4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.DataAccess.Repository.IRepository
{
    public interface IOrderRepository
    {
        void Update(Order obj);
        Order GetFirstOrDefault(Expression<Func<Order, bool>> filter);
        public IEnumerable<Order> GetAll();

    }
}
