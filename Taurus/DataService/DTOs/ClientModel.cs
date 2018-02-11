using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataService.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string Description { get; set; }

        public string MobileNumber { get; set; }
        public string Email { get; set; }
    }

    public class CreateNewClientRequestModel
    {
        public AddressModel Address { get; set; }
        public ClientModel Client { get; set; }
        public int PropertyId { get; set; }
    }


}