using Expensemanager.Data;
using Expensemanager.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Expensemanager.Pages
{
    public partial class Userdetailsview
    {
        int? iMasterId = null;
        private List<UserDetails> _userDetails = new();
        ExpenseDBContext db;
        private UserDetails user { get; set; } = new();
        [Inject]
        private IDbContextFactory<ExpenseDBContext> _dbContextFactory { get; set; }
        protected Confirm Deleteconfirmation { get; set; }
        string dialog = "";
        string ButtonText = "";

        protected override async Task OnInitializedAsync()
        {
            db = await _dbContextFactory.CreateDbContextAsync();
            user = new UserDetails();
            LoadData();
            await base.OnInitializedAsync();
        }
        private void LoadData()
        {
            _userDetails.Clear();
            _userDetails.AddRange(db.userDetails.ToList());
            StateHasChanged();
        }
        private async Task Create(UserDetails user)
        {
            if (iMasterId == null)
            {
                await db.userDetails.AddAsync(user);
            }
            else
            {
                db.userDetails.Update(user);
            }
            await db.SaveChangesAsync();
            LoadData();
            Clear();
        }
        private void Delete()
        {
            if (user.Name == "Admin")
            {
                dialog = "You can't delete Admin User!";
                ButtonText = "Ok";
            }
            else
            {
                dialog = "Are you sure you want delete?";
                ButtonText = "Delete";
            }
            Deleteconfirmation.Show();
        }
        protected async Task ConfirmDelete_Click(bool DeleteConfirmed)
        {
            if (DeleteConfirmed)
            {
                var car = await db.userDetails.FindAsync(iMasterId);
                db.userDetails.Remove(car);
                await db.SaveChangesAsync();
                LoadData();
                Clear();
            }
        }
        private void Clear()
        {
            user = new UserDetails();
            iMasterId = null;
            StateHasChanged();
        }
        private void SetForUpdate(UserDetails usr)
        {
            user = usr;
            iMasterId = usr.Id;
        }
    }
}
