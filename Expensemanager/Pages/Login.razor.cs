using Expensemanager.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace Expensemanager.Pages
{
public partial class Login
{
        string? alerMessage;
        ExpenseDBContext db;
        [Inject]
        private IDbContextFactory<ExpenseDBContext> _dbContextFactory { get; set; }
        [Inject]
        private ProtectedSessionStorage storage { get; set; }

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
                await storage.SetAsync("UserId", user.Id);
                navManager.NavigateTo("index", true);
            }
            else
            {
                user = new UserDetails();
                alerMessage = "Incorrect Username/Password!";
            }
        }
    }
}
