using DataService.Models;
using System.Linq;

namespace EF.Data
{
    public interface IEmailRepository : IBaseRepository
    {
        void Save(EmailHistory record);
    }
}