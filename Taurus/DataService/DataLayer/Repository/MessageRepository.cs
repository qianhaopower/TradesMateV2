
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

    public class MessageRepository : BaseRepository, IMessageRepository
    {  
        //AAA(ClientName) has granted you access to property WWW(propertyName, address)
        private const string AddPropertyCoOwnerMessage = "Dear {0}, you are granted access to {1., You can see {1}'s works now.";


        private readonly IAuthRepository _authRepo;
        private readonly ICompanyRepository _companyRepository;
    

        public MessageRepository(EFDbContext ctx, ApplicationUserManager manager, IAuthRepository authRepo, ICompanyRepository companyRepository) : base(ctx,manager)
        {
            _authRepo = authRepo;
            _companyRepository = companyRepository;
        }


        public IQueryable<Message> GetMessageForUser(string username)
        {
            var user = _authRepo.GetUserByUserName(username);
            var messages = _ctx.Messages.Where(p => p.UserIdTo == user.Id)
                .Include(p => p.MessageResponse)
                .Include(p => p.Member)
                .Include(p => p.Property)
                .Include(p => p.Client)
                .Include(p => p.Company);

            var responds = _ctx.Messages
                .Include(p => p.MessageResponse)
                      .Include(p => p.Member)
                .Where(p => p.MessageResponse != null && p.MessageResponse.UserIdTo == user.Id)
              ;
           
            return messages.Union(responds);
        }

        public IQueryable<Client> GetClientThatHasMessageForUser(string username)
        {
            var user = _authRepo.GetUserByUserName(username);
            var clients = _ctx.Messages.Where(p => p.UserIdTo == user.Id && p.MessageType == MessageType.WorkRequest)
                .Include(p => p.Client).Select(p => p.Client);
            return clients;
        }

        public int GetUnReadMessageCountForUser(string username)
        {
            var user = _authRepo.GetUserByUserName(username);
            var count = _ctx.Messages.
                Include(p => p.MessageResponse//I send the message, it has a unread response. 
                )
                .Count(p => (p.UserIdTo == user.Id && p.IsRead == false) // message sent to me but I have not read
                ||(p.UserIdFrom == user.Id && p.MessageResponse != null && p.MessageResponse.IsRead == false));
                
            return count;
        }

       

        public void MarkMessageAsRead(int messageId, string userId)
        {
            // we mark both message and response, but only one should has the user id.
            var message = _ctx.Messages.Find(messageId);
          
            if (message != null
                  && message.UserIdTo == userId
                  && message.IsRead == false)//only mark it if current user is the message/response destinaion
            {
                message.IsRead = true;
                _ctx.Entry(message).State = EntityState.Modified;
            }

            var response = _ctx.MessageResponses.Find(messageId);

            if (response != null
                && response.UserIdTo == userId
                && response.IsRead == false)//only mark it if current user is the message/response destinaion
            {
                response.IsRead = true;
                _ctx.Entry(response).State = EntityState.Modified;
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
                    HandleWorkRequestResponse(message, action);
                    break;
                case MessageType.AssignDefaultRoleRequest:
                    HandleAssignDefaultRoleResponse(message, action);
                    break;
                case MessageType.InviteJoinCompanyRequest:
                    HandleInviteJoinCompanyResponse(message, action);
                    break;
                case MessageType.InviteClientToCompany:
                    InviteClientToCompanyResponse(message, action);
                    break;

            }
            message.IsWaitingForResponse = false;
            GenerateResponse(messageId, action);
        }

        private void HandleWorkRequestResponse(Message message, ResponseAction action)
        {
            message.IsWaitingForResponse = false;
            if (action == ResponseAction.Accept)
            {
              
            }

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
               _companyRepository.DoUpdateCompanyMemberRole(message.CompanyId.Value, message.MemberId.Value, message.Role);

            }

        }

        private void HandleInviteJoinCompanyResponse(Message message, ResponseAction action)
        {
            message.IsWaitingForResponse = false;
            if (action != ResponseAction.Accept) return;
            if (message.CompanyId == null) return;
            if (message.MemberId != null)
                _companyRepository.DoMemberJoinCompany(message.CompanyId.Value, message.MemberId.Value);
        }

        private void InviteClientToCompanyResponse(Message message, ResponseAction action)
        {
            message.IsWaitingForResponse = false;
            if (action != ResponseAction.Accept) return;
            if (message.CompanyId == null) return;
            if (message.ClientId != null)
                _companyRepository.DoClientAddToCompany(message.CompanyId.Value, message.ClientId.Value);
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

        public void GenerateClientWorkRequest(MessageDetailModel model, string userId)
        {

            //here userId is the client who request the work. 
            if (!model.CompanyId.HasValue)
            {
                throw new Exception("Must select a company to generate a work request.");
            }
            if (model.PropertyId.HasValue)
            {
                //this is a existing property, do not care the property address field then. 
                model.PropertyAddress = null;
            }

            //set the client mobile number if it is not there.
            var client = _authRepo.GetClientForUser(userId);
            if (client.MobileNumber == null)
            {
                client.MobileNumber = model.Mobile;
            }
            _ctx.Entry<Client>(client).State = EntityState.Modified;


            //the message should be sent to the admin of the company.
            var userIdTo = _companyRepository.GetCompanyAdminMember(model.CompanyId.Value).Id;
            var clientId = _authRepo.GetClientForUser(userId).Id;
            var memberId = _authRepo.GetMemberForUser(userIdTo).Id;

            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                PropertyId = model.PropertyId,
                CompanyId = model.CompanyId,
                ClientId = clientId,
                Section = model.Section,
                ServiceType = model.ServiceType,
                UserIdTo = userIdTo,
                UserIdFrom = userId,
                MemberId = memberId,
                MessageText = model.MessageText,
                MessageType = MessageType.WorkRequest,
                IsWaitingForResponse = true,
                IsRead = false,
                PropertyAddress = model.PropertyAddress,

            };
            _ctx.Entry<Message>(message).State = EntityState.Added;

            if (model.PropertyId.HasValue)//existing property
            {
                // if the company-property relationship is not there, add it. 
                if (_ctx.PropertyCompanies.Any(p => p.CompanyId == message.CompanyId && p.PropertyId == message.PropertyId))
                {
                    //relationship is already there, all good. The client is requesting work for a property that with this company
                }
                else
                {
                    // add the relationship
                    PropertyCompany propertyCompanyNew = new PropertyCompany
                    {
                        PropertyId = message.PropertyId.Value,
                        CompanyId = message.CompanyId.Value,
                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                    };
                    _ctx.Entry(propertyCompanyNew).State = EntityState.Added;

                }
            }
            else// new property
            {

            }




            _ctx.SaveChanges();
        }

    }
}