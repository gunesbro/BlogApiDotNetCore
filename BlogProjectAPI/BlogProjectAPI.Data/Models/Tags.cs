using System.ComponentModel.DataAnnotations;

namespace BlogProjectAPI.Data.Models
{
    public class Tags
    {
        [Key]
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}
