using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrderingSystem.Shared
{
    public class Customer
    {
        [Key]
        public string CustCode { get; set; }
        public string? Fname { get; set; }
        public string? Mname { get; set; }
        public string? Lname { get; set; }

        public string? FullName => $"{Fname} {Lname}";
        public string BillAddress { get; set; }
        public string ShipAddress { get; set; }
        public string Email { get; set; }
        public string? MobileNum { get; set; }
        public string? HomeNum { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();


    }
}
