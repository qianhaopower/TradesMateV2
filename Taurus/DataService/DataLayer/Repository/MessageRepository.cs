
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

        #region messages
                private const string AssignDefaultRoleMessage = "Dear {0}, You are assigned default role in {1}. Now you can view all {1}'s properties and related works.";

                //XXX(Admin name) want to assign you defaut role in YYY(company name), if you accept you will become contractor in ZZZ,WWW and XXX(other company name).
                private const string AssignDefaultRoleRequestMessage = "Dear {0}, {1} want to assign you defaut role, if you accept your role in {2} will become contractor.";

      

                //XXX(Admin name) has assigned you contractor role in YYY(company name), now you can view YYY's properties and related works allocated to you.
                private const string AssignContractorRoleMessage = "Dear {0}, You are assigned contractor role in {1}. Now you can view {1}'s properties allocated to you.";

                //XXX(Admin name) has invited you to join YYY(company name).
                private const string InviteJoinCompanyRequestMessage = "Dear {0}, you are invited to join {1}. You can see {1}'s properties if accept.";

        //XXX(name) has invited you to join YYY(company name).
        private const string InviteClientJoinCompanyRequestMessage = "Dear {0}, {1} wants to list you as client. {1} will be able to allocate work to your proeprty if accept.";

        //AAA(ClientName) has granted you access to property WWW(propertyName, address)
        private const string AddPropertyCoOwnerMessage = "Dear {0}, you are granted access to {1., You can see {1}'s works now.";
        #endregion

        public MessageRepository(EFDbContext ctx) : base(ctx)
        {

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

            var responds = _ctx.Messages
                .Include(p => p.MessageResponse)
                      .Include(p => p.Member)
                .Where(p => p.MessageResponse != null && p.MessageResponse.UserIdTo == user.Id)
              ;
           
            return messages.Union(responds);
        }

        public IQueryable<Client> GetClientThatHasMessageForUser(string username)
        {
            var user = new AuthRepository(_ctx).GetUserByUserName(username);
            var clients = _ctx.Messages.Where(p => p.UserIdTo == user.Id && p.MessageType == MessageType.WorkRequest)
                .Include(p => p.Client).Select(p => p.Client);
            return clients;
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

        public void CreatePropertyForWorkRequest(int messageId, PropertyModel model)
        {
            var message = _ctx.Messages.Find(messageId);
            if(message == null)
            {
                throw new Exception("Cannot find message with Id "+ messageId);
            }
            if (!message.IsWaitingForResponse)
            {
                throw new Exception("Message already handled ");
            }

            if(message.PropertyId != null)
            {
                throw new Exception("Property already attached for this message");
            }

            if(model.Address == null)
            {
                throw new Exception("Address cannot be null");
            }

            Property newProperty = new Property
            {
                Name = model.Name,
                Address = new Address()
                {
                    City = model.Address.City,
                    Line1 = model.Address.Line1,
                    Line2 = model.Address.Line2,
                    Line3 = model.Address.Line3,
                    PostCode = model.Address.PostCode,
                    State = model.Address.State,
                    Suburb = model.Address.Suburb,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                },
                Description = model.Description,
                Condition = model.Condition,
                Narrative = model.Narrative,
                Comment = model.Comment,
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,

            };
            _ctx.Entry(newProperty).State = EntityState.Added;
            message.Property = newProperty;
            message.PropertyAddress = message.PropertyAddress + " (Address record created)";
            _ctx.Entry(message).State = EntityState.Modified;


            //add this property to the company
            PropertyCompany propertyCompanyNew = new PropertyCompany
            {
                Property = newProperty,
                CompanyId = message.CompanyId.Value,
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
            };
            _ctx.Entry(propertyCompanyNew).State = EntityState.Added;



            ClientProperty clientPropertyNew = new ClientProperty
            {
                ClientId = message.ClientId.Value,
                Property = newProperty,
                Confirmed = true,
                Role = ClientRole.Owner,
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
            };

            _ctx.Entry(clientPropertyNew).State = EntityState.Added;

            new PropertyRepository(_ctx).CreateDefaultSections(model.DefaultSections, newProperty, _ctx);

            _ctx.SaveChanges();

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
                new CompanyRepository(_ctx).DoUpdateCompanyMemberRole(message.CompanyId.Value, message.MemberId.Value, message.Role);

            }

        }

        private void HandleInviteJoinCompanyResponse(Message message, ResponseAction action)
        {
            message.IsWaitingForResponse = false;
            if (action != ResponseAction.Accept) return;
            if (message.CompanyId == null) return;
            if (message.MemberId != null)
                new CompanyRepository(_ctx).DoMemberJoinCompany(message.CompanyId.Value, message.MemberId.Value);
        }

        private void InviteClientToCompanyResponse(Message message, ResponseAction action)
        {
            message.IsWaitingForResponse = false;
            if (action != ResponseAction.Accept) return;
            if (message.CompanyId == null) return;
            if (message.ClientId != null)
                new CompanyRepository(_ctx).DoClientAddToCompany(message.CompanyId.Value, message.ClientId.Value);
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




        public void GenerateAddClientToCompany(int clientId, int companyId, string messageFromRequestor)
        {
            var companyName = _ctx.Companies.Find(companyId)?.Name;
            var clientName = _ctx.Clients.Find(clientId)?.FirstName;

            var memberUser = _ctx.Users.First(p => p.Client.Id == clientId);

            var adminUser = new CompanyRepository(_ctx).GetCompanyAdminMember(companyId);

            var systemMessage = string.Format(InviteClientJoinCompanyRequestMessage, clientName, companyName);
            // var messageToSend = systemMessage + char(13) + CHAR(10) + messageFromRequestor;


            var sb = new StringBuilder();
            sb.AppendLine(systemMessage);
            sb.AppendLine("<br/>");
            sb.AppendLine(messageFromRequestor);
            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CompanyId = companyId,
                ClientId = clientId,
                UserIdFrom = adminUser.Id,
                UserIdTo = memberUser.Id,
                MessageText = sb.ToString(),
                MessageType = MessageType.InviteClientToCompany,
                IsWaitingForResponse = true,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }

        public void GenerateAddMemberToCompany(int memberId, int companyId, string messageFromRequestor,CompanyRole role)
        {
            var companyName = _ctx.Companies.Find(companyId).Name;
            var memberName = _ctx.Members.Find(memberId).FirstName;

            var memberUser = _ctx.Users.First(p => p.Member.Id == memberId);

            var adminUser = new CompanyRepository(_ctx).GetCompanyAdminMember(companyId);

            var systemMessage = string.Format(InviteJoinCompanyRequestMessage, memberName, companyName);
           // var messageToSend = systemMessage + char(13) + CHAR(10) + messageFromRequestor;


            var sb = new StringBuilder();
            sb.AppendLine(systemMessage);
            sb.AppendLine("<br/>");
            sb.AppendLine(messageFromRequestor);
            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                Role = role,
                UserIdFrom = adminUser.Id,
                UserIdTo = memberUser.Id,
                MessageText = sb.ToString(),
                MessageType = MessageType.InviteJoinCompanyRequest,
                IsWaitingForResponse = true,
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
            var client = new ClientRepository(_ctx).GetClientForUser(userId);
            if (client.MobileNumber == null)
            {
                client.MobileNumber = model.Mobile;
            }
            _ctx.Entry<Client>(client).State = EntityState.Modified;


            //the message should be sent to the admin of the company.
            var userIdTo =  new CompanyRepository(_ctx).GetCompanyAdminMember(model.CompanyId.Value).Id;
            var clientId = new ClientRepository(_ctx).GetClientForUser(userId).Id;
            var memberId = new ClientRepository(_ctx).GetMemberForUser(userIdTo).Id;

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
            }else// new property
            {

            }
               


            
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
       
    }
}