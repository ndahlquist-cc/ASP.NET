/* NDMemberMetadata.cs
 * 
 * This is a partial class for the members metadata model
 * 
 * Nicole Dahlquist - Section 3
 * 
 * Created: Nov 7, 2014
 * + Add display name annotations
 * Nov 11, 2014
 * + Add required annotations
 * Nov 12, 2014
 * + Add self-validations
 * + Test validations
 * Nov 13, 2014
 * +Add comments 
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using NDSailing.App_GlobalResources;
using NDClassLibrary;
using System.Text.RegularExpressions;
//using System.ComponentModel.DataAnnotations.ValidationAttribute;


//using NDClassLibrary.Annotations;

namespace NDSailing.Models
{
    /// <summary>
    /// This class provides a way to edit the metadata without affecting
    /// the original metadata file
    /// </summary>
    [MetadataType(typeof(NDMemberMetadata))]
    public partial class member : IValidatableObject
    {
        private static sailSQLContext db = new sailSQLContext();

        /// <summary>
        /// Model self-validation
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // full name validation
            if (!string.IsNullOrWhiteSpace(spouseFirstName) && (!string.IsNullOrWhiteSpace(spouseLastName) && spouseLastName == lastName) 
                || !string.IsNullOrWhiteSpace(spouseFirstName) && string.IsNullOrWhiteSpace(spouseLastName))
            {
                fullName = lastName + ", " + firstName + " & " + spouseFirstName;
            }
            else if (!string.IsNullOrWhiteSpace(spouseFirstName)  && !string.IsNullOrWhiteSpace(spouseLastName) && spouseLastName != lastName)
            {
                fullName = lastName + ", " + firstName + " & " + spouseLastName + ", " + spouseFirstName;
            }
            else if (string.IsNullOrWhiteSpace(spouseFirstName) && !string.IsNullOrWhiteSpace(spouseLastName))
            {
                yield return new ValidationResult(string.Format(NDTranslations.spouseFirstRequired, NDTranslations.spouseFirstName, NDTranslations.spouseLastName), new[] { "spouseFirstName" });
            }
            else
            {
                fullName = lastName + ", " + firstName;
            }
            // postal code validation
            if (!string.IsNullOrWhiteSpace(postalCode))
            {
                Regex pattern = new Regex(@"^[ABCEGHJKLMNPRSTVXY]\d[A-Z] ?\d[A-Z]\d$", RegexOptions.IgnoreCase);
                if (pattern.IsMatch(postalCode.ToString()) && postalCode.Trim().Length > 5 && postalCode.Trim().Length < 8)
                {
                    postalCode = postalCode.Trim();
                    postalCode = postalCode.ToUpper();
                    if (postalCode.Length == 6)
                    {
                        postalCode = postalCode.Substring(0, 3) + " " + postalCode.Substring(3, 3);
                    }
                }
            }
            else
            {
                yield return new ValidationResult(string.Format(NDTranslations.postalRequiredFormat, NDTranslations.postalCode), new[] { "postalCode" });
            }
            
            // province code validation
            var provinceCodeValid = db.provinces.Find(provinceCode);

            if (!string.IsNullOrWhiteSpace(provinceCode))
            {
                if (provinceCode.Length != 2)
                {
                    yield return new ValidationResult(NDTranslations.provinceLength, new[] { "provinceCode" });
                }
                if (provinceCodeValid == null)
                {
                    yield return new ValidationResult(NDTranslations.provinceInvalid, new[] { "provinceCode" });
                }
            }
            else
            {
                yield return new ValidationResult(string.Format(NDTranslations.Required, NDTranslations.provinceCode), new[] { "provinceCode" });
            }
            // use canada post validations
            if (useCanadaPost == true)
            {
                if (string.IsNullOrWhiteSpace(street))
                {
                    yield return new ValidationResult(string.Format(NDTranslations.Required, NDTranslations.street), new[] { "street" });
                }
                if (string.IsNullOrWhiteSpace(city))
                {
                    yield return new ValidationResult(string.Format(NDTranslations.Required, NDTranslations.city), new[] { "city" });
                }
            }
            else if (useCanadaPost == false)
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    yield return new ValidationResult(string.Format(NDTranslations.Required, NDTranslations.email), new[] { "email" });
                }
            }
            // emails validation
            if (!string.IsNullOrWhiteSpace(email))
            {
                Regex emailPattern = new Regex(@"[\w][\w.\s]*@([\w\s][\w.\s]*[\w\s])[.][a-z][a-z]+$", RegexOptions.IgnoreCase);
                if (!emailPattern.IsMatch(email.ToString()))
                {
                    yield return new ValidationResult(string.Format(NDTranslations.emailFormat, NDTranslations.email), new[] { "email" });
                }
            }
        }   
    }

    /// <summary>
    /// Metadata where Data annotations may be added
    /// </summary>
    public class NDMemberMetadata
    {
        [Required(ErrorMessageResourceType = typeof(NDTranslations), ErrorMessageResourceName = "integerEmpty")]
        [Display(Name="memberId", ResourceType=typeof(NDTranslations))]
        public int memberId { get; set; }

        [Display(Name = "fullName", ResourceType = typeof(NDTranslations))]
        public string fullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(NDTranslations), ErrorMessageResourceName = "Required")]
        [Display(Name = "firstName", ResourceType = typeof(NDTranslations))]
        public string firstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(NDTranslations), ErrorMessageResourceName = "Required")]
        [Display(Name = "lastName", ResourceType = typeof(NDTranslations))]
        public string lastName { get; set; }

        [Display(Name = "spouseFirstName", ResourceType = typeof(NDTranslations))]
        public string spouseFirstName { get; set; }

        [Display(Name = "spouseLastName", ResourceType = typeof(NDTranslations))]
        public string spouseLastName { get; set; }

        [Display(Name = "street", ResourceType = typeof(NDTranslations))]
        public string street { get; set; }

        [Display(Name = "city", ResourceType = typeof(NDTranslations))]
        public string city { get; set; }

        
        [Display(Name = "provinceCode", ResourceType = typeof(NDTranslations))]
        public string provinceCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(NDTranslations), ErrorMessageResourceName = "Required")]
        [NDPostalCodeValidation(ErrorMessageResourceType= typeof(NDTranslations),
            ErrorMessageResourceName="postalError")]
        [Display(Name = "postalCode", ResourceType = typeof(NDTranslations))]
        public string postalCode { get; set; }

        [RegularExpression(@"^\d{3}-\d{3}-\d{4}", ErrorMessageResourceType = typeof(NDTranslations), 
            ErrorMessageResourceName= "phoneError")]
        [Display(Name = "homePhone", ResourceType = typeof(NDTranslations))]
        public string homePhone { get; set; }

        [Display(Name = "email", ResourceType = typeof(NDTranslations))]
        public string email { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(NDTranslations),
            ErrorMessageResourceName = "integer")]
        [Display(Name = "yearJoined", ResourceType = typeof(NDTranslations))]
        public Nullable<int> yearJoined { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "comment", ResourceType = typeof(NDTranslations))]
        public string comment { get; set; }

        [Display(Name = "taskExempt", ResourceType = typeof(NDTranslations))]
        public Nullable<bool> taskExempt { get; set; }

        [Display(Name = "useCanadaPost", ResourceType = typeof(NDTranslations))]
        public Nullable<bool> useCanadaPost { get; set; }

        public virtual ICollection<boat> boats { get; set; }

        
        [Display(Name = "province", ResourceType = typeof(NDTranslations))]
        public virtual province province { get; set; }
        public virtual ICollection<membership> memberships { get; set; }
        public virtual ICollection<memberTask> memberTasks { get; set; }
    }
}