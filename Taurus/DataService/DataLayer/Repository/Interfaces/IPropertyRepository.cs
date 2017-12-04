
using DataService.Entities;
using DataService.Infrastructure;
using DataService.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace EF.Data
{

    public interface IPropertyRepository
    {



        Property CreatePropertyForClient(string userName, PropertyModel model);

        void DeleteProperty(string username, int key);

        void CreateDefaultSections(DefaultPropertySection model, Property parentProperty, EFDbContext context);

        string GetSectionNameFromEnum(SectionType type);

        IQueryable<Property> GetPropertyForUser(string userName);

        List<Section> GetPropertySectionList(string name, int key);

        IEnumerable<PropertyReportGroupItem> GetPropertyReportData(int propertyId, string userName);



        IQueryable<Company> GetAllCompanies();

        IQueryable<WorkItem> GetAllPropertyWorkItems(int propertyId);

        AllocationModel UpdateMemberAllocation(string userName, int propertyId, int memberId, bool allocate);

        IQueryable<AllocationModel> GetMemberAllocation(string userName, int memberId);

        IQueryable<Client> GetPropertyOwnerClinet(int propertyId);

        IQueryable<Company> GetCompanyForProperty(int propertyID);

    }
}