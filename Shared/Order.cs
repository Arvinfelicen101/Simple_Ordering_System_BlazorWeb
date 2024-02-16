using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Shared
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerCustCode { get; set; }
        public virtual Customer? Customer { get; set; }

        public string? ProductProdCode { get; set; }

        public virtual Product? Product { get; set; }

        public int Qty { get; set; }
       
    }
}
