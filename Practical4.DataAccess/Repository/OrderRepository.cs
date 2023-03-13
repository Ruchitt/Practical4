using Microsoft.EntityFrameworkCore;
using Practical4.DataAccess.Repository.IRepository;
using Practical4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.DataAccess.Repository
{
    public class OrderRepository:IOrderRepository
    {
        private ApplicationDbContext _db;
        internal DbSet<Order> dbSet;

        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<Order>();
        }

        public void Update(Order obj)
        {
            _db.orders.Update(obj);
            _db.SaveChanges();
        }
        public Order GetFirstOrDefault(Expression<Func<Order, bool>> filter)
        {
            IQueryable<Order> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }
    }
}
