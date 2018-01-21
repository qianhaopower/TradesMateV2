using EF.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataService.Models
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }


        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }


        public string ConfirmPassword { get; set; }

        [Required]
        public int UserType { get; set; }

        public String CompanyName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public String Email { get; set; }

        public string UserId { get; set; }

        public bool IsContractor { get; set; }

        public List<TradeType> TradeTypes { get; set; }

        public bool PasswordAllocated {get;set;}







    }


}