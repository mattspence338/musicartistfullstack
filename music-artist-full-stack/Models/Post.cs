using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace music_artist_full_stack.Models
{
    public class Post
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int PostTypeId { get; set; }
        public PostType PostType { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [MaxLength(255)]
        public string Url { get; set; }

        public List<Comment> Comments { get; set; }

    }
}
