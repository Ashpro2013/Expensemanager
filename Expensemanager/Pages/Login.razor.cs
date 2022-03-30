using Expensemanager.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace Expensemanager.Pages
{
public partial class Login
{
        ExpenseDBContext db;
        [Inject]
        private IDbContextFactory<ExpenseDBContext> _dbContextFactory { get; set; }
        public UserDetails user { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                db = await _dbContextFactory.CreateDbContextAsync();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
        protected override void OnAfterRender(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    AppData.isLogin = false;
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
        private async Task Login_Click()
        {
            user = await db.userDetails.FirstOrDefaultAsync(x => x.Name == user.Name && x.Password == user.Password);
            if (user != null)
            {
                AppData.isLogin = true;
                AppData.UserName = user.Name;
                AppData.UserId = user.Id;
                navManager.NavigateTo("index", true);
            }
        }
    }
}
