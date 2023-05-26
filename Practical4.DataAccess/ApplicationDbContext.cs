using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Practical4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical4.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Address> address { get; set; }
        public DbSet<OrderAddress> orderAddresses { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItems> orderItems { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<usercart> usercart { get; set; }


    }
}
