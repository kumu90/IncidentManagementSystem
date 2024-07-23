using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IncidentManagementSystem.Model.Annotation
{
    public class ImageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is HttpPostedFileBase file))
            {
                return new ValidationResult("Please provide a file.");
            }

            try
            {
                string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(file.FileName);
                if (!allowedExtensions.Contains(fileExtension.ToLower()))
                {
                    return new ValidationResult($"The selected file ({fileExtension}) is not Image. Please upload a file with an .jpg, .jpeg, .png, .gif extension.");
                }

                //var allowedMimeTypes = new[] { "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
                //if (!allowedMimeTypes.Contains(file.ContentType))
                //{
                //    return new ValidationResult("Invalid file format.");
                //}

                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
              
            }
            return ValidationResult.Success;
        }
    }
}
