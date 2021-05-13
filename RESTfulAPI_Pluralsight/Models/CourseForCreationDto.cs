using RESTfulAPI_Pluralsight.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAPI_Pluralsight.Models
{
    [CourseTitlemustBeDifferentFromDescrition(ErrorMessage ="Title must be different from description!")]
    public class CourseForCreationDto
    {
        [Required(ErrorMessage ="You should fill out the title!")]
        [MaxLength(100, ErrorMessage ="Title must be less than 100 characters!")]
        public string Title { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }

        //public Guid AuthorId { get; set; }
        //already have it being sent through routing
    }
}
