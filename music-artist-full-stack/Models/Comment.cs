using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace music_artist_full_stack.Models
{
    public class Comment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [DisplayName("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [DisplayName("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [Required]
        public DateTime CreateDateTime { get; set; }
    }
}
