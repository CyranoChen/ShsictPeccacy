using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Mvc.Models
{
    public class HomeModels
    {
        public class ConfigManagementDto
        {
            public List<Config> Configs { get; set; }
        }
    }
}