using Expensemanager.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Expensemanager.Pages
{
    public partial class Index
    {
        ExpenseDBContext db;
        [Inject]
        private IDbContextFactory<ExpenseDBContext> _dbContextFactory { get; set; }

        string sCash="0.00";
        string sBank = "0.00";
        string sExpense;
        string sIncome;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                db = await _dbContextFactory.CreateDbContextAsync();
                if (!AppData.isLogin)
                {
                    navManager.NavigateTo("");
                }
                DashboardLoadMethod();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
        void DashboardLoadMethod()
        {
            List<Data.Transaction> transactions = new ();
            transactions = db.Transactions.Where(x=> x.UserId == AppData.UserId).ToList();
            sExpense = transactions.Sum(x => x.Debit).ToString("N2");
            sIncome = transactions.Sum(x => x.Credit).ToString("N2");
            sBank = transactions.Where(x => x.Mode == "Bank").Sum(x => x.Credit-x.Debit).ToString("N2");
            sCash = transactions.Where(x => x.Mode == "Cash").Sum(x => x.Credit - x.Debit).ToString("N2");
        }
    }
}
