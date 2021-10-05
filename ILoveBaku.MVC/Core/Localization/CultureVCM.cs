using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Core.Localization
{
    public class CultureVCM
    {
        //public SelectList Cultures { get; set; }

        public string ReturnUrl { get; set; }

        #region for doesn't work TagHelper
        public List<Culture> Cultures { get; set; }

        public string CurrentCulture { get; set; }
        #endregion
    }
}
