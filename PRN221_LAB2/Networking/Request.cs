using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Networking
{
    public class Request
    {

        public static string GET = "GET";
        public static string POST = "POST";

        public static string READ = "READ";
        public static string CREATE = "CREATE";

        public static string ORDER = "ORDER";
        public static string PRODUCT = "PRODUCT";
        public string requestMethod { get; }
        public string action { get; }
        public string path { get; }
        
        public string payload;

        public Request(string RequestMethod, string path, string action, string payload)
        {
            this.requestMethod = RequestMethod;
            this.path = path;
            this.action = action;
            this.payload = payload;
        }



    }
}
