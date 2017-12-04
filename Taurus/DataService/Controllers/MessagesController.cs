using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using EF.Data;
using DataService.Models;
using AutoMapper;

namespace DataService.Controllers
{

    [Authorize]
    public class MessagesController : ApiController
    {
        private IAuthRepository _authRepo;
        private IMessageRepository _messageRepo;
        public MessagesController(IAuthRepository authRepo, IMessageRepository messageRepo)
        {
            _authRepo = authRepo;
            _messageRepo = messageRepo;
        }

        public IHttpActionResult GetMessages()
        {
            return Ok(GetUserMessage());
        }

        public IHttpActionResult GetWorkRequestMessageForProperty(int propertyId)
        {
            var messages = GetUserMessage().Where(p => p.MessageType == MessageType.WorkRequest
             && p.PropertyId == propertyId).ToList();
            return Ok(messages);
        }

        private IEnumerable<MessageDetailModel> GetUserMessage()
        {
            var messages = _messageRepo.GetMessageForUser(User.Identity.Name).ToList();
            //messages.ForEach(p => p.HasResponse = p.MessageResponse != null);

            var returnList = messages.Select(Mapper.Map<Message, MessageDetailModel>).ToList();
            returnList.ForEach(p => p.HasResponse = p.MessageResponse != null);

            var userId = _authRepo.GetUserByUserName(User.Identity.Name).Id;

            foreach (var message in returnList)
            {
                if (message.UserIdTo == userId && message.IsRead == false)
                {
                    message.ShouldDisplayUnread = true;
                }
                if (message.MessageResponse != null
                    && message.MessageResponse.UserIdTo == userId)
                //&& message.MessageResponse.IsRead == false)
                {
                    message.Title = "You have a new respond";
                    if (message.MessageResponse.IsRead == false)
                        message.ShouldDisplayUnread = true;
                }
            }
            return returnList;
        }

        public IHttpActionResult GetMessage(int messageId)
        {
            var message = _messageRepo.GetMessageForUser(User.Identity.Name).Where(p => p.Id == messageId).FirstOrDefault();

            return Ok(Mapper.Map<Message, MessageDetailModel>(message));
        }
        //called every 2 second
        public IHttpActionResult GetUnReadMessagesCount()
        {
            var count = _messageRepo.GetUnReadMessageCountForUser(User.Identity.Name);
            return Ok(count);
        }

        [HttpPost]
        public IHttpActionResult MarkMessageAsRead(int messageId)
        {
            var userId = _authRepo.GetUserByUserName(User.Identity.Name).Id;
            _messageRepo.MarkMessageAsRead(messageId, userId);
            return Ok();

        }

        [HttpPost]
        public IHttpActionResult GenerateClientWorkRequest(MessageDetailModel model)
        {
            var userId = _authRepo.GetUserByUserName(User.Identity.Name).Id;
            _messageRepo.GenerateClientWorkRequest(model, userId);
            return (Ok());

        }

        [HttpPost]
        public IHttpActionResult CreatePropertyForWorkRequest(int messageId, PropertyModel property)
        {
            var userId = _authRepo.GetUserByUserName(User.Identity.Name).Id;
            _messageRepo.CreatePropertyForWorkRequest(messageId, property);
            return (Ok());

        }
        [HttpPost]
        public IHttpActionResult HandleMessageResponse(int messageId, ResponseAction  action)
        {
            _messageRepo.HandleMessageResponse(messageId, action);
            return Ok();

        }
    }
}
