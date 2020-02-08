using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProjectAPI.Data.Models
{
    public class TagsOfPosts
    {
        [Key]
        public int TagsOfPostsId { get; set; }
        [ForeignKey("Posts")]
        public int PostId { get; set; }
        public virtual Posts Posts { get; set; }
        [ForeignKey("Tags")]
        public int TagId { get; set; }
        public virtual Tags Tags { get; set; }

    }
}
