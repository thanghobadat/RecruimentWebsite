using ViewModel.Common;
using ViewModel.System.Users;

namespace MVCApp.ViewModels
{
    public class AccountsViewModel
    {
        public PageResult<CompanyAccountViewModel> CompanyAccount { get; set; }
        public PageResult<UserAccountViewModel> UserAccount { get; set; }
    }
}
