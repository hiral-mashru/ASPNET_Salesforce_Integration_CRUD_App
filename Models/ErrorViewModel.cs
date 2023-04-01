namespace Account_CRUP_App.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        //public List<Records>? errRecord { get; set; }
        public string? message { get; set; }
        public string? errorCode { get; set; }

    }

    /*public class errRecord
    {
        public string? message { get; set; }
        public string? errorCode { get; set; }
    }*/
}