using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Model
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        public int OrderId { get; set; }
        public string? CustomerId { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
