using System;
using Account_CRUP_App.Models;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Nodes;

namespace Account_CRUD_App.Models 
{
    public class SFLogin
    {
        public const string LoginEndpoint = "https://login.salesforce.com/services/oauth2/token";
        public const string ApiEndpoint = "/services/data/v36.0/"; //Use your org's version number
        
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public static string AuthToken;
        public static string InstanceUrl;

        static readonly HttpClient Client;
       
        static SFLogin()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public Dictionary<string,string> authLogin(string username, string password)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://login.salesforce.com/services/oauth2/token");
            request.Headers.Add("Authorization", "Bearer TsLxmAO9tvcTH3ciba3RdbeQP");
            request.Headers.Add("Cookie", "BrowserId=DbVwxcNeEe2U8j0TtFIreA; CookieConsentPolicy=0:0; LSKey-c$CookieConsentPolicy=0:0");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("grant_type", "password"));
            collection.Add(new("client_id", "3MVG9X12xD2kqQman8DJNEQSNqWZdTBzGEKwyYb5lNKwbWVtHUr..uzzp4VaGbafy.A8sf6EccWzgyAqQkjTg"));
            collection.Add(new("client_secret", "D713EBE1FD515A283FE3E87E553B9A6148B4AF3A0DC111163283A956B93564E4"));
            collection.Add(new("username", username));
            collection.Add(new("password", password+"TsLxmAO9tvcTH3ciba3RdbeQP"));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = client.SendAsync(request).Result;
            //response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
            AuthToken = values["access_token"];
            InstanceUrl = values["instance_url"];
            //return response.Content.ReadAsStringAsync().Result;
            return values;
        }
        public Boolean login()
        {
            String jsonResponse;
            using (var client = new HttpClient())
            {
                var request = new FormUrlEncodedContent(new Dictionary<String, String>
                {
                    {"grant_type","password"},
                    {"client_id", ClientId},
                    {"client_secret", ClientSecret},
                    {"username", Username},
                    {"password", Password + Token}
                });
                request.Headers.Add("X-PreetyPrint", "1");
                var response = client.PostAsync(LoginEndpoint, request).Result;
                jsonResponse = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("JSONRESPONSE"+jsonResponse);
            }
            if (jsonResponse.Contains("error"))
            {
                return false;
            }
            else
            {
                Console.WriteLine(jsonResponse);
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
                AuthToken = values["access_token"];
                AuthModel auth1 = new AuthModel();
                auth1.AuthToken = AuthToken;
                auth1.InstanceUrl = InstanceUrl;
                //HttpContext.Session.SetString("access_token", AuthToken);
                InstanceUrl = values["instance_url"];
                Console.Write("Access Token: " + AuthToken);
                return true;
            }
        }

        public string getQuery(string soqlQuery)
        {
            using(var client = new HttpClient())
            {
                string restRequest = InstanceUrl + "/services/data/v36.0/" + "query?q=" + soqlQuery;
                var request = new HttpRequestMessage(HttpMethod.Get, restRequest);
                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-PreetyPrint", "1");
                var response = client.SendAsync(request).Result;
                Console.WriteLine("RESPONSE::"+response);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.Content.ReadAsStringAsync().Result;
                } else
                {
                    return "ERROR:: StatusCode: " + response.StatusCode + " - " + response.Content.ReadAsStringAsync().Result;
                }
                
            }
        }

        public Boolean takeQuery(string input)
        {
            var methd = (HttpMethod?)null;
            using (var client = new HttpClient())
            {
                string idStr = "";
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(input);
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
                    if(entry.Key == "Id")
                    {
                        idStr = "/"+entry.Value;
                        continue;
                    }
                    cont += "\r\n    \"" + entry.Key + "\": \"" + entry.Value + "\"";
                    Console.WriteLine("\ncont++ "+cont);
                    i++;
                }
                cont = cont + "\r\n}";
                Console.WriteLine("\nSTRING::" + cont +"::"+idStr);
                
                if (idStr != "")
                {
                    methd = HttpMethod.Patch;
                } else
                {
                    methd = HttpMethod.Post;
                }
                Console.WriteLine("\nMethod::" + methd);
                var request = new HttpRequestMessage(methd, InstanceUrl + "/services/data/v57.0/sobjects/Account"+idStr);
                request.Headers.Add("Authorization", "Bearer "+AuthToken);
                //request.Headers.Add("Content-Type", "application/json");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("Cookie", "BrowserId=DbVwxcNeEe2U8j0TtFIreA; CookieConsentPolicy=0:1; LSKey-c$CookieConsentPolicy=0:1");

                var content = new StringContent(cont, null, "application/json");
                request.Content = content;

                var response = client.SendAsync(request).Result;
                Console.WriteLine("\n\nRESPONSE::"+response.Content.ReadAsStringAsync().Result);
                response.EnsureSuccessStatusCode();
                string s = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            //return "Done";
        }

        public Boolean deleteQuery(string id)
        {
            var client = new HttpClient();
            Console.WriteLine("\nIDD:" + id);
            var request = new HttpRequestMessage(HttpMethod.Delete, InstanceUrl + "/services/data/v57.0/sobjects/Account/"+id);
            request.Headers.Add("Authorization", "Bearer "+AuthToken);
            request.Headers.Add("Cookie", "BrowserId=DbVwxcNeEe2U8j0TtFIreA; CookieConsentPolicy=0:1; LSKey-c$CookieConsentPolicy=0:1");
            var content = new StringContent("", null, "text/plain");
            request.Content = content;
            var response = client.SendAsync(request).Result;
            //response.EnsureSuccessStatusCode();
            Console.WriteLine("\nDelete::"+response.Content.ReadAsStringAsync().Result);

            return true;
        }
    }
}