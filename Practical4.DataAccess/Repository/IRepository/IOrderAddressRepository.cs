﻿using Practical4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.DataAccess.Repository.IRepository
{
    public interface IOrderAddressRepository
    {
        Task<OrderAddress> Add(OrderAddress orderAddress);
        public IEnumerable<OrderAddress> GetAll();


    }
}
