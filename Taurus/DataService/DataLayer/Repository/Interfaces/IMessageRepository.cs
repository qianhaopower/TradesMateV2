
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
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EF.Data
{

     public interface IMessageRepository 
    {
        IQueryable<Message> GetMessageForUser(string username);

        IQueryable<Client> GetClientThatHasMessageForUser(string username);

        int GetUnReadMessageCountForUser(string username);

        void CreatePropertyForWorkRequest(int messageId, PropertyModel model);

        void MarkMessageAsRead(int messageId, string userId);

        void HandleMessageResponse(int messageId, ResponseAction action);

        void GenerateAssignDefaultRoleMessage(int memberId, int companyId);

        void GenerateAssignDefaultRoleRequestMessage(int memberId, int companyId);

        bool CheckIfThereIsWaitingDefaultRoleRequestMessage(int memberId, int companyId);

        void GenerateAssignContractorRoleMessage(int memberId, int companyId);
        void GenerateAddMemberToCompany(int memberId, int companyId, string messageFromRequestor, CompanyRole role);

        void GenerateClientWorkRequest(MessageDetailModel model, string userId);

        void GenerateAddPropertyCoClient(int propetyId, int clientId);
       
    }
}