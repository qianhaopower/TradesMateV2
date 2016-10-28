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
    

    public class MessagesController : ApiController
    {
       

        public IHttpActionResult GetMessages()
        {
            var messages = new MessageRepository().GetMessageForUser(User.Identity.Name);

            return Ok(messages.Select(Mapper.Map<Message, MessageModel>));

        }

        //called every 2 second
        public IHttpActionResult GetPendingMessagesCount()
        {
            var count = new MessageRepository().GetPendingMessageCountForUser(User.Identity.Name);

            return Ok(count);
        }



    }
}
