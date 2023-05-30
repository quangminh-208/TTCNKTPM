using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDT.Models.EF;

namespace WebDT.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int actual_number { get; set; }
    }

   
}