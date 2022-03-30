using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;
using Expensemanager.Data;
using Expensemanager.Shared;

namespace Expensemanager.Pages
{
    public partial class Expensedetails
    {
        int? iMasterId = null;
        private  List<Transaction> _expenses = new();
        ExpenseDBContext db;
        private Transaction expense { get; set; } = new();
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        [Inject]
        private IDbContextFactory<ExpenseDBContext> _dbContextFactory { get; set; }
        protected Confirm Deleteconfirmation { get; set; }
        string dialog = "";
        string ButtonText = "";
        protected override async Task OnInitializedAsync()
        {
            db = await _dbContextFactory.CreateDbContextAsync();
            expense = new Transaction();
            expense.Date =FromDate=ToDate= DateTime.Now;
            expense.Mode = null;
            LoadData();
            await base.OnInitializedAsync();
        }
        private void Search()
        {
            _expenses = new List<Transaction>();
            _expenses.AddRange(db.Transactions.Where(x=> x.Date>=FromDate && x.Date<=ToDate).ToList());
            StateHasChanged();
        }
        private void LoadData()
        {
            _expenses.Clear();
            _expenses.AddRange(db.Transactions.OrderByDescending(x=> x.Date));
            StateHasChanged();
        }
        private async Task Create(Transaction expense)
        {
            if (iMasterId == null)
            {
                await db.Transactions.AddAsync(expense);
            }
            else
            {
                db.Transactions.Update(expense);
            }
            await db.SaveChangesAsync();
            LoadData();
            Clear();
        }
        private void Delete()
        {
            dialog = "Are you sure you want delete?";
            ButtonText = "Delete";
            Deleteconfirmation.Show();
        }
        protected async Task ConfirmDelete_Click(bool DeleteConfirmed)
        {
            if (DeleteConfirmed)
            {
                var car = await db.Transactions.FindAsync(iMasterId);
                db.Transactions.Remove(car);
                await db.SaveChangesAsync();
                LoadData();
                Clear();
            }
        }
        private void Clear()
        {
            expense = new Transaction();
            expense.Date = DateTime.Now;
            iMasterId = null;
            StateHasChanged();
        }
        private void SetForUpdate(Transaction exp)
        {
            expense = exp;
            iMasterId = exp.Id;
        }
    }
}
