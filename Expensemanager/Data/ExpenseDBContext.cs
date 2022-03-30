using Microsoft.EntityFrameworkCore;

namespace Expensemanager.Data
{
    public class ExpenseDBContext:DbContext
    {
        public ExpenseDBContext(DbContextOptions<ExpenseDBContext> options) :
   base(options)
        {
        }
        public DbSet<Transaction> Transactions { get; set; } = default!;
        public DbSet<UserDetails> userDetails { get; set; }
        public DbSet<Diary> diaries { get; set; }
        public DbSet<Planning> plannings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(expense =>
            {
                expense.HasKey(e => e.Id);
                expense.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<UserDetails>(x =>
            {
                x.HasKey(e => e.Id);
                x.Property(e => e.Id).ValueGeneratedOnAdd();
                x.HasData(new UserDetails { Id = 1, Name = "Admin", Password = "5320" });
            });
            modelBuilder.Entity<Diary>(x =>
            {
                x.HasKey(e => e.Id);
                x.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Planning>(x =>
            {
                x.HasKey(e => e.Id);
                x.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
