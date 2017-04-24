using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MobileApi.Server.Models
{
    public class ProfileViewModel
    {
        [Display(Name = "First Name")]
        [StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Display(Name = "Second Name")]
        [StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string SecondName { get; set; }

        [StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Patronym { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        [StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string Email { get; set; }

        [DataType(DataType.ImageUrl)]
        public string AvatarLink { get; set; }
        
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
    }
}