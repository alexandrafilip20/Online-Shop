using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    public class Item
    {
        public Item() { }

        public Item(Product pr, int q)
        {
            product = pr;
            quantity = q;
        }

        public virtual Product product { get; set; }
        public int quantity { get; set; }
    }
}