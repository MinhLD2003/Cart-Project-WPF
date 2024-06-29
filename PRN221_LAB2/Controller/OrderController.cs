using Newtonsoft.Json;
using ServerApp.Entity;
using ServerApp.Networking;
using ServerApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Controller
{
    internal class OrderController : IController
    {
        private readonly IProductService _productService;
        public OrderController(IProductService productService)
        {
            this._productService = productService;
        }
        public bool OrderProduct(Order order)
        {
            return _productService.OrderProduct(order);
        }
        public object Execute(Request request)
        {
            if (request.action.Equals("Create", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(request.payload))
                {
                    Order order = JsonConvert.DeserializeObject<Order>(request.payload);
                    bool OK = OrderProduct(order);
                    return OK;
                }
                else
                {
                }
            }
            return false;
        }
    }
}
