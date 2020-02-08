using System.ComponentModel.DataAnnotations;

namespace BlogProjectAPI.Data.Models
{
    public class TokenAccessLogs
    {
        [Key]
        public int TokenAccessLogsId { get; set; }
        public string Access { get; set; }
        public bool AccessTrueFalse { get; set; }
        public string AccessRequest { get; set; }

    }
}
