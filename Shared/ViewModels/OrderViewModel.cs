﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Shared.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerCustCode { get; set; }
        public string ProductProdCode { get; set; }
        public int Qty { get; set; }

        public decimal Price { get; set; }

        public decimal Disc { get; set; }

        public decimal TotPrice { get; set; }

        public decimal DPrice { get; set; }
        public int RowNo { get; set; }
        public CustomerViewModel Customer { get; set; }
        public ProductViewModel Product { get; set; }

        

    }

}
