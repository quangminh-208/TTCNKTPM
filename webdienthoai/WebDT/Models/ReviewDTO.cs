using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDT.Models
{
    public class ReviewDTO
    {
        public int id { get; set; }
        public string Content { get; set; }
        public Nullable<int> Rating { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> User_id { get; set; }
        public Nullable<int> Product_id { get; set; }
        public Nullable<bool> Status { get; set; }

        public string Fullname { get; set; }

    }
}