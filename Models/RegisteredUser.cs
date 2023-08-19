using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoNamVu_SpringFinalProject.Models
{
    class RegisteredUser : User
    {
        public override string FullName { get; set; }
        public override string UserAddress { get; set; }
        public override string PostalCode { get; set; }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        public override bool hasAuthority()
        {
            return false;
        }
    }
}
