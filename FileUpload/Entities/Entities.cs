﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public  virtual ICollection<Order> Orders  { get; set; }
    }
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Customer Customer  { get; set; }
    }

}
