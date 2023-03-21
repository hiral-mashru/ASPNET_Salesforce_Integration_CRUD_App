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
    }
}
