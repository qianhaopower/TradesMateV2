
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


        //private UserManager<ApplicationUser> _userManager;

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

        public MessageRepository(EFDbContext ctx = null)
        {
            if (ctx != null)
            {
                _ctx = ctx;
            }
            else
            {
                _ctx = new EFDbContext();
            }
        }


        public IQueryable<Message> GetMessageForUser(string username)
        {
            var user = new AuthRepository(_ctx).GetUserByUserName(username);
            var messages = _ctx.Messages.Where(p => p.UserIdTo == user.Id)
                .Include(p => p.MessageResponse)
                .Include(p => p.Member)
                .Include(p => p.Property)
                .Include(p => p.Client)
                .Include(p => p.Company);

            return messages;
        }

        public int GetUnReadMessageCountForUser(string username)
        {
            var user = new AuthRepository(_ctx).GetUserByUserName(username);
            var count = _ctx.Messages.Include(p=> p.MessageResponse).
                Where(p => (p.UserIdTo == user.Id && p.IsRead == false) // message sent to me but I have not read
                ||(p.UserIdFrom == user.Id && p.MessageResponse != null && p.MessageResponse.IsRead == false)//I send the message, it has a unread response. 
                )
                .Count();
                
            return count;
        }

        public void MarkMessageAsRead(int messageOrResponseId)
        {
            var message = _ctx.Messages.Find(messageOrResponseId);
          
            if (message != null)
            {
                message.IsRead = true;
                _ctx.Entry(message).State = EntityState.Modified;
            }else
            {
                var messageResponse = _ctx.MessageResponses.Find(messageOrResponseId);
                messageResponse.IsRead = true;
                _ctx.Entry(messageResponse).State = EntityState.Modified;
            }
            _ctx.SaveChanges();
                
            
        }

        //this create the MessageResponse entity
        private MessageResponse GenerateResponse(int messageId, ResponseAction action)
        {
            var message = _ctx.Messages.Find(messageId);

            var response = new MessageResponse()
            {
                ResponseText = null,// do not allow responseText yet.
                Id = messageId,
                ResponseAction = action,
                UserIdFrom = message.UserIdTo,
                UserIdTo = message.UserIdFrom,
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                IsRead = false,
            };

            _ctx.Entry<MessageResponse>(response).State = EntityState.Added;
            _ctx.SaveChanges();
            return response;
        }


        public void HandleMessageResponse(int messageId, ResponseAction action)
        {
            var message = _ctx.Messages.Find(messageId);
            if (!message.IsWaitingForResponse)
            {
                throw new Exception("Message already processed.");
            }
            switch (message.MessageType)
            {
                //nothing to handle as when the request is simply a notification for these four cases
                case MessageType.AssignDefaultRole:
                case MessageType.AddPropertyCoOwner:
                case MessageType.AssignContractorRole:
                case MessageType.WorkRequest:// work request eventually we need set the work item as started if accepted. Once we have the work item status.
                    break;
                case MessageType.AssignDefaultRoleRequest:
                    HandleAssignDefaultRoleResponse(message, action);
                    break;
                case MessageType.InviteJoinCompanyRequest:
                    HandleInviteJoinCompanyResponse(message, action);
                    break;

            }
            message.IsWaitingForResponse = false;
            GenerateResponse(messageId, action);
        }

        private void HandleAssignDefaultRoleResponse(Message message, ResponseAction action)
        {
            message.IsWaitingForResponse = false;
            if (action == ResponseAction.Accept)
            {
                //proceed

                //here we don't validate as when the message is generated, validation is done.
                //No illegal message could exist in the Message table as it is not created by the data from client
                //use .value as if there is no value, the data is corrupted and we should crash. 
                new CompanyRepository(_ctx).DoUpdateCompanyMemberRole(message.CompanyId.Value, message.MemberId.Value, message.Role);

            }

        }

        private void HandleInviteJoinCompanyResponse(Message message, ResponseAction action)
        {
            message.IsWaitingForResponse = false;
            if (action == ResponseAction.Accept)
            {
                new CompanyRepository(_ctx).DoMemberJoinCompany(message.CompanyId.Value, message.MemberId.Value);
            }

        }



        public void GenerateAssignDefaultRoleMessage(int memberId, int companyId)
        {
            var companyName = _ctx.Companies.Find(companyId).Name;
            var member = _ctx.Members.Find(memberId);
            var memberName = member.FirstName;
            var memberUser = _ctx.Users.First(p => p.Member.Id == memberId);

            var adminUser = new CompanyRepository(_ctx).GetCompanyAdminMember(companyId);

            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                UserIdFrom = adminUser.Id,
                UserIdTo = memberUser.Id,
                MessageText = string.Format(AssignDefaultRoleMessage, memberName, companyName),
                MessageType = MessageType.AssignDefaultRole,
                IsWaitingForResponse = false,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            //_ctx.SaveChanges();
        }

        public void GenerateAssignDefaultRoleRequestMessage(int memberId, int companyId)
        {
            

            var companyName = _ctx.Companies.Find(companyId).Name;
            var memberName = _ctx.Members.Find(memberId).FirstName;
            var companyRepo = new CompanyRepository(_ctx);

            var memberUser = _ctx.Users.First(p => p.Member.Id == memberId);

            var adminUser = new CompanyRepository(_ctx).GetCompanyAdminMember(companyId);

            var otherCompanyNames = companyRepo.GetMemberInfoOutsideCompany(companyId, memberId)
                .Where(p => p.CompanyMember.Role == CompanyRole.Default)
                .Select(p => p.Company)
                .ToList()
                .Select(p => p.Name)
                .Aggregate((x, y) => x + ", " + y);
            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                UserIdFrom = adminUser.Id,
                UserIdTo = memberUser.Id,
                MessageText = string.Format(AssignDefaultRoleRequestMessage, memberName, companyName, otherCompanyNames),
                MessageType = MessageType.AssignDefaultRoleRequest,
                IsWaitingForResponse = true,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }



        public bool CheckIfThereIsWaitingDefaultRoleRequestMessage(int memberId, int companyId)
        {
            var any = _ctx.Messages.Where(p => p.MemberId == memberId
            && p.CompanyId == companyId
            && p.MessageType == MessageType.AssignDefaultRoleRequest
            && p.IsWaitingForResponse == true).Any();
            return any;

        }

        public void GenerateAssignContractorRoleMessage(int memberId, int companyId)
        {
            var memberUser = _ctx.Users.First(p => p.Member.Id == memberId);

            var adminUser = new CompanyRepository(_ctx).GetCompanyAdminMember(companyId);
            var companyName = _ctx.Companies.Find(companyId).Name;
            var memberName = _ctx.Members.Find(memberId).FirstName;

            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                UserIdFrom = adminUser.Id,
                UserIdTo = memberUser.Id,
                MessageText = string.Format(AssignContractorRoleMessage, memberName, companyName),
                MessageType = MessageType.AssignContractorRole,
                IsWaitingForResponse = false,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            //_ctx.SaveChanges();
        }

        public void GenerateAddMemberToCompany(int memberId, int companyId, CompanyRole role)
        {
            var companyName = _ctx.Companies.Find(companyId).Name;
            var memberName = _ctx.Members.Find(memberId).FirstName;

            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                Role = role,
                MessageText = string.Format(InviteJoinCompanyRequestMessage, memberName, companyName),
                MessageType = MessageType.InviteJoinCompanyRequest,
                IsWaitingForResponse = true,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }

        public void GenerateClientWorkRequest(int clientId, int propertyId, string messageText)
        {

            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                PropertyId = propertyId,
                ClientId = clientId,
                MessageText = messageText,
                MessageType = MessageType.WorkRequest,
                IsWaitingForResponse = true,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }

        public void GenerateAddPropertyCoClient(int propetyId, int clientId)
        {
            var clientName = _ctx.Clients.Find(clientId).FirstName;
            var property = _ctx.Properties.Find(propetyId).Name;

            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                PropertyId = propetyId,
                ClientId = clientId,
                MessageText = string.Format(AddPropertyCoOwnerMessage, clientName, property),
                MessageType = MessageType.AddPropertyCoOwner,
                IsWaitingForResponse = false,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }


        public void Dispose()
        {
            _ctx.Dispose();
            //_userManager.Dispose();
        }
    }
}