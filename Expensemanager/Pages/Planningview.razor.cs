using Expensemanager.Data;
using Expensemanager.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Expensemanager.Pages
{
    public partial class Planningview
    {
        int? iMasterId = null;
        private List<Planning> _plannings = new();
        ExpenseDBContext db;
        private Planning planning { get; set; } = new();
        public DateTime dtmDate { get; set; }
        [Inject]
        private IDbContextFactory<ExpenseDBContext> _dbContextFactory { get; set; }
        protected Confirm Deleteconfirmation { get; set; }
        string dialog = "";
        string ButtonText = "";
        protected override async Task OnInitializedAsync()
        {
            db = await _dbContextFactory.CreateDbContextAsync();
            planning = new Planning();
            planning.Date = dtmDate = DateTime.Now;
            LoadData();
            await base.OnInitializedAsync();
        }
        private void Search()
        {
            _plannings = new List<Planning>();
            _plannings.AddRange(db.plannings.Where(x => x.Date >= dtmDate && x.Date<=dtmDate).ToList());
            StateHasChanged();
        }
        private void LoadData()
        {
            _plannings.Clear();
            _plannings.AddRange(db.plannings.Where(x=> x.Date>=DateTime.Now.Date && x.Date<=DateTime.Now));
            StateHasChanged();
        }
        private async Task Create(Planning planning)
        {
            if (iMasterId == null)
            {
                await db.plannings.AddAsync(planning);
            }
            else
            {
                db.plannings.Update(planning);
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
                var car = await db.plannings.FindAsync(iMasterId);
                db.plannings.Remove(car);
                await db.SaveChangesAsync();
                LoadData();
                Clear();
            }
        }
        private void Clear()
        {
            planning = new Planning();
            planning.Date = DateTime.Now;
            iMasterId = null;
            StateHasChanged();
        }
        private void SetForUpdate(Planning pln)
        {
            planning = pln;
            iMasterId = pln.Id;
        }
    }
}
