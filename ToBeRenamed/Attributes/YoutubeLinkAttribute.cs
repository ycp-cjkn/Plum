using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ToBeRenamed.Attributes
{
    public class YoutubeLinkAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string link = "";
            
            try
            {
                link = (string) validationContext.ObjectInstance;
            }
            catch (Exception)
            {
                return new ValidationResult(getErrorMessage());
            }
            

            var linkParts = link.Split('=');
            var videoIdentifier = linkParts.Last();
            
            if (!link.Contains("youtube") || videoIdentifier.Length != 11)
            {
                return new ValidationResult(getErrorMessage());
            }

            return ValidationResult.Success;
        }

        private string getErrorMessage()
        {
            return "Invalid link. Make sure that a valid Youtube link is used.";
        }
    }
}