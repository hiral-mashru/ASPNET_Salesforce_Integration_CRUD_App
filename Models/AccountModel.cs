using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Account_CRUP_App.Models
{
    public class AccountModel
    {
        public int? totalSize { get; set; }
        public Boolean? done { get; set; }
        public List<Records>? records { get; set; }
    }

    public class Records
    {
        public Dictionary<string,string>? attributes { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Region__c { get; set; }
        public string? Type { get; set; }
        public string? Customer_Rating__c { get; set; }
    }
}
