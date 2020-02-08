using System;
using System.Collections.Generic;
using System.Text;
using BlogProjectAPI.Data.Models;

namespace BlogProjectAPI.DAL.Abstract
{
    public class ListModel<T, TState> where  T:class where TState:ProccessState
    {
        public List<T> TEntity { get; set; }
        public  TState  State { get; set; }
    }
}
