

using EF.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataService.Models
{


    public class ResetPasswordDTO
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
    }


}