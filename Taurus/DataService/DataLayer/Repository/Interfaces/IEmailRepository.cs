using DataService.Models;
using System.Linq;

namespace EF.Data
{
    public interface IEmailRepository
    {
        void Save(EmailHistory record);
    }
}