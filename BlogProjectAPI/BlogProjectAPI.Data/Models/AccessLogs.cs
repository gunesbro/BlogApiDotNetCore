using System;
using System.ComponentModel.DataAnnotations;

namespace BlogProjectAPI.Data.Models
{
    public class AccessLogs
    {
        [Key]
        public int AccessLogsId { get; set; }
        public string Path { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public int StatusCode { get; set; }  
        public DateTime Time { get; set; }
        public string QueryString { get; set; }
        public string Host { get; set; }
        public string Scheme { get; set; }
        public string WhoRequested { get; set; }

    }
}
