using Account_CRUD_App.Models;
using Account_CRUP_App.Controllers;
using Account_CRUP_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Immutable;
using System.Configuration;
using System.Xml.Linq;

namespace Account_CRUD_App.Controllers
{
    public class LoginController : Controller
    {
        /*private static string GetSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        private static void SetSetting(string key, string value)
        {
            Configuration configuration = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Full, true);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }*/
        private static Models.SFLogin createClientConnection()
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            return new Models.SFLogin
            {
                Username = appSettings["username"],
                Password = appSettings["password"],
                Token = appSettings["token"],
                ClientId = appSettings["clientId"],
                ClientSecret = appSettings["clientSecret"]
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult login1(String username, String password)
        {
            SFLogin log = new SFLogin();
            Dictionary<string, string> response = log.authLogin(username, password);
            Console.WriteLine("\n\n\nNEWW:: "+response);
            if (response.ContainsKey("error"))
            {
                String loginError = "Please provide valid Username or Password";
                return BadRequest(new { Message = loginError });
            }
            else
            {
                HomeController.access_token = response["access_token"];
                HomeController.instance_url = response["instance_url"];
                //Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //config.AppSettings.Settings["access_token"].Value = response["access_token"];
                //config.AppSettings.Settings["instance_url"].Value = response["instance_url"];
                //config.Save(ConfigurationSaveMode.Modified);
                //System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                //Console.WriteLine("\nCONFIG::" + config.AppSettings.Settings["access_token"].Value+"::"+ config.AppSettings.Settings["instance_url"].Value);
                
                /*TempData["AuthToken"] = response["access_token"];
                TempData["InstanceURL"] = response["instance_url"];
                TempData.Keep("AuthToken");
                TempData.Keep("InstanceURL");*/
                
                return RedirectToAction("Index", "Home");
            }
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult login(String username, String password)
        {
            try
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["username"].Value = username;
                config.AppSettings.Settings["password"].Value = password;
                config.Save(ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                Console.WriteLine("CONFIG::"+ config.AppSettings);
                var conn = createClientConnection();
                bool ans = conn.login();
                if (ans == true)
                {
                    string getData = conn.getQuery("select id, name from Apttus_Proposal__Proposal__c","","");
                    TempData["quoteData"] = getData;
                    Console.WriteLine("\nGetData:: " + getData);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    String loginError = "Please provide valid Username or Password";
                    return BadRequest(new { Message = loginError });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }*/

        /*public IActionResult Index()
        {
            return View();
        }*/
        public ActionResult logout() 
        {
            try
            {
                HomeController.access_token = "";
                HomeController.instance_url = "";
                /*Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["username"].Value = "";
                config.AppSettings.Settings["password"].Value = "";
                config.Save(ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                Console.WriteLine("CONFIG::" + config.AppSettings);
                var conn = createClientConnection();
                bool ans = conn.login();
                if (ans == true)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }*/
                return RedirectToAction("Login", "Home");
            } catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
