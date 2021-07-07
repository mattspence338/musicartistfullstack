using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace music_artist_full_stack.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(28, MinimumLength = 28)]
        public string FirebaseUserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [DisplayName("UserType")]
        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }

        [DataType(DataType.Url)]
        [MaxLength(255)]
        public string ProfilePhoto { get; set; }

        public DateTime DateCreated { get; set; }
    }
}

