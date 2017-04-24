using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileApi.Server.Core.Domain
{
    public class UserProfile
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Patronym { get; set; }

        public string Email { get; set; }

        public string AvatarLink { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? DateOfBirth { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Key, ForeignKey("User")]
        public string UserId { get; set; }
    }
}
