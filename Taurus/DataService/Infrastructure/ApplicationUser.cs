using EF.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace DataService.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// 0 Cient
        /// 1 TradeMan
        /// </summary>
        [Required]
        public UserType UserType { get; set; }


        //[Required]
        //public int CompanyId { get; set; }

      


        public DateTime JoinDate { get; set; }


        //[ForeignKey("UserId")]
        public virtual Client Client { get; set; }

        public virtual Member Member { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here

            return userIdentity;
        }
    }
}