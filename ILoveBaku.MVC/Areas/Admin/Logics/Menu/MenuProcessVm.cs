using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Menu
{
    public class MenuProcessVm
    {
        public bool IsUpdate { get; set; }
        public int MenuId { get; set; }
        public int ParentId { get; set; }
    }
}
