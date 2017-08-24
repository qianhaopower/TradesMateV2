

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

        public TradeType TradeWorkType { get; set; }
        public WorkItemStatus Status { get; set; }

        public int TaskNumber { get; set; }

    }

    public class PropertyReportGroupItem
    {
        public string SectionName { get; set; }

        public List<WorkItemModel> workItems { get; set; }
    }


}