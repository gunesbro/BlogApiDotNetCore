using System.ComponentModel.DataAnnotations;

namespace BlogProjectAPI.Data.Models
{
    public class ExceptionLogs
    {
        [Key]
        public int ExceptionLogsId { get; set; }
        public string ExceptionFile { get; set; }
        public string ExceptionMethod { get; set; }
        public string ExceptionLine { get; set; }
        public string ExceptionMessage { get; set; }

    }
}
