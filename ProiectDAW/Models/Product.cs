using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW.Models
{
    public class Product
    {
        [Key]
        public int ProdusID { get; set; }

        [Required(ErrorMessage = "Titlu este obligatoriu!")]
        [StringLength(100, ErrorMessage = "Titlul nu poate avea mai mult de 100 de caractere!")]
        public string Titlu { get; set; }

        [Required(ErrorMessage = "Descrierea este obligatorie!")]
        [DataType(DataType.MultilineText)]
        public string Descriere { get; set; }

        [Required(ErrorMessage = "Pretul este obligatoriu")]
        [Range(0.01, float.MaxValue, ErrorMessage = "Pretul nu este valid")]
        public float Pret { get; set; }

        //System.Drawing.Image
        [DisplayName("Upload File")]
        public string ImagePath { get; set; }

        public decimal Rating { get; set; }


        [Required(ErrorMessage = "Categoria este obligatorie!")]
        public int CategorieID { get; set; }
        public virtual Category Category { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

       
        public virtual ICollection<Review> Reviews { get; set; }

        public IEnumerable<SelectListItem> Categ { get; set; }

        public bool needsApproval { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}