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

    public class ProductController : IController
    {
        private List<Product> _productList;
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            _productList = new List<Product>();
            this.productService = productService;
        }

        public ProductController()
        {
            _productList = new List<Product>();
            
        }

        public void GetProductList()
        {
            _productList = productService.GetProducts();

        }
        public Object Execute(Request request)
        {
            if (request.action.Equals("read", StringComparison.OrdinalIgnoreCase))
            {
                GetProductList();
                if (string.IsNullOrEmpty(request.payload))
                {
                    return _productList;
                }
                else
                {
                }
            }
            return null;
        }
    }
}
