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
    public class OrderItemsRepository:IOrderItemsRepository
    {
        private ApplicationDbContext _db;
        internal DbSet<OrderItems> dbSet;

        public OrderItemsRepository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<OrderItems>();
        }
        public OrderItems GetFirstOrDefault(Expression<Func<OrderItems, bool>> filter)
        {
            IQueryable<OrderItems> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }
        public IEnumerable<OrderItems> GetAll()
        {
            return _db.orderItems.ToList();
        }
    }
}
