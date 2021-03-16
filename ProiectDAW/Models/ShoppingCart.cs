using ProiectDAW.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW.Models
{
    public class ShoppingCart
    {
        [Key]
        public int ShoppingCartID { get; set; }

        //public int UserId { get; set; }

        //public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Product> Products { get; set; }
       

    }
}