﻿using Microsoft.EntityFrameworkCore;
using Practical4.DataAccess.Repository.IRepository;
using Practical4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.DataAccess.Repository
{
    public class AddressRepository:IAddressRepository
    {
        private readonly ApplicationDbContext _db;

        public AddressRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Address> GetAll()
        {
            return _db.address.ToList();
        }
        public async Task<Address> Add(Address address)
        {
            _db.address.Add(address);
            _db.SaveChanges();

            return address;
        }
    }
}
