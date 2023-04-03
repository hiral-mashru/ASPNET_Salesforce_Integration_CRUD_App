using Account_CRUD_App.Models;
using Account_CRUP_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Web.Helpers;
using Account_CRUD_App.Controllers;
using System.Linq;

namespace Account_CRUP_App.Controllers
{
    public class HomeController : Controller
    {
        AccountCRUD accCRUD = new AccountCRUD();
        //List<string> fields = new List<string>() { "Id", "Name", "Region__c", "Type", "Customer_Rating__c" };
        List<string> fields = new List<string>() { "Id", "Name", "Region__c", "Type", "Customer_Rating__c", "Phone", "Fax", "Apttus_Billing__SLASerialNumber__c", "BillingCity", "BillingState", "BillingCountry" };

        private readonly ILogger<HomeController> _logger;
        public static string access_token;
        public static string instance_url;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult getAccount()
        {
            if (access_token != "") { 
                Console.WriteLine("\nIN HOME Controller\n");
                List<object> response = new List<object>();
                response = accCRUD.Read(1, 10, fields);
                Console.WriteLine("\nress::" + response.ToString + "--");
                foreach (object res in response)
                {
                    Console.WriteLine("\nress::" + res.ToString);
                }
                ViewBag.accData = response;
            } else
            {
                TempData["errMsg"] = "Access Token is missing...";
            }
            
            return View("Accounts");
        }

        public IActionResult singleAcc(string id)
        {
            if (access_token != "")
            {
                if (TempData.ContainsKey("createResponse"))
                {
                    Console.WriteLine("\nAfter Create::" + id);
                }
                //AccountCRUD accCRUD = new AccountCRUD();
                object response = new object();
                response = accCRUD.Read(id, fields);
                Console.WriteLine("\nress::" + response.ToString + "--");
                if(response != null)
                {
                    ViewBag.aceess_token = access_token;
                    ViewBag.accDetail = response;
                }
            }
            else
            {
                TempData["errMsg"] = "Access Token is missing...";
            }
            return View("AccDetails");
        }

        //[Route("Home/createAccount")]
        public IActionResult showCreateAcc()
        {
            return View("CreateAcc");
        }

        //[HttpPost]
        public ActionResult createAccount(string input)
        {
            //AccountCRUD accCRUD = new AccountCRUD();
            string response = accCRUD.Create(input);
            if (response.Length == 18)
            {
                TempData["createResponse"] = response;
                Console.WriteLine("INNNN");
                singleAcc(response);
                return RedirectToAction("singleAcc", "Home", new { id = response });
            } else
            {
                return RedirectToAction("createAccount", "Home");
            }
            
        }

        public IActionResult showUpdateAccount(string id)
        {
            Console.WriteLine("\nUpdateAcc");
            SFLogin log = new SFLogin();
            if (access_token != "" && instance_url != "")
            {
                string getData = log.getQuery("select id, name, Region__c, Type, Customer_Rating__c from Account where id = '" + id + "'");
                Console.WriteLine("\nHomeGETData::" + getData);

                if (!String.IsNullOrEmpty(getData))
                {
                    var accRecords = JsonConvert.DeserializeObject<AccountModel>(getData.ToString());
                    Console.WriteLine("\nMODEL:: " + accRecords.records[0].Name);
                    if (accRecords.records.Count > 0)
                    {
                        ViewBag.aceess_token = access_token;
                        ViewBag.accDetail = accRecords.records[0];
                    }
                }
            }
            return View("UpdateAccount");
        }

        public ActionResult updateAccount(string input, string id)
        {
            Console.WriteLine("\nUPDATE:: " + id+" :: "+input);
            string response = accCRUD.Update(id, input);
            Console.WriteLine("\nAfter Update::"+ response);
            if (response.Length == 18)
            {
                TempData["createResponse"] = response;
                Console.WriteLine("INNNN");
                singleAcc(response);
                return RedirectToAction("singleAcc", "Home", new { id = response });
            }
            else
            {
                return RedirectToAction("createAccount", "Home");
            }


            //SFLogin log = new SFLogin();
            //bool response = log.takeQuery(input);
            //TempData["status"] = "Account is Updated!";
            //singleAcc(response);
            //return RedirectToAction("getAccount", "Home");
        }
        public ActionResult deleteAccount(string id)
        {
            SFLogin log = new SFLogin();
            if(log.deleteQuery(id))
            {
                TempData["status"] = "Account is Deleted.";
            }
            return RedirectToAction("getAccount","Home");
        }
        public IActionResult Index()
        {
            Console.WriteLine("\nIN HOME Controller\n" + instance_url + ":::"+access_token);
            
            /*Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            if (config.AppSettings.Settings["access_token"].Value != "")
            {
                Console.WriteLine("\nIndex CONFIGG::" + config.AppSettings.Settings["access_token"].Value);
            }
            instance_url = config.AppSettings.Settings["instance_url"].Value;
            access_token = config.AppSettings.Settings["access_token"].Value;
            */
            SFLogin log = new SFLogin();
            int pageNum = 1;
            string getData = log.getQuery("select id, name from Apttus_Proposal__Proposal__c ORDER BY LastModifiedDate DESC ");
            Console.WriteLine("\nHomeGETData::" + getData);
            if (!String.IsNullOrEmpty(getData))
            {
                var quoteRecords = JsonConvert.DeserializeObject<QuoteModel>(getData.ToString());
                
                Console.WriteLine("\nMODEL:: " + quoteRecords);
                if (quoteRecords.records.Count > 0)
                {
                    ViewBag.aceess_token = access_token;
                    int numOfRecords = 0;
                    if (pageNum != 1)
                    {
                        numOfRecords = pageNum * 10;
                    } else
                    {
                        numOfRecords = pageNum * 10;
                    }
                    
                    ViewBag.quotes1 = quoteRecords.records.Take(numOfRecords).ToList();
                    int totalPage = Convert.ToInt32(Math.Ceiling((double)quoteRecords.records.Count() / 10));
                    ViewBag.TotalQuotes = totalPage;
                    TempData["PageNumber"] = pageNum;
                }
            }
            return View("Quotes");
        }

        public IActionResult singleQuote(string id)
        {
            Console.WriteLine("\n\nSing Quote ID:: " + id);
            //Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SFLogin log = new SFLogin();
            Console.WriteLine("\n\n\nAuthToken::"+ access_token +" :: InstanceURL:: "+ instance_url);
            if (access_token != "" && instance_url != "")
            {
                string getData = log.getQuery("select id, name, Apttus_Proposal__Approval_Stage__c, Apttus_Proposal__Net_Amount__c, " +
                    "Apttus_Proposal__Presented_Date__c from Apttus_Proposal__Proposal__c where id = '" + id + "'");
                Console.WriteLine("\nHomeGETData::" + getData);

                if (!String.IsNullOrEmpty(getData))
                {
                    var quoteRecords = JsonConvert.DeserializeObject<QuoteModel>(getData.ToString());
                    Console.WriteLine("\nMODEL:: " + quoteRecords.records[0].Name);
                    if (quoteRecords.records.Count > 0)
                    {
                        ViewBag.aceess_token = access_token;
                        ViewBag.quoteDetail = quoteRecords.records[0];
                    }
                }
            }
            return View("QuoteDetails");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}