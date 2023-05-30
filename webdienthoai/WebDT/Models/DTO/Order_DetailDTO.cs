using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDT.Models.DTO
{
    public class Order_DetailDTO
    {
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Amount { get; set; }

        public string Product_Name { get; set; }
    }
}