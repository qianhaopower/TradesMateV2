using EF.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataService.Models
{
    public class UpdateMemberServiceTypeModel
    {
        public int MemberId { get; set; }
        public List<TradeType> SelectedTypes { get; set; }

    }
}