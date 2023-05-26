using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Practical4.DataAccess;
using Practical4.DataAccess.Repository;
using Practical4.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:FunctionsStartup(typeof(Practical4.Startup))]
namespace Practical4
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //var configuration = new ConfigurationBuilder()
            //    .AddJsonFile("local.settings.json",optional:true,reloadOnChange:true)
            //    .AddEnvironmentVariables()
            //    .Build();
            var con_string = Environment.GetEnvironmentVariable("ConnectionString");;

            // Register UserManager<IdentityUser>

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(con_string, ServerVersion.AutoDetect(con_string)));

            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderAddressRepository, OrderAddressRepository>();
            builder.Services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();



        }
    }
}
