using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataService.Models
{
    public class InviteClientModel
    {
        public int ClientId { get; set; }

        public string Text { get; set; }

    }
}