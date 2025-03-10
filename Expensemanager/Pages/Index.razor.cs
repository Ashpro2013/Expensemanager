using Expensemanager.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.WebRequestMethods;

namespace Expensemanager.Pages
{
    public partial class Index
    {
        ExpenseDBContext db;
        [Inject]
        private IDbContextFactory<ExpenseDBContext> _dbContextFactory { get; set; }
        [Inject]
        private ProtectedSessionStorage storage { get; set; }
        string sCash="0.00";
        string sBank = "0.00";
        string sExpense;
        string sIncome;
        List<Data.Transaction> entIncome;
        List<Data.Transaction> entExpense;
        int? iuserId=null;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                db = await _dbContextFactory.CreateDbContextAsync();
                if (!AppData.isLogin)
                {
                    navManager.NavigateTo("/");
                }
                DashboardLoadMethod(AppData.UserId);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var value = await storage.GetAsync<int>("UserId");
                iuserId = value.Value;
                if (iuserId == null || iuserId==0)
                {
                    navManager.NavigateTo("/");
                }
                DashboardLoadMethod(iuserId);
                StateHasChanged();
            }
        }
        void DashboardLoadMethod(int? iuser)
        {
            List<Data.Transaction> transactions = new ();
            transactions = db.Transactions.Where(x=> x.UserId == iuser).ToList();
            sExpense = transactions.Sum(x => x.Debit).ToString("N2");
            sIncome = transactions.Sum(x => x.Credit).ToString("N2");
            sBank = transactions.Where(x => x.Mode == "Bank").Sum(x => x.Credit-x.Debit).ToString("N2");
            sCash = transactions.Where(x => x.Mode == "Cash").Sum(x => x.Credit - x.Debit).ToString("N2");
            entExpense = transactions.Where(x=> x.Credit == 0).ToList();
            entIncome = transactions.Where(x=> x.Debit==0).ToList();
        }
    }
}
