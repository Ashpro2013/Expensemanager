namespace Expensemanager.Data
{
    public class Planning
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public int? Marks { get; set; }
        public int? Expected { get; set; }
        public int? Achieved { get; set; }
        public string? Remarks { get; set; }
        public int UserId { get; set; }
    }
}
