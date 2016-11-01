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

namespace DataService.Controllers
{

    [Authorize]
    public class MessagesController : ApiController
    {


        public IHttpActionResult GetMessages()
        {
            var messages = new MessageRepository().GetMessageForUser(User.Identity.Name);

            return Ok(messages.Select(Mapper.Map<Message, MessageModel>));

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
        public IHttpActionResult MarkMessageAsRead(int messageOrResponseId)//this is wrong need change
        {
            new MessageRepository().MarkMessageAsRead(messageOrResponseId);
            return Ok();

        }

        [HttpPost]
        public IHttpActionResult HandleMessageResponse(int messageId, ResponseAction  action)
        {
            new MessageRepository().HandleMessageResponse(messageId, action);
            return Ok();

        }
    }
}
