using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoNamVu_SpringFinalProject.Models
{
     class Card
    {
        public string nameOnCard { get; set; }
        public double cardNumber { get; set; }
        public int mmyy { get; set; }
        public int cvc { get; set; }
        public override string ToString()
        {
            return "NameOnCard: " + nameOnCard +"  -  CardNumber: " + cardNumber +"  -  MM/YY: " + mmyy + "  -  CVC: "+cvc;
        }
    }
}
