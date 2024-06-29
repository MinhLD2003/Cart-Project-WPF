using ServerApp.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Controller
{
    public interface IController
    {
        public Object Execute(Request request); 
    }
}
