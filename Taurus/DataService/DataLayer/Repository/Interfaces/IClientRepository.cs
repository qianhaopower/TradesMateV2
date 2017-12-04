using System.Linq;

namespace EF.Data
{
    public interface IClientRepository
    {
        Client GetClientForUser(string userId);

        Member GetMemberForUser(string userId);
        IQueryable<Client> GetAccessibleClientForUser(string userName);

        Client GetClient(string userName, int clientId);
        void DeleteClient(string userName, int clientId);
    }
}