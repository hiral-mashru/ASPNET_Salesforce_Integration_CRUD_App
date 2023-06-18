using Account_CRUD_App.Models;
using Account_CRUP_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Account_CRUP_App.Controllers
{
    public class HomeController : Controller
    {
        AccountCRUD accCRUD = new AccountCRUD();
        List<string> fields = new List<string>() { "Id", "Name", "NumberOfEmployees", "ShippingState", "ShippingCountry", "Phone", "Fax", "Apttus_Billing__SLASerialNumber__c", "BillingCity", "BillingState", "BillingCountry" };

        private readonly ILogger<HomeController> _logger;
        public static string access_token;
        private static string instance_url;

        public static string Instance_url { get => instance_url; set => instance_url = value; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult getAccount()
        {
            if (access_token != "") {
                Console.WriteLine("\nIN HOME Controller:: ");
                /*List<Account> response = new List<Account>();
                response = accCRUD.Read(1, 10000, fields);
                Console.WriteLine("\nress::" + response.ToString() + "--" + response.Count());
                ViewBag.accData = response;
                //TempData["totalAcc"] = response.Count();
                ViewBag.totalAcc = response.Count();*/
                SFLogin log = new SFLogin();
                string getData = log.getData("select COUNT(ID) from Account");
                Console.WriteLine("\nHomeGETData::" + getData);
                if (!String.IsNullOrEmpty(getData))
                {
                    var accRecords = JsonConvert.DeserializeObject<AccountModel>(getData.ToString());

                    Console.WriteLine("\nMODEL:: " + accRecords.records[0].expr0);
                    if (accRecords.records[0].expr0 > 0)
                    {
                        ViewBag.totalAcc = accRecords.records[0].expr0;
                    }
                }
                return View("Accounts");
            } else
            {
                //TempData["errMsg"] = "Access Token is missing...";
                return View("Login");
            }
            return View("Accounts");
        }

        public JsonResult accList(int pageNum, int pageSize)
        {
            Console.WriteLine("\n\nPageNum::" + pageNum + "::" + pageSize);
            List<Account> response = accCRUD.Read(pageNum, pageSize, fields);
            Console.WriteLine("\nress::" + response.ToString + "--");
            foreach(var record in response)
            {
                Console.WriteLine("Rec::"+record);
            }
            //ViewBag.accData = response;
            return Json(response);
        }

        public IActionResult singleAcc(string id)
        {
            if (access_token != "")
            {
                if (TempData.ContainsKey("createResponse"))
                {
                    Console.WriteLine("\nAfter Create::" + id);
                }
                Account response = accCRUD.Read(id, fields);
                Console.WriteLine("\nress::" + response.ToString + "--");
                if(response != null)
                {
                    ViewBag.aceess_token = access_token;
                    ViewBag.accDetail = response;
                }
            }
            else
            {
                //TempData["errMsg"] = "You are Logged out...";
                return View("Login");
            }
            return View("AccDetails");
        }

        //[Route("Home/createAccount")]
        public IActionResult showCreateAcc()
        {
            if (access_token != "")
            {
                return View("CreateAcc");
            } else
            {
                return View("Login");
            }
        }

        //[HttpPost]
        public ActionResult createAccount(Account accRecord)
        {
            if (access_token != "")
            {
                Console.WriteLine("\nINN");
                string response = accCRUD.Create(accRecord);
                if (response.Length == 18)
                {
                    Console.WriteLine("INNNN " + response);
                    return RedirectToAction("getAccount", "Home");
                    //return RedirectToAction("singleAcc", "Home", new { response });
                }
                else
                {
                    return View("CreateAcc");
                }
            } else
            {
                return View("Login");
            }
        }

        public IActionResult showUpdateAccount(string id)
        {
            if (access_token != "") { 
                Account response = accCRUD.Read(id, fields);
                if (response != null)
                {
                    ViewBag.aceess_token = access_token;
                    ViewBag.accDetail = response;
                }

                return View("UpdateAccount");
            } else
            {
                return View("Login");
            }
        }

        /*public JsonResult updateAccount1(string input, string id)
        {
            Console.WriteLine("\nUPDATE:: " + id + " :: " + input);
            string res = accCRUD.Update(id, input);
            Console.WriteLine("\nAfter Update::" + res);
            if (res.Length == 18)
            {
                TempData["createResponse"] = res;
                Console.WriteLine("INNNN");
                //singleAcc(response);
                List<String> response = new List<String>();
                response.Add(res); 
                Console.WriteLine("res::"+ response[0]);
                return Json(response);
            }
            else
            {
                return Json("ERROR");
            }
        }*/

        public ActionResult updateAccount(Account accRecords)
        {
            /*Records accRecords = new Records();
            accRecords.Id = id;
            accRecords.Name = name;
            accRecords.Region__c = region;
            accRecords.Customer_Rating__c = rating;
            accRecords.Type = type;
            accRecords.Phone = Phone;
            accRecords.Fax = Fax;
            accRecords.Apttus_Billing__SLASerialNumber__c = serialNum;
            accRecords.BillingCity = billingCity;
            accRecords.BillingState = billingState;
            accRecords.BillingCountry = billingCountry;*/

            //Console.WriteLine("\nUPDATE:: " + id+" :: "+input);
            if (access_token != "")
            {
                string response = accCRUD.Update(accRecords.Id, accRecords);
                Console.WriteLine("\nAfter Update::" + response);
                if (response.Length == 18)
                {
                    TempData["createResponse"] = response;
                    Console.WriteLine("INNNN");
                    //singleAcc(response);
                    //return RedirectToAction("singleAcc", "Home", new { id = response });
                    return RedirectToAction("getAccount", "Home");
                }
                else
                {
                    return View("UpdateAccount");
                }
            } else
            {
                return View("Login");
            }
        }
        public ActionResult deleteAccount(string id)
        {
            if(accCRUD.Delete(id))
            {
                TempData["status"] = "Account is Deleted.";
            }

            return RedirectToAction("getAccount","Home");
        }
        public IActionResult Index()
        {
            if (access_token != "")
            {
                Console.WriteLine("\nIN HOME Controller\n" + Instance_url + ":::" + access_token);

                SFLogin log = new SFLogin();
                string getData = log.getData("select COUNT(ID) from Apttus_Proposal__Proposal__c");
                Console.WriteLine("\nHomeGETData::" + getData);
                if (!String.IsNullOrEmpty(getData))
                {
                    var quoteRecords = JsonConvert.DeserializeObject<QuoteModel>(getData.ToString());

                    Console.WriteLine("\nMODEL:: " + quoteRecords.records[0].expr0);
                    if (quoteRecords.records[0].expr0 > 0)
                    {
                        //ViewBag.aceess_token = access_token;
                        //ViewBag.quotes1 = quoteRecords.records;//.Take(numOfRecords).ToList();
                        ViewBag.TotalQuotes = quoteRecords.records[0].expr0;
                    }
                }
                return View("Quotes");
            } else
            {
                return View("Login");
            }
        }

        public JsonResult quoteList(int pageNum, int pageSize)
        {
            Console.WriteLine("\n\nPageNum::" + pageNum + "::" + pageSize);
            SFLogin log = new SFLogin();
            string getData = log.getData("select id, name, Apttus_Proposal__Approval_Stage__c, Apttus_Proposal__Net_Amount__c from Apttus_Proposal__Proposal__c ORDER BY LastModifiedDate DESC limit " + pageSize + " offset " + ((pageNum - 1) * pageSize));
            Console.WriteLine("\nHomeGETData::" + getData);
            List<QuoteRecords> response = new List<QuoteRecords>();
            if (!String.IsNullOrEmpty(getData))
            {
                var quoteRecords = JsonConvert.DeserializeObject<QuoteModel>(getData.ToString());
                Console.WriteLine("\nMODEL:: " + quoteRecords);
                response = quoteRecords.records.ToList<QuoteRecords>();
                
            }
            return Json(response);
        }

        public IActionResult singleQuote(string id)
        {
            if (access_token != "")
            {
                Console.WriteLine("\n\nSing Quote ID:: " + id);
                //Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                SFLogin log = new SFLogin();
                Console.WriteLine("\n\n\nAuthToken::" + access_token + " :: InstanceURL:: " + Instance_url);
                if (access_token != "" && Instance_url != "")
                {
                    string getData = log.getData("select id, name, Apttus_Proposal__Approval_Stage__c, Apttus_Proposal__Net_Amount__c, " +
                        "Apttus_Proposal__Presented_Date__c from Apttus_Proposal__Proposal__c where id = '" + id + "'");
                    Console.WriteLine("\nHomeGETData::" + getData);

                    if (!String.IsNullOrEmpty(getData))
                    {
                        var quoteRecords = JsonConvert.DeserializeObject<QuoteModel>(getData.ToString());
                        Console.WriteLine("\nMODEL:: " + quoteRecords.records[0].Name);
                        if (quoteRecords.records.Count > 0)
                        {
                            if (TempData.ContainsKey("isReprised"))
                            {
                                TempData["reprised"] = "The Cart is Reprised.";
                                TempData.Remove("isReprised");
                            }
                            ViewBag.aceess_token = access_token;
                            ViewBag.quoteDetail = quoteRecords.records[0];
                        }
                    }
                }
                return View("QuoteDetails");
            } else
            {
                return View("Login");
            }
        }

        public ActionResult reprice(string id)
        {
            SFLogin log = new SFLogin();
            string getCart = "select Id from Apttus_Config2__ProductConfiguration__c where Apttus_QPConfig__Proposald__r.Id='" + id + "' ORDER BY LastModifiedDate DESC limit 1";
            string gCart = log.getData(getCart);
            Console.WriteLine("\nGCART:: " + gCart);
            var cart = JsonConvert.DeserializeObject<QuoteModel>(gCart.ToString());
            if (cart.records[0] != null)
            {
                Console.WriteLine("Cart Id " + cart.records[0].Id);
                bool cartCheck = log.repriceCart(cart.records[0].Id);
                Console.WriteLine("cart Check: " + cartCheck);
                if (cartCheck = true)
                {
                    TempData["isReprised"] = "The Cart is Reprised.";
                    //return BadRequest(new { Message = "The Cart is Reprised." });
                }
            }
            return RedirectToAction("singleQuote", "Home", new { id });
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