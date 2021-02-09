using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkinCareShop.Models {
    public class ManufacturerAddressViewModel {
        [RegularExpression(@"^[A-Z][a-zA-Z0-9 ]*$", ErrorMessage = "This is not a valid Name!")]
        public string Name { get; set; }

        [RegularExpression(@"^07(\d{8})$", ErrorMessage = "This is not a valid phone number!")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "This is not a valid email!")]
        public string Email { get; set; }

        [Required, RegularExpression(@"^[A-Z][a-zA-Z ]*$", ErrorMessage = "This is not a valid country!")]
        public string Country { get; set; }

        [Required, RegularExpression(@"^[A-Z][a-zA-Z ]*$", ErrorMessage = "This is not a valid city!")]
        public string City { get; set; }

        [Required, RegularExpression(@"^[A-Z][a-zA-Z ]*[0-9]*$", ErrorMessage = "This is not a valid street!")]
        public string StreetNumber { get; set; }

    }
}