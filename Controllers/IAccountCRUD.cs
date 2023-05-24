using System.Collections.Generic;
using Account_CRUP_App.Models;

namespace Account_CRUP_App.Controllers
{
    public interface IAccountCRUD
    {
        string Create(Account accountData);
        Account Read(string id, List<string> fields);
        List<Account> Read(int pageNumber, int pageSize, List<string> fields);
        string Update(string id, Account accountData);
        bool Delete(string id);
    }
}
