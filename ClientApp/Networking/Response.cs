using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Networking
{
    public class Response
    {
        private string statusCode;
        private string responseData;

        public Response() { }
        public Response(string statusCode, string responseData)
        {
            this.statusCode = statusCode;
            this.responseData = responseData;
        }
    }
}
