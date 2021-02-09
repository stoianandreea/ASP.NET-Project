using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkinCareShop.Models {
    public class Address {
        [Key]
        public int AdressId { get; set; }

        [Required, RegularExpression(@"^[A-Z][a-zA-Z ]*$", ErrorMessage = "This is not a valid country!")]
        public string Country { get; set; }

        [Required, RegularExpression(@"^[A-Z][a-zA-Z ]*$", ErrorMessage = "This is not a valid city!")]
        public string City { get; set; }

        [Required, RegularExpression(@"^[A-Z][a-zA-Z ]*[0-9]*$", ErrorMessage = "This is not a valid street!")]
        public string StreetNumber { get; set; }

        // one-to-one relationship with Manufacturer
        public virtual Manufacturer Manufacturer { get; set; }
    }
}