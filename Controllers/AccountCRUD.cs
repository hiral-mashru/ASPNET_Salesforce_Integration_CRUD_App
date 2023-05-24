using Account_CRUD_App.Models;
using Account_CRUP_App.Models;
using Account_CRUP_App.Models;
using Newtonsoft.Json;

namespace Account_CRUP_App.Controllers
{
    public class AccountCRUD : IAccountCRUD
    {
        SFLogin log = new SFLogin();
        public string Create(Account acc)
        {
            Console.WriteLine("\nCreateMethod::" + acc );
            string reqBody = "{\r\n    \"Name\": \"" + acc.Name + "\",\r\n    \"NumberOfEmployees\":\"" + acc.NumberOfEmployees + "\",\r\n    " +
                "\"ShippingState\":\"" + acc.ShippingState + "\",\r\n    \"ShippingCountry\":\"" + acc.ShippingCountry + "\",\r\n    " +
                "\"Phone\":\"" + acc.Phone + "\",\r\n    \"Fax\":\"" + acc.Fax + "\",\r\n    \"Apttus_Billing__SLASerialNumber__c\":" +
                "\"" + acc.Apttus_Billing__SLASerialNumber__c + "\",\r\n    \"BillingCity\":\"" + acc.BillingCity + "\",\r\n    " +
                "\"BillingState\": \"" + acc.BillingState + "\",\r\n    \"BillingCountry\": \"" + acc.BillingCountry + "\"\r\n}";

            /*var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(accountData.ToString());
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
            cont = cont + "\r\n}";*/
            Console.WriteLine("\nSTRING::" + reqBody + "::");

           
            string response = log.postData(reqBody, HttpMethod.Post, "");
            Console.WriteLine("\nResponseCreate::"+response);
            if (response.Contains("true"))
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

        public Account Read(string id, List<string> fields)
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
            
            string getData = log.getData("select " + fieldStr + " from Account where id = '" + id + "'");
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
                //var error = JsonConvert.DeserializeObject<ErrorViewModel>(data.ToString());
                //Console.WriteLine($"\nError: {error}");
                return null;
            }
            //throw new NotImplementedException();
        }

        public List<Account> Read(int pageNumber, int pageSize, List<string> fields)
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
            Console.WriteLine("\nFieldStr::"+fieldStr+"::"+pageNumber);
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }
            string getData = log.getData("select "+fieldStr+" from Account order by LastModifiedDate DESC limit "+pageSize+" offset "+((pageNumber-1)*pageSize));
            Console.WriteLine("\nHomeGETData::" + getData);
            if (!String.IsNullOrEmpty(getData) && !(getData.Contains("ERROR::")))
            {
                var accRecords = JsonConvert.DeserializeObject<AccountModel>(getData.ToString());
                return accRecords.records.ToList<Account>();
            } else
            {
                var error = JsonConvert.DeserializeObject<ErrorViewModel>(getData.ToString());
                Console.WriteLine($"\nError: {error}");
                List<Account> errList = new List<Account>();
                return errList;
            }
            //throw new NotImplementedException();
        }

        
        public string Update(string id, Account acc)
        {
            Console.WriteLine("\nUpdateMethod::" + acc.ToString());

            string reqBody = "{\r\n    \"Name\": \""+acc.Name + "\",\r\n    \"NumberOfEmployees\":\"" + acc.NumberOfEmployees + "\",\r\n    " +
                "\"ShippingState\":\"" + acc.ShippingState + "\",\r\n    \"ShippingCountry\":\"" + acc.ShippingCountry + "\",\r\n    " +
                "\"Phone\":\""+acc.Phone+ "\",\r\n    \"Fax\":\""+acc.Fax+ "\",\r\n    \"Apttus_Billing__SLASerialNumber__c\":" +
                "\""+acc.Apttus_Billing__SLASerialNumber__c+ "\",\r\n    \"BillingCity\":\""+acc.BillingCity+ "\",\r\n    " +
                "\"BillingState\": \""+acc.BillingState+ "\",\r\n    \"BillingCountry\": \""+acc.BillingCountry+ "\"\r\n}";

            /*string idStr = "";
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(acc.ToString());
            Console.WriteLine($"\n\nTake query: {data}");
            string reqBody = "{";
            int i = 0;
            foreach (KeyValuePair<string, string> entry in data)
            {
                Console.WriteLine("\nITEM::" + entry);
                if (i != 0)
                {
                    reqBody = reqBody + ",";
                }
                if (entry.Key == "Id")
                {
                    idStr = "/" + entry.Value;
                    continue;
                }
                reqBody += "\r\n    \"" + entry.Key + "\": \"" + entry.Value + "\"";
                Console.WriteLine("\ncont++ " + reqBody);
                i++;
            }
            reqBody = reqBody + "\r\n}";*/
            Console.WriteLine("\nSTRING::" + reqBody);


            string response = log.postData(reqBody, HttpMethod.Patch, "/"+id);
            Console.WriteLine("\nResponseCreate::" + response);
            if(response.Contains("True")) {
                Console.WriteLine("Response of Update: " + response);
                return id;
            }
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
