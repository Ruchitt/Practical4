using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Practical4.DataAccess.Repository.IRepository;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System.Linq;
using Practical4.Models;
using static Practical4.Models.Order;

namespace Practical4
{
    public class Function1
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderAddressRepository _orderAddressRepository;



        public Function1(IAddressRepository addressRepository, IOrderRepository orderRepository, IOrderAddressRepository orderAddressRepository)
        {
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _orderAddressRepository = orderAddressRepository;
        }

        [FunctionName("AddAddress")]
        public async Task<IActionResult> AddAddress(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var address = JsonConvert.DeserializeObject<Address>(requestBody);
                var addedAddress = _addressRepository.Add(address);

                return new OkObjectResult(new Response { Data = addedAddress, Status = "Success" });
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error adding address to database");
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("GetAddressList")]
        public async Task<IActionResult> GetAddressList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var addresses = _addressRepository.GetAll();
                return new OkObjectResult(new Response { Data = addresses, Status = "Success" });
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error getting addresses from database");
                return new StatusCodeResult(500);
            }
        }
        [FunctionName("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
          ILogger log)
        {
            var orderid = req.Query["orderid"];
            var status = req.Query["status"];
            var order = _orderRepository.GetFirstOrDefault(o => o.OrderId == int.Parse(orderid));
            if (order.IsActive != false)
            {
                if (order.Status >= (StatusType)int.Parse(status))
                {
                    return new StatusCodeResult(500);
                }
                else
                {
                    order.Status = (StatusType)int.Parse(status);
                    _orderRepository.Update(order);
                }
            }
            return new OkObjectResult(new Response { Data = order, Status = "Success" });
        }

        //[FunctionName("CreateDraftOrder")]
        //public async Task<IActionResult> CreateDraftOrder(
        //   [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        //   ILogger log)
        //{
        //    try
        //    {
        //        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //        var address = JsonConvert.DeserializeObject<OrderAddress>(requestBody);
        //        var addedAddress = _orderAddressRepository.Add(address);

        //        return new OkObjectResult(new Response { Data = addedAddress, Status = "Success" });
        //    }
        //    catch (Exception ex)
        //    {
        //        log.LogError(ex, "Error adding address to database");
        //        return new StatusCodeResult(500);
        //    }
        //}

            //[FunctionName("Update OrderAddress")]
            //public static async Task<IActionResult> UpdateOrderAddress(
            //  [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            //  ILogger log)
            //{
            //    log.LogInformation("C# HTTP trigger function processed a request.");

            //    string name = req.Query["name"];

            //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //    dynamic data = JsonConvert.DeserializeObject(requestBody);
            //    name = name ?? data?.name;

            //    string responseMessage = string.IsNullOrEmpty(name)
            //        ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //        : $"Hello, {name}. This HTTP triggered function executed successfully.";

            //    return new OkObjectResult(responseMessage);
            //}
        }
    }

