using System.Collections.Generic;
using Account_CRUP_App.Models;

namespace Account_CRUP_App.Controllers
{
    public interface IAccountCRUD
    {
        string Create(object accountData);
        Records Read(string id, List<string> fields);
        List<Records> Read(int pageNumber, int pageSize, List<string> fields);
        string Update(string id, object accountData);
        bool Delete(string id);
    }
}
