using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainingProject.ViewModels.Test
{
    public class TestViewModel
    {
        //public TestViewModel()
        //{

        //}
        //public TestViewModel(int Id)
        //{
        //    this.Age = 10 + 10;
        //}

        public int Id { get; set; }

        [Display(Name="First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Range(10, 40)]
        public int Age { get; set; }

        [Phone]
        [StringLength(12,ErrorMessage ="Max 12 allow")]
        public string MobileNo { get; set; }

        [EmailAddress]
        public string EmailId { get; set; }
    }


}