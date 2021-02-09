using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkinCareShop.Models {
    public class Category {
        [Key]
        public int CategoryId { get; set; }

        [Required,
            MinLength(2, ErrorMessage = "Category cannot be less than 2!"),
            MaxLength(50, ErrorMessage = "Category cannot be more than 50!")]
        public string Name { get; set; }

        // many-to-one relationship with Product
        public virtual ICollection<Product> Products { get; set; }
    }
}