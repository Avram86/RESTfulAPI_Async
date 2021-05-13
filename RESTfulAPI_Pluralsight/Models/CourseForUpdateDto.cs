using RESTfulAPI_Pluralsight.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAPI_Pluralsight.Models
{
   
    public class CourseForUpdateDto
    {
        [Required(ErrorMessage = "You should fill out the title!")]
        [MaxLength(100, ErrorMessage = "Title must be less than 100 characters!")]
        public string Title { get; set; }

        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
    }
}
