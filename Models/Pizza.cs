using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoNamVu_SpringFinalProject.Models
{
    class Pizza : Item
    {
        public override string Name { get; set; }
        public override string ImgUrl { get; set; }
        public override int Id { get; set; }
        public override int Quantity { get; set; }
        public override int TotalPrice { get; set; }

        public string Topping { get; set; }
        public string Size { get; set; }
        public override string ToString()
        {
            return Quantity + "x" + Name + "-" + Size + "-" + Topping;
        }
    }
}
