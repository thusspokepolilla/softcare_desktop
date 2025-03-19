using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace softcare_desktop
{
    public class AdminViewmodel : ObservableObject
    {
        private AdminPageList _adminPageList;

        public AdminPageList AdminCurrentPage
        {
            get { return _adminPageList; }
            set { SetProperty(ref _adminPageList, value); }
        }

        public ICommand CMDAdminChangePage => new RelayCommand<AdminPageList>(AdminChangePage);

        private void AdminChangePage(AdminPageList newPage)
        {
            AdminCurrentPage = newPage;
        }

        public AdminViewmodel() { AdminCurrentPage = AdminPageList.AdminDashboard; }
    }
}
