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
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Practical4
{
    public class Function1
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderAddressRepository _orderAddressRepository;
        private readonly IOrderItemsRepository _orderItemsRepository;


        public Function1(IAddressRepository addressRepository, IOrderRepository orderRepository, IOrderAddressRepository orderAddressRepository, IOrderItemsRepository orderItemsRepository)
        {
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _orderAddressRepository = orderAddressRepository;
            _orderItemsRepository = orderItemsRepository;

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
                return new OkObjectResult(new Response { Data = "Error While Adding Address", Status = "Error" });

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
                return new OkObjectResult(new Response { Data = "Error While Getting AddressList", Status = "Error" });

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
                    return new OkObjectResult(new Response { Data = "Error While Updating OrderStatus", Status = "Error" });
                }
                else
                {
                    order.Status = (StatusType)int.Parse(status);
                    _orderRepository.Update(order);
                }
            }
            return new OkObjectResult(new Response { Data = order, Status = "Success" });
        }


        [FunctionName("CreateDraftOrder")]
        public async Task<IActionResult> CreateDraftOrder(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var jsonObj = JObject.Parse(requestBody);
            var shippingaddressId = (int)jsonObj["shippingaddressid"];
            var billingaddressId = (int)jsonObj["billingaddressid"];
            var addressDetails = _addressRepository.GetFirstOrDefault(x => x.AddressId == shippingaddressId & x.AddressId == billingaddressId);
            if(addressDetails==null)
            {
               return new OkObjectResult(new Response { Data = "Address Not Found", Status = "Error" });
            }

            using (var client = new HttpClient())
            {
                var apiUrl = "https://localhost:44302/api/AddOrder/Add OrderItem?statusType=draft";
                var jsonContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(responseContent);
                    int orderId = responseObject.orderId;
                    var shippingAddress = new OrderAddress
                    {
                        OrderId = orderId,
                        AddressId = shippingaddressId,
                    };
                    _orderAddressRepository.Add(shippingAddress);

                    var billingAddress = new OrderAddress
                    {
                        OrderId = orderId,
                        AddressId = billingaddressId,
                    };
                    _orderAddressRepository.Add(billingAddress);
                    return new OkObjectResult(new Response { Data = orderId, Status = "Success" });
                }
                else
                {
                    return new OkObjectResult(new Response { Data ="Error While Creating Draft Order", Status = "Error" });
                }
            }
        }


        [FunctionName("UpdateOrderAddress")]
        public async Task<IActionResult> UpdateOrderAddress(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
          ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var jsonObj = JObject.Parse(requestBody);
            var orderid = (int)jsonObj["orderid"];
            var address = JsonConvert.DeserializeObject<Address>(requestBody);
            var orderDetails = _orderRepository.GetFirstOrDefault(x => x.OrderId == orderid);
            var addressDetails = _addressRepository.GetFirstOrDefault(x => x.AddressId == address.AddressId);

            if (orderDetails.Status < StatusType.Shipped)
            {
                addressDetails.State = address.State;
                addressDetails.AddressDetail = address.AddressDetail;
                addressDetails.ContactPerson = address.ContactPerson;
                addressDetails.City = address.City;
                addressDetails.ContactNo = address.ContactNo;
                addressDetails.Country = address.Country;

                _addressRepository.Update(addressDetails);
                return new OkObjectResult(new Response { Data = addressDetails, Status = "Success" });

            }
            else
            {
                return new OkObjectResult(new Response { Data = "Order Is Either Shipped Or Paid", Status = "Error" });

            }

        }


        [FunctionName("GetOrders")]
        public async Task<IActionResult> GetOrders(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var jsonObj = JObject.Parse(requestBody);

                string? customerNameOrEmail =(string) jsonObj["customerNameOrEmail"];
                string? orderStatus = (string)jsonObj["orderStatus"];
                string? productId = (string)jsonObj["productId"];
                string? dateRange = (string)jsonObj["dateRange"];
                string? Active_Inactive = (string)jsonObj["Active_Inactive"];
                //var allDetails = _orderRepository.GetAll()
                //    .Join(_orderAddressRepository.GetAll(), a =>a.OrderId , oA=>oA.OrderId,(a,oA) => new {a,oA})
                //    .Join(_orderItemsRepository.GetAll(),ooI=>ooI.oA.OrderId , oI=>oI.OrderId, (ooI, oI) => new { ooI,oI})
                //    .Join(_addressRepository.GetAll(),ooA=>ooA.ooI.oA.AddressId , A=>A.AddressId , (ooA, A) => new{ ooA,A})
                //    .Where(m=>m.ooA.ooI.a.CustomerName.ToLower().Contains(customerNameOrEmail.ToLower())
                //    || m.ooA.ooI.a.CustomerEmail.ToLower().Contains(customerNameOrEmail.ToLower())
                //    )
                //    .Select(m=> new
                //    {
                //        OrderID=m.ooA.ooI.a.OrderId,
                //        Description=m.ooA.ooI.a.Note,
                //        Customer_name=m.ooA.ooI.a.CustomerName,
                //        email= m.ooA.ooI.a.CustomerEmail,
                //        status= m.ooA.ooI.a.Status,
                //        totalItems= total+1,
                //        Total_Amount=m.ooA.ooI.a.TotalAmount,
                //        Shipping_address=m.A

                //    }).Distinct();
                var orders = _orderRepository.GetAll();
                var orderaddress = _orderAddressRepository.GetAll();
                var address = _addressRepository.GetAll();
                var orderitems = _orderItemsRepository.GetAll();

                var obj = (from o in orders
                          join oa in orderaddress on o.OrderId equals oa.OrderId
                          join a in address on oa.AddressId equals a.AddressId
                          join oi in orderitems on o.OrderId equals oi.OrderId into a1
                          where o.CustomerName.ToLower().Contains(customerNameOrEmail.ToLower())
                          || o.CustomerEmail.ToLower().Contains(customerNameOrEmail.ToLower())
                           select new
                          {
                              OrderID = o.OrderId,
                              Description = o.Note,
                              Customer_name = o.CustomerName,
                              email = o.CustomerEmail,
                              status = o.Status,
                              totalItems = a1.Count(),
                              Total_Amount = o.TotalAmount,
                              Shipping_address = a
                          }).DistinctBy(x=>x.OrderID);


                return new OkObjectResult(new  { Data = obj , Status = "Success" });
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new Response { Data = "Error While Getting Order List", Status = "Error" });

            }
        }
    }
}

