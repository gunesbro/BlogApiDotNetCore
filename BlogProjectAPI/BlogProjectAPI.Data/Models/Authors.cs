using System;
using System.ComponentModel.DataAnnotations;

namespace BlogProjectAPI.Data.Models
{
    public class Authors
    {
        [Key]
        public int AuthorId { get; set; }
        public string AuthorUsername { get; set; }
        public string AuthorPassword { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string AuthorAvatar { get; set; }
        public string AuthorSortDesc { get; set; }
        public DateTime AuthorCreationDate { get; set; }
    }
}
