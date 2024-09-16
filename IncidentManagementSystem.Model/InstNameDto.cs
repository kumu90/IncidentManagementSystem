﻿using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IncidentManagementSystem.Model.Annotation;

namespace IncidentManagementSystem.Model
{
    public class InstNameDto
    {
        public string Id { get; set; }

        [DisplayName("Institution Id")]
        public string InstId { get; set; }

        [DisplayName("Institution Name")]
        [Required]
        public string InstitutionName { get; set; }

        [DisplayName("Country")]
        [Required]
        public string Country { get; set; }

        [DisplayName("State")]
        [Required]
        public string State { get; set; }

        [DisplayName("Address")]
        [Required]
        public string Address { get; set; }

        [DisplayName("ZipCode")]
        [Required]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid ZIP Code format.")]
        public string ZipCode { get; set; }

        [DisplayName("Contact Admin")]
        public string ContactPersonAdmin { get; set; }

        [DisplayName("Contact Technical")]
        public string ContactPersonTechnical { get; set; }

        [DisplayName("Contact Number")]
        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string ContactNumber { get; set; }

        [DisplayName("Email")]
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [DisplayName("Image")]
        [Image]
        //[Required(ErrorMessage ="Uplode Image Less Then 200Kb")]
        public string ImageUrl { get; set; }
        public byte[] ImageData {  get; set; }
        public string contentType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string Flag { get; set; }
        [Required(ErrorMessage = "Please select at least one service.")]
        public List<string> ServiceIdList { get; set; }

        public int TotalCount { get; set; }

        
    }

    public class InstListDto
    {
        public InstListDto()
        {
            InstList = new List<InstNameDto>();
        }

        public List<InstNameDto> InstList { get; set; }
        public int TotalCount { get; set; }
    }
   
}
