using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoNamVu_SpringFinalProject.Models
{
    public abstract class Item
    {
        public abstract string Name { get; set; }
        public abstract string ImgUrl { get; set; }
        public abstract int Id { get; set; }
        public abstract int Quantity { get; set; }
        public abstract int TotalPrice { get; set; }
        public override string ToString()
        {
            return Quantity + "x" + Name;
        }
    }
}
