﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataService.Models
{
    public class ClientSearchModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public int ClientId { get; set; }

    }


}