using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Expensemanager.Data;
using Expensemanager.Shared;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Expensemanager.Pages;
public partial class Changepassword
{
    [Inject]
    private IDbContextFactory<ExpenseDBContext> _dbContextFactory { get; set; }
    ExpenseDBContext db;
    private ExUserDetails user { get; set; } = new();
    UserDetails userDetails;
    string? alerMessage;
    protected override async Task OnInitializedAsync()
    {
        db = await _dbContextFactory.CreateDbContextAsync();
        userDetails = new UserDetails();
        userDetails = await db.userDetails.FirstOrDefaultAsync(x => x.Id == AppData.UserId);
        user = new ExUserDetails();
        user.Name = userDetails.Name;
         await base.OnInitializedAsync();
    }
    private void Create(ExUserDetails exUser)
    {
        if(exUser.OldPassword != userDetails.Password) { alerMessage = "Old Password not correct";return; }
        if(exUser.Password != exUser.ConfirmPassword) { alerMessage = "Confirmation Password not match"; return; }
        UserDetails uDetails = new UserDetails();
        uDetails.Id = exUser.Id;
        uDetails.Name = exUser.Name;
        uDetails.Password = exUser.ConfirmPassword;
        userDetails.Password = exUser.Password;
        db.userDetails.Update(uDetails);
        Clear();
    }
    private void Clear()
    {
        user = new ExUserDetails();
        user.Name = userDetails.Name;
        user.OldPassword = userDetails.Password;
        alerMessage = string.Empty;
        StateHasChanged();
    }
}
