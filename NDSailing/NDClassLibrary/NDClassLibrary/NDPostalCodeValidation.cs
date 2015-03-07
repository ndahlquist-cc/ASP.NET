/* NDPostalCodeValidation.cs
 * 
 * This is where the postal code validation occurs
 * 
 * Nicole Dahlquist - Section 3
 * 
 * Created: Nov 7, 2014
 * + CreatedNDPostalCodeValidation
 * + Code regex expression, which compares to value parameter
 * + returns success on matched, empty, and null value
 * Nov 11, 2014
 * + Add NDClassLibrary to NDSailing project
 * Nov 13, 2014
 * + Add comments
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NDClassLibrary
{
    /// <summary>
    /// This class accepts a postalCode entered by the user and compares it to 
    /// the regex postal expression
    /// </summary>
    public class NDPostalCodeValidation : ValidationAttribute
    {
        // Constructor for NDPostalCodeValidation class
        public NDPostalCodeValidation()
            : base()
        {
        }
        
        /// <summary>
        /// This method tests a user-entered postal code against the regex pattern for Canadian
        /// postal codes
        /// </summary>
        /// <param name="value">postal code entered by user</param>
        /// <param name="validationContext">validation context for error message</param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           
                Regex pattern = new Regex(@"^[ABCEGHJKLMNPRSTVXY]\d[A-Z] ?\d[A-Z]\d$", RegexOptions.IgnoreCase);
                if (value == null)
                {
                    return ValidationResult.Success;
                }
                if (pattern.Match(value.ToString().Trim()).Success || value.ToString().Trim() == "")
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            
            
       
    }
}
