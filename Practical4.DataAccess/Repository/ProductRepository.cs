using Microsoft.EntityFrameworkCore;
using Practical4.DataAccess.Repository.IRepository;
using Practical4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.DataAccess.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<Products> dbSet;


        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<Products>();

        }

        public IEnumerable<Products> GetAll()
        {
            return _db.products.ToList();
        }
        public async Task<Products> Add(Products products)
        {
            _db.products.Add(products);
            _db.SaveChanges();

            return products;
        }
        //public Address GetFirstOrDefault(Expression<Func<Address, bool>> filter)
        //{
        //    IQueryable<Address> query = dbSet;
        //    query = query.Where(filter);
        //    return query.FirstOrDefault();
        //}
        //public void Update(Address obj)
        //{
        //    _db.address.Update(obj);
        //    _db.SaveChanges();
        //}
    }
}
