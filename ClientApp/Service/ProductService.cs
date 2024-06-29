using ClientApp.Model;
using ClientApp.Networking;
using Newtonsoft.Json;

namespace ClientApp.Service
{
    public interface IProductService
    {
        Task GetProducts();
        Task<bool> Order(Order order);
        List<Product> GetAllProducts();
    }
    class ProductService : IProductService
    {
        private List<Product> products = null;

        private IClient _client;
        public ProductService(IClient client)
        {
            _client = client;
            products = new List<Product>();
        }
        public List<Product> GetAllProducts()
        {
            return products;
        }
        public async Task GetProducts()
        {
            try
            {
                Request request = new Request(Request.GET, Request.PRODUCT, Request.READ, "");
                await _client.SendRequest(request);

                string response = await _client.ReceiveResponse();

                if (response != null)
                {
                    products = JsonConvert.DeserializeObject<List<Product>>(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());

            }
        }

        public async Task<bool> Order(Order order)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(order);
                Request req = new Request(Request.POST, Request.ORDER, Request.CREATE, jsonString);
                Console.WriteLine("Sending Request: " + JsonConvert.SerializeObject(req));

                await _client.SendRequest(req); 

                string response = await _client.ReceiveResponse(); 
                Console.WriteLine("Received Response: " + response);

                if (response != null && response.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Response did not match 'true'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

            return false;
        }

    }
}
