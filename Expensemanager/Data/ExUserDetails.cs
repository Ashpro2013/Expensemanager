namespace Expensemanager.Data
{
    public class ExUserDetails : UserDetails
    {
        public string OldPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
