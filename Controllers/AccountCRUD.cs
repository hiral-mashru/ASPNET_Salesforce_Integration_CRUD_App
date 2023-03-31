using Account_CRUD_App.Models;
using Account_CRUP_App.Models;
using Newtonsoft.Json;

namespace Account_CRUP_App.Controllers
{
    public class AccountCRUD : IAccountCRUD
    {
        public string Create(object accountData)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public object Read(string id, List<string> fields)
        {
            throw new NotImplementedException();
        }

        public List<object> Read(int pageNumber, int pageSize, List<string> fields)
        {
            string fieldStr = "";
            int i = 0;
            if(fields.Count > 0)
            {
                foreach(var field in fields)
                {
                    if (i != 0)
                    {
                        fieldStr += ", ";
                    }
                    fieldStr += field.ToString();
                    i++;
                }
            } else
            {
                fieldStr = "Id, Name";
            }
            Console.WriteLine("\nFieldStr::"+fieldStr);
            SFLogin log = new SFLogin();
            string getData = log.getQuery("select "+fieldStr+" from Account order by LastModifiedDate DESC limit "+pageSize);
            Console.WriteLine("\nHomeGETData::" + getData);
            if (!String.IsNullOrEmpty(getData) && !(getData.Contains("ERROR::")))
            {
                var accRecords = JsonConvert.DeserializeObject<AccountModel>(getData.ToString());
                return accRecords.records.ToList<object>();
            } else
            {
                //object err = { "Name": getData } ;
                return new List<object>() { getData };
            }
            throw new NotImplementedException();
        }

        public string Update(string id, object accountData)
        {
            throw new NotImplementedException();
        }
    }
}
