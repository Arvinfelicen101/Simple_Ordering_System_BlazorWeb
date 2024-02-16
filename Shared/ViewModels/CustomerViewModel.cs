using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Shared.ViewModels
{
    public class CustomerViewModel
    {
        public string? CustCode { get; set; }
        public string? FullName { get; set; }

        public string? Fname { get; set; }
        public string? Mname { get; set; }
        public string? Lname { get; set; }
        public string? BillAddress { get; set; }
        public string? ShipAddress { get; set; }
        public string? Email { get; set; }
        public string MobileNum { get; set; }
        public string HomeNum { get; set; }


        public static CustomerViewModel FromCustomer(Customer customer)
        {
            return new CustomerViewModel
            {
                CustCode = customer.CustCode,
                FullName = $"{customer.Fname} {customer.Lname}",
                Fname = customer.Fname,
                Mname = customer.Mname,
                Lname = customer.Lname,
                BillAddress = customer.BillAddress,
                ShipAddress = customer.ShipAddress,
                Email = customer.Email,
                MobileNum = customer.MobileNum,
                HomeNum = customer.HomeNum
            };
        }

        public CustomerViewModel
            ()
        { }
        public CustomerViewModel(string custCode, string fname, string lname, string mname,
                                 string billAddress, string shipAddress, string email,
                                 string mobileNum, string homeNum)
        {
            CustCode = custCode;
            Fname = fname;
            Lname = lname;
            Mname = mname;
            BillAddress = billAddress;
            ShipAddress = shipAddress;
            Email = email;
            MobileNum = mobileNum;
            HomeNum = homeNum;
        }
    }
}
