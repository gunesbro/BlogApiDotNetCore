using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProjectAPI.Data.Models
{
    public class Posts
    {
        [Key]
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public string PostDesc { get; set; }
        public string PostCoverImage { get; set; }
        public DateTime PostCreationDate { get; set; } = DateTime.Now; //Deafult
        public DateTime? PostPublishDate { get; set; } = DateTime.Now;
        public bool IsPublished { get; set; }
        public DateTime? PostUpdateDate { get; set; }

        public int LastUpdatedAuthorId { get; set; }

        [ForeignKey("Authors")]
        public int AuthorId { get; set; }
        public virtual Authors Authors { get; set; }
    }
}
