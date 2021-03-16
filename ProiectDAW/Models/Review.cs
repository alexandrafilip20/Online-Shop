using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectDAW.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        [Required(ErrorMessage = "Campul nu poate fi necompletat")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Rating-ul este obligatoriu")]
        [Range(1, 5, ErrorMessage = "Rating-ul nu este valid")]
        public int Nota { get; set; }

        public DateTime Date { get; set; }
        public int ProdusID { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual Product Produs { get; set; }
    }
}