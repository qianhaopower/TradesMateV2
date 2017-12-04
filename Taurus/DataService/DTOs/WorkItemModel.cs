

using EF.Data;
using System;
using System.Collections.Generic;

namespace DataService.Models
{
    
    public class WorkItemModel
	{
        public int Quantity { get; set; }
        public string Description { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }
        public int WorkItemTemplateId { get; set; }

        public TradeType TradeWorkType { get; set; }
        public string Status { get; set; }

        public int TaskNumber { get; set; }

        public List<string> ImageUrls { get; set; }

        public int SectionId { get; set; }

    }

    public class PropertyReportGroupItem
    {
        public string SectionName { get; set; }

        public List<WorkItemModel> workItems { get; set; }
    }
}