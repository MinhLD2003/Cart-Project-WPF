using Microsoft.Extensions.DependencyInjection;
using ServerApp.Controller;

namespace ServerApp.Networking
{
    public class Route
    {
        public string controller { get; set; }
        public string action { get; set; }

        public Route(string controller, string action)
        {
            this.controller = controller;
            this.action = action;
        }
    }
    public class RoutingHandler
    {
        private readonly List<Route> _routes;
        private readonly IServiceProvider _serviceProvider;
        public RoutingHandler(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            _routes = new List<Route>
            {
                new Route("Product", "Read"),
                new Route("Order" , "Create")
            };
        }

        public IController MatchRoute(Request request)
        {
            string controllerName = request.path;
            string actionName = request.action;
            var route = _routes.Find(r => r.controller.Equals(controllerName, StringComparison.OrdinalIgnoreCase)
                                           && r.action.Equals(actionName, StringComparison.OrdinalIgnoreCase));
            if (route != null)
            {
                IController controller = CreateControllerInstance(route.controller);
                if (controller != null)
                {
                    return controller;
                }
            }

            return null;
        }

        private IController CreateControllerInstance(string controllerName)
        {
            switch (controllerName.ToLower())
            {
                case "product":
                    return _serviceProvider.GetService<ProductController>();
                case "order":
                    return _serviceProvider.GetService<OrderController>();
                default:
                    return null;
            }
        }
    }
}
