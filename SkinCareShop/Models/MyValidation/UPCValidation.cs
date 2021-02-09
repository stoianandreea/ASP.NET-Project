using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SkinCareShop.Models.MyValidation {
    public class UPCValidation : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            var product = (Product)validationContext.ObjectInstance;
            string code = product.Upc;

            if (code != (new Regex("[^0-9]")).Replace(code, "")) {
                // is not numeric
                return new ValidationResult("The UPC is not numeric!");
            }
           
            // calculate check digit
            int[] a = new int[12];
            a[0] = int.Parse(code[0].ToString());
            a[1] = int.Parse(code[1].ToString());
            a[2] = int.Parse(code[2].ToString());
            a[3] = int.Parse(code[3].ToString());
            a[4] = int.Parse(code[4].ToString());
            a[5] = int.Parse(code[5].ToString());
            a[6] = int.Parse(code[6].ToString());
            a[7] = int.Parse(code[7].ToString());
            a[8] = int.Parse(code[8].ToString());
            a[9] = int.Parse(code[9].ToString());
            a[10] = int.Parse(code[10].ToString());
            a[11] = int.Parse(code[11].ToString());

            int oddSum = a[0] + a[2] + a[4] + a[6] + a[8] + a[10];
            oddSum *= 3;
            int evenSum = a[1] + a[3] + a[5] + a[7] + a[9];
            int totalSum = oddSum + evenSum;
            int check = (10 - (totalSum % 10)) % 10;

            bool cond = true;
            if(check != a[11]) {
                cond = false;
            }
            
            return cond ? ValidationResult.Success : new ValidationResult("The UPC is incorrect!");
        }
    }
}
