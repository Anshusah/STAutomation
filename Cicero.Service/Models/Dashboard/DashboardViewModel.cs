using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class DashboardViewModel
    {
        public string AppName { get; set; }

        public double TotalUsers { get; set; }

        public double TotalArticles { get; set; }

        public List<ArticleViewModel> Articles { get; set; }

        public List<DashboardUserViewModel> Users { get; set; }

        public double NewClaims { get; set; }

        public double TotalClaims { get; set; }

        public List<ChartDefinitionModel> ChartDefinition { get; set; }

        public List<ActivityLogViewModel> LastFourActivities {get;set;}
    }
}
