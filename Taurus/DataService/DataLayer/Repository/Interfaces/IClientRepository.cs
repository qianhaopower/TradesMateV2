﻿using DataService.Infrastructure;
using DataService.Models;
using System.Linq;

namespace EF.Data
{
    public interface IClientRepository
    {
        Client GetClientForUser(string userId);

        Member GetMemberForUser(string userId);
        IQueryable<Client> GetAccessibleClientForUser(string userName);

        Client GetClient(string userName, int clientId);
        Client UpdateClient(string userName, ClientModel model);
        void RemoveClient(string userName, int clientId);
        string CreateClient(string name, CreateNewClientRequestModel model, ApplicationUserManager appUserManager);
    }
}