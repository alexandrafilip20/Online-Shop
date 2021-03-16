using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectDAW.Models
{
    public class Category
    {
        [Key]
        public int CategorieID { get; set; }

        [Required(ErrorMessage = "Numele categoriei este obligatoriu!")]
        public string CategorieNume { get; set; }

        public virtual ICollection<Product> Produse { get; set; }
    }
}