using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OkyanusYayincilikWebForm.Models
{
    public class Class1
    {
        public IEnumerable<SelectListItem> City { get; set; }
        public IEnumerable<SelectListItem> Town{ get; set; }
        public IEnumerable<SelectListItem> Institution { get; set; }
        public IEnumerable<SelectListItem> Product { get; set; }
        public Orders orders { get; set; }

        
    }
}