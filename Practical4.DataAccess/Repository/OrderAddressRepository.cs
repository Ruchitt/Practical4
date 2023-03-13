using Practical4.DataAccess.Repository.IRepository;
using Practical4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.DataAccess.Repository
{
    public class OrderAddressRepository:IOrderAddressRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderAddressRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OrderAddress> Add(OrderAddress orderAddress)
        {
            _db.orderAddresses.Add(orderAddress);
            _db.SaveChanges();

            return orderAddress;
        }
    }
}
