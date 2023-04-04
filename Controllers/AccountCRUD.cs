using Account_CRUD_App.Models;
using Account_CRUP_App.Models;
using Newtonsoft.Json;
using System.Drawing.Printing;

namespace Account_CRUP_App.Controllers
{
    public class AccountCRUD : IAccountCRUD
    {
        SFLogin log = new SFLogin();
        public string Create(object accountData)
        {
            Console.WriteLine("\nCreateMethod::" + accountData );
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(accountData.ToString());
            Console.WriteLine($"\n\nTake query: {data}");
            string cont = "{";
            int i = 0;
            foreach (KeyValuePair<string, string> entry in data)
            {
                Console.WriteLine("\nITEM::" + entry);
                if (i != 0)
                {
                    cont = cont + ",";
                }
                cont += "\r\n    \"" + entry.Key + "\": \"" + entry.Value + "\"";
                Console.WriteLine("\ncont++ " + cont);
                i++;
            }
            cont = cont + "\r\n}";
            Console.WriteLine("\nSTRING::" + cont + "::");

           
            string response = log.getBoolean(cont, HttpMethod.Post, "");
            Console.WriteLine("\nResponseCreate::"+response);
            if (response.Contains("\"success\":true"))
            {
                string str = response.Substring(7, 18);
                Console.WriteLine("\nstrID in response: " + str);
                return str;
            } else
            {
                return response;
            }
            //return "";
            //throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            if (log.deleteQuery(id))
            {
                return true;
            } else
            {
                return false;
            }
            
            //throw new NotImplementedException();
        }

        public object Read(string id, List<string> fields)
        {
            string fieldStr = "";
            int i = 0;
            if (fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    if (i != 0)
                    {
                        fieldStr += ", ";
                    }
                    fieldStr += field.ToString();
                    i++;
                }
            }
            else
            {
                fieldStr = "Id, Name";
            }
            Console.WriteLine("\nFieldStr::" + fieldStr);
            
            string getData = log.getQuery("select " + fieldStr + " from Account where id = '" + id + "'");
            Console.WriteLine("\nHomeGETData::" + getData);
            if (!String.IsNullOrEmpty(getData) && !(getData.Contains("errorCode")))
            {
                var accRecords = JsonConvert.DeserializeObject<AccountModel>(getData.ToString());
                return accRecords.records[0];
            }
            else
            {
                string data = getData.Substring(1, getData.Length - 2);
                Console.WriteLine("\nSubstring:" + data);
                var error = JsonConvert.DeserializeObject<ErrorViewModel>(data.ToString());
                Console.WriteLine($"\nError: {error}");
                return error;
            }
            //throw new NotImplementedException();
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
            
            string getData = log.getQuery("select "+fieldStr+" from Account order by LastModifiedDate DESC limit "+pageSize);
            Console.WriteLine("\nHomeGETData::" + getData);
            if (!String.IsNullOrEmpty(getData) && !(getData.Contains("ERROR::")))
            {
                var accRecords = JsonConvert.DeserializeObject<AccountModel>(getData.ToString());
                return accRecords.records.ToList<object>();
            } else
            {
                var error = JsonConvert.DeserializeObject<ErrorViewModel>(getData.ToString());
                Console.WriteLine($"\nError: {error}");
                List<object> errList = new List<object>();
                return errList;
            }
            //throw new NotImplementedException();
        }

        
        public string Update(string id, object accountData)
        {
            Console.WriteLine("\nUpdateMethod::" + accountData);
            
            string idStr = "";
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(accountData.ToString());
            Console.WriteLine($"\n\nTake query: {data}");
            string cont = "{";
            int i = 0;
            foreach (KeyValuePair<string, string> entry in data)
            {
                Console.WriteLine("\nITEM::" + entry);
                if (i != 0)
                {
                    cont = cont + ",";
                }
                if (entry.Key == "Id")
                {
                    idStr = "/" + entry.Value;
                    continue;
                }
                cont += "\r\n    \"" + entry.Key + "\": \"" + entry.Value + "\"";
                Console.WriteLine("\ncont++ " + cont);
                i++;
            }
            cont = cont + "\r\n}";
            Console.WriteLine("\nSTRING::" + cont + "::" + idStr);


            string response = log.getBoolean(cont, HttpMethod.Patch, idStr);
            Console.WriteLine("\nResponseCreate::" + response);
            if (response.Contains("\"success\":true"))
            {
                string str = response.Substring(7, 18);
                Console.WriteLine("\nstrID in response: " + str);
                return str;
            }
            else
            {
                return response;
            }
            //throw new NotImplementedException();
        }
    }
}
