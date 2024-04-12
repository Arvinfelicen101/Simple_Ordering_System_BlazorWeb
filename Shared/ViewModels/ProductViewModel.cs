using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Shared.ViewModels
{
    public class ProductViewModel
    {
        public string? ProdCode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

      
        public ProductViewModel()
        {
            
        }

        public ProductViewModel(string prodCode, string name, decimal price)
        {
            ProdCode = prodCode;
            Name = name;
            Price = price;
        }
    }
}
