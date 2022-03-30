namespace Expensemanager.Data
{
    public class Transaction
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Mode { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public DateTime Date { get; set; }
        public string? Narration { get; set; }
    }
}
