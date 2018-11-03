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


    public class CreateNewClientBulkModel
    {
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessStreet { get; set; }
        public string BusinessCity { get; set; }
        public string BusinessState { get; set; }
        public string BusinessPostalCode { get; set; }
        public string BusinessCountry { get; set; }
        public string HomeStreet { get; set; }
        public string HomeCity { get; set; }
        public string HomeState { get; set; }
        public string HomePostalCode { get; set; }
        public string BusinessPhone { get; set; }
        public string BusinessFax { get; set; }
        public string MobilePhone { get; set; }
        public string Categories { get; set; }
        public string HomePhone { get; set; }
        public string EmailAddress { get; set; }
        public string WebPage { get; set; }
    }

}