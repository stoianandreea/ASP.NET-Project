using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkinCareShop.Models.MyValidation;

namespace SkinCareShop.Models {
    public class Product {
        [Key]
        public int ProductId { get; set; }

        [UPCValidation]
        public string Upc { get; set; }

        [MinLength(2, ErrorMessage = "Name cannot be less than 2!"),
        MaxLength(100, ErrorMessage = "Name cannot be more than 100!")]
        public string Name { get; set; }

        [MinLength(2, ErrorMessage = "Description cannot be less than 2!"),
        MaxLength(10000, ErrorMessage = "Description cannot be more than 10000!")]
        public string Description { get; set; }
        [Range(0, 1000, ErrorMessage = "Enter Quantity between 0 to 1000")]
        public int Quantity { get; set; }

        // one-to-many relationship with Manufacturer
        [ForeignKey("Manufacturer")]
        public int ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> ManufacturersList { get; set; }
        // one-to-many relationship with Category
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> CategoriesList { get; set; }

        // many to many relationship with User (ApplicationUser)
        public virtual ICollection<ApplicationUser> Users { get; set; }

    }
}