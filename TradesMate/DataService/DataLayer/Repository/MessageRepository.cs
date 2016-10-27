
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

    public class MessageRepository : IDisposable
    {
        private EFDbContext _ctx;
      

        private UserManager<ApplicationUser> _userManager;

        //XXX(Admin name) has assigned you default role in YYY(company name), now you can view all YYY properties and related works
        private const string AssignDefaultRoleMessage = "Dear {0}, You are assigned default role in {1}. Now you can view all {1}'s properties and related works.";

        //XXX(Admin name) want to assign you defaut role in YYY(company name), if you accept you will become contractor in ZZZ,WWW and XXX(other company name).
        private const string AssignDefaultRoleRequestMessage = "Dear {0}, {1} want to assign you defaut role, if you accept your role in {2} will become contractor.";

        //XXX(Admin name) has assigned you contractor role in YYY(company name), now you can view YYY's properties and related works allocated to you.
        private const string AssignContractorRoleMessage = "Dear {0}, You are assigned contractor role in {1}. Now you can view {1}'s properties allocated to you.";

        //XXX(Admin name) has invited you to join YYY(company name).
        private const string InviteJoinCompanyRequestMessage = "Dear {0}, you are invited to join {1}, you can see {1}'s properties if you accept.";

        //AAA(ClientName) has granted you access to property WWW(propertyName, address)
        private const string AddPropertyCoOwnerMessage = "Dear {0}, you are granted access to {1}, you can see {1}'s works now.";

        public MessageRepository()
        {
            _ctx = new EFDbContext();
        }

        //this create the MessageResponse entity
        public MessageResponse GenerateResponse(int messageId, ResponseAction action)
        {
            var message = _ctx.Messages.Find(messageId);

            var response = new MessageResponse()
            {
                ResponseText = null,// do not allow responseText yet.
                MessageId = messageId,
                ResponseAction = action,
                UserIdFrom = message.UserIdTo,
                UserIdTo = message.UserIdFrom,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,

            };

            _ctx.Entry<MessageResponse>(response).State = EntityState.Added;
            _ctx.SaveChanges();
            return response;
        }

        //this do the action after the 
        public void HandleMessageResponse(MessageResponse response)
        {
            var message = _ctx.Messages.Find(response.MessageId);
            switch (message.MessageType)
            {
                //nothing to handle
                case MessageType.AssignDefaultRole:
                case MessageType.AddPropertyCoOwner:
                case MessageType.AssignContractorRole:
                case MessageType.WorkRequest:// work request eventually we need set the work item as started if accepted. Once we have the work item status.
                    break;
                case MessageType.AssignDefaultRoleRequest:
                    HandleAssignDefaultRoleResponse(message, response);
                    break;
                case MessageType.InviteJoinCompanyRequest:
                    HandleInviteJoinCompanyResponse(message, response);
                    break;

            }
        }

        private void HandleAssignDefaultRoleResponse(Message message, MessageResponse response)
        {
            message.Pending = false;

        }

        private void HandleInviteJoinCompanyResponse(Message message, MessageResponse response)
        {
            message.Pending = false;

        }



        public void GenerateAssignDefaultRoleMessage(int memberId, int companyId)
        {
            var companyName = _ctx.Companies.Find(companyId).Name;
            var member = _ctx.Members.Find(memberId);
            var memberName = member.FirstName;
            var memberUser  = _ctx.Users.First(p => p.Member == member);

            var adminUser = new CompanyRepository(_ctx).GetCompanyAdminMember(companyId);
           
            var message = new Message()
            {
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                UserIdFrom = adminUser.Id,
                UserIdTo = memberUser.Id,
                MessageText = string.Format(AssignDefaultRoleMessage, memberName, companyName),
                MessageType = MessageType.AssignDefaultRole,
                Pending = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }
       


        public void GenerateAssignDefaultRoleRequestMessage(int memberId, int companyId)
        {
            var companyName = _ctx.Companies.Find(companyId).Name;
            var memberName = _ctx.Members.Find(memberId).FirstName;
            var companyRepo = new CompanyRepository(_ctx);
         
            var otherCompanyNames = companyRepo.GetMemberInfoOutsideCompany(companyId, memberId)
                .Where(p => p.CompanyMember.Role == CompanyRole.Default)
                .Select(p => p.Company)
                .ToList()
                .Select(p => p.Name)
                .Aggregate((x, y) => x + ", " + y);
            var message = new Message()
            {
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                MessageText = string.Format(AssignDefaultRoleRequestMessage, memberName, companyName, otherCompanyNames),
                MessageType = MessageType.AssignDefaultRoleRequest,
                Pending = true,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }

       





        public void GenerateAssignContractorRoleMessage(int memberId, int companyId)
        {
            var companyName = _ctx.Companies.Find(companyId).Name;
            var memberName = _ctx.Members.Find(memberId).FirstName;
           
            var message = new Message()
            {
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                MessageText = string.Format(AssignContractorRoleMessage, memberName, companyName),
                MessageType = MessageType.AssignContractorRole,
                Pending = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }

        public void GenerateAddMemberToCompany(int memberId, int companyId, CompanyRole role)
        {
            var companyName = _ctx.Companies.Find(companyId).Name;
            var memberName = _ctx.Members.Find(memberId).FirstName;
           
            var message = new Message()
            {
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                Role = role,
                MessageText = string.Format(InviteJoinCompanyRequestMessage, memberName, companyName),
                MessageType = MessageType.InviteJoinCompanyRequest,
                Pending = true,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }


        public void GenerateClientWorkRequest(int clientId, int propertyId, string messageText)
        {

            var message = new Message()
            {
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                PropertyId = propertyId,
                ClientId = clientId,
                MessageText = messageText,
                MessageType = MessageType.WorkRequest,
                Pending = true,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }

        public void GenerateAddPropertyCoClient(int propetyId, int clientId )
        {
            var clientName = _ctx.Clients.Find(clientId).FirstName;
            var property = _ctx.Properties.Find(propetyId).Name;

            var message = new Message()
            {
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                PropertyId = propetyId,
                ClientId = clientId,
                MessageText = string.Format(AddPropertyCoOwnerMessage, clientName, property),
                MessageType = MessageType.AddPropertyCoOwner,
                Pending = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }


        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}