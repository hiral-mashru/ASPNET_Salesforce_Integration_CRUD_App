using Newtonsoft.Json.Linq;

namespace Account_CRUP_App.Models
{
    public class JsonResultModel
    {
        public IEnumerable<JObject> Results { get; set; }
    }
}
