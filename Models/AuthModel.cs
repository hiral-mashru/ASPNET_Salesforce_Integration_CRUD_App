namespace Account_CRUP_App.Models
{
    public class AuthModel
    {
        public const string ApiEndpoint = "/services/data/v36.0/"; //Use your org's version number
        public string AuthToken { get; set; }
        public string InstanceUrl { get; set; }

        
    }
}
