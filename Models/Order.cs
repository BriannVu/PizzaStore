using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoNamVu_SpringFinalProject.Models
{
    public class Order
    {
        public int orderId { get; set; }
        public int userId { get; set; }
        public DateTime orderDate { get; set; }
        public string orderDetails { get; set; }
        public string orderAddress { get; set; }
        public double orderTotalAmount { get; set; }

    }
}
