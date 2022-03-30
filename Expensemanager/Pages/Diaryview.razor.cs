using WYSIWYGTextEditor;
using Expensemanager.Data;
using Expensemanager.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace Expensemanager.Pages
{
    public partial class Diaryview
    {
        TextEditor MyEditor;

        int? iMasterId = null;
        private List<Diary> _diaries = new();
        ExpenseDBContext db;
        public Diary diary { get; set; }
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
            diary = new Diary();
            diary.DateOfDiary = FromDate = ToDate = DateTime.Now;
            LoadData();
            await base.OnInitializedAsync();
        }
        private void Search()
        {
            _diaries = new List<Diary>();
            _diaries.AddRange(db.diaries.Where(x => x.UserId==AppData.UserId && x.DateOfDiary >= FromDate && x.DateOfDiary <= ToDate).ToList());
            StateHasChanged();
        }
        private void LoadData()
        {
            _diaries.Clear();
            _diaries.AddRange(db.diaries.Where(x=> x.UserId==AppData.UserId).OrderByDescending(x => x.DateOfDiary));
            StateHasChanged();
        }
        private async Task Create(Diary diary)
        {
            diary.Diarynote = await MyEditor.GetHTML();
            diary.UserId = AppData.UserId.ToInt32();
            if (iMasterId == null)
            {
                await db.diaries.AddAsync(diary);
            }
            else
            {
                db.diaries.Update(diary);
            }
            await db.SaveChangesAsync();
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
                var diary = await db.diaries.FindAsync(iMasterId);
                db.diaries.Remove(diary);
                await db.SaveChangesAsync();
                Clear();
            }
        }
        private void Clear()
        {
            diary = new Diary();
            diary.DateOfDiary = DateTime.Now;
            iMasterId = null;
            LoadData();
            StateHasChanged();
        }
        private async void SetForUpdate(Diary _diary)
        {
            diary = _diary;
          await  MyEditor.LoadHTMLContent(diary.Diarynote);
            iMasterId = diary.Id;
        }
    }
}
