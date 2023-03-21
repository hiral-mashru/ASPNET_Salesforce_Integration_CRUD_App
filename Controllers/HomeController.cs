using Account_CRUD_App.Models;
using Account_CRUP_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Configuration;

namespace Account_CRUP_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult getAccount()
        {
            Console.WriteLine("\nIN HOME Controller\n");
            
            if (TempData.ContainsKey("accData"))
            {
                var accRecords = JsonConvert.DeserializeObject<AccountModel>(TempData["accData"].ToString());
                foreach (var item in accRecords.records)
                {
                    Console.WriteLine("\nMODEL:: " + item.Name);
                }
                Console.WriteLine("\nMODEL:: " + accRecords);
                if (accRecords.records.Count > 0)
                {
                    ViewBag.accRecords = accRecords.records;
                }
            }
            return View();
        }
        public IActionResult Index()
        {
            Console.WriteLine("\nIN HOME Controller\n");
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            if (config.AppSettings.Settings["access_token"].Value != "")
            {
                Console.WriteLine("\nIndex CONFIGG::" + config.AppSettings.Settings["access_token"].Value);
            }
            SFLogin log = new SFLogin();
            string getData = log.getQuery("select id, name from Apttus_Proposal__Proposal__c", config.AppSettings.Settings["instance_url"].Value, config.AppSettings.Settings["access_token"].Value);
            Console.WriteLine("\nHomeGETData::" + getData);
            if (!String.IsNullOrEmpty(getData))
            {
                var quoteRecords = JsonConvert.DeserializeObject<QuoteModel>(getData.ToString());
                foreach (var item in quoteRecords.records)
                {
                    Console.WriteLine("\nMODEL:: " + item.Name);
                }
                Console.WriteLine("\nMODEL:: " + quoteRecords);
                if (quoteRecords.records.Count > 0)
                {
                    ViewBag.quotes1 = quoteRecords.records;
                }
            }
            return View();
        }

        public IActionResult singleQuote(string id)
        {
            Console.WriteLine("\n\nSing Quote ID:: " + id);
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SFLogin log = new SFLogin();
            Console.WriteLine("\n\n\nAuthToken::"+ config.AppSettings.Settings["access_token"].Value +" :: InstanceURL:: "+ config.AppSettings.Settings["instance_url"].Value);
            if (config.AppSettings.Settings["access_token"].Value != "" && config.AppSettings.Settings["instance_url"].Value != "")
            {
                string getData = log.getQuery("select id, name from Apttus_Proposal__Proposal__c where id = '" + id + "'", config.AppSettings.Settings["instance_url"].Value, config.AppSettings.Settings["access_token"].Value);
                Console.WriteLine("\nHomeGETData::" + getData);

                if (!String.IsNullOrEmpty(getData))
                {
                    var quoteRecords = JsonConvert.DeserializeObject<QuoteModel>(getData.ToString());
                    Console.WriteLine("\nMODEL:: " + quoteRecords.records[0].Name);
                    if (quoteRecords.records.Count > 0)
                    {
                        ViewBag.quoteDetail = quoteRecords.records[0];
                    }
                }
            }
            return View("Privacy");
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