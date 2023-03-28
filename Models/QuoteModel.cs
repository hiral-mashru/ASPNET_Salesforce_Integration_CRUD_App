namespace Account_CRUP_App.Models
{
    public class QuoteModel
    {
        public int? totalSize { get; set; }
        public Boolean? done { get; set; }
        public List<QuoteRecords>? records { get; set; }
    }
    public class QuoteRecords
    {
        public Dictionary<string, string>? attributes { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Apttus_Proposal__Approval_Stage__c { get; set; }
        public double? Apttus_Proposal__Net_Amount__c { get; set; }
        public DateOnly? Apttus_Proposal__Presented_Date__c { get; set; }
    }
}
