using System.Collections.Generic;

namespace Account_CRUP_App.Controllers
{
    public interface IAccountCRUD
    {
        string Create(object accountData);
        object Read(string id, List<string> fields);
        List<object> Read(int pageNumber, int pageSize, List<string> fields);
        string Update(string id, object accountData);
        bool Delete(string id);
    }
}
