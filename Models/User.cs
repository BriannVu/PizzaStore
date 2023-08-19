using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoNamVu_SpringFinalProject.Models
{
    public abstract class User
    {
        public abstract string FullName { get; set; }
        public abstract string UserAddress { get; set; }
        public abstract string PostalCode { get; set; }
        public abstract Boolean hasAuthority();
        public override string ToString()
        {
            return "Name: " + FullName + " | Address: " + UserAddress + " - " + PostalCode;
        }
    }
}
