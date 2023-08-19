using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoNamVu_SpringFinalProject.Models
{
    class UnregisteredUser : User
    {
        public override string FullName { get; set; }
        public override string UserAddress { get; set; }
        public override string PostalCode { get; set; }

        public override bool hasAuthority()
        {
            return false;
        }
    }
}
