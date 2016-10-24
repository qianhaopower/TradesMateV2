using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataService.Models
{
    public class MemberModel
    {
        //[Required]
        //[Display(Name = "User name")]
        //public string UserName { get; set; }


        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }


        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        public string MemberRole { get; set; }

        public int MemberId { get; set; }

        public string Username { get; set; }

    }

   



}