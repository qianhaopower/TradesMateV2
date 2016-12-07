using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using EF.Data;
using DataService.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http.Description;
using DataService.Models;
using AutoMapper;
using Microsoft.AspNet.Identity;

namespace DataService.Controllers
{

    [Authorize]
    public class MessagesController : ApiController
    {


        public IHttpActionResult GetMessages()
        {
            var messages = new MessageRepository().GetMessageForUser(User.Identity.Name).ToList();
            //messages.ForEach(p => p.HasResponse = p.MessageResponse != null);

            var returnList = messages.Select(Mapper.Map<Message, MessageDetailModel>).ToList();
            returnList.ForEach(p => p.HasResponse = p.MessageResponse != null);

            var userId = new AuthRepository().GetUserByUserName(User.Identity.Name).Id;

            //returnList.ForEach(p => p.ShouldDisplayUnread =
            //(p.UserIdTo == userId && p.IsRead == false)
            //|| (p.MessageResponse != null && p.MessageResponse.UserIdTo == userId && p.MessageResponse.IsRead == false)
            //);

            foreach(var message in returnList)
            {
                if(message.UserIdTo == userId && message.IsRead == false)
                {
                    message.ShouldDisplayUnread = true;
                }
                if(message.MessageResponse != null 
                    && message.MessageResponse.UserIdTo == userId)
                    //&& message.MessageResponse.IsRead == false)
                {
                    message.Title = "You have a new respond";
                    if (message.MessageResponse.IsRead == false)
                    message.ShouldDisplayUnread = true;

                   
                }
            }

            return Ok(returnList);

        }


        public IHttpActionResult GetMessage(int messageId)
        {
            var message = new MessageRepository().GetMessageForUser(User.Identity.Name).Where(p => p.Id == messageId).FirstOrDefault();

            return Ok(Mapper.Map<Message, MessageDetailModel>(message));

        }
        //called every 2 second
        public IHttpActionResult GetUnReadMessagesCount()
        {
            var count = new MessageRepository().GetUnReadMessageCountForUser(User.Identity.Name);

            return Ok(count);
        }

        [HttpPost]
        public IHttpActionResult MarkMessageAsRead(int messageId)//this is wrong need change
        {
            var userId = new AuthRepository().GetUserByUserName(User.Identity.Name).Id;
            new MessageRepository().MarkMessageAsRead(messageId, userId);
            return Ok();

        }

        //[HttpPost]
        //public IHttpActionResult MarkResponseAsRead(int responseId)//this is wrong need change
        //{
        //    var current = User.Identity.GetUserId();
        //    new MessageRepository().MarkResponseAsRead(responseId, current);
        //    return Ok();

        //}

        [HttpPost]
        public IHttpActionResult GenerateClientWorkRequest(MessageDetailModel model)
        {
            var userId = new AuthRepository().GetUserByUserName(User.Identity.Name).Id;
            new MessageRepository().GenerateClientWorkRequest(model, userId);
            return (Ok());

        }


        [HttpPost]
        public IHttpActionResult CreatePropertyForWorkRequest(int messageId, PropertyModel property)
        {
            var userId = new AuthRepository().GetUserByUserName(User.Identity.Name).Id;
            new MessageRepository().CreatePropertyForWorkRequest(messageId, property);
            return (Ok());

        }
        [HttpPost]
        public IHttpActionResult HandleMessageResponse(int messageId, ResponseAction  action)
        {
            new MessageRepository().HandleMessageResponse(messageId, action);
            return Ok();

        }
    }
}
