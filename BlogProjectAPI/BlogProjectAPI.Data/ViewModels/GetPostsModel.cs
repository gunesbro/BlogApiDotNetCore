using System;
using System.Collections.Generic;
using System.Text;

namespace BlogProjectAPI.Data.ViewModels
{
    public class GetPostsModel
    {
        public string? OrderBy { get; set; }
        public string? SortBy { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
