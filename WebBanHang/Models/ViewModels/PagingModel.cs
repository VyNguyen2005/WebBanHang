using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Models.ViewModels
{
    public class PagingModel
    {
        public List<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }
        public Func<int?, string> GenerateUrl { get; set; }
    }
}
