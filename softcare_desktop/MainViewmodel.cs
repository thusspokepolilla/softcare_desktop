using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace softcare_desktop
{
    public class MainViewmodel : ObservableObject
    {
        private AdminPageList _adminPageList;

        public AdminPageList AdminCurrentPage
        {
            get { return _adminPageList; }
            set { SetProperty(ref _adminPageList, value); }
        }

        public ICommand CMDChangePage => new RelayCommand<AdminPageList>(ChangePage);

        private void ChangePage(AdminPageList newPage)
        {
            AdminCurrentPage = newPage;
        }

        public MainViewmodel() { AdminCurrentPage = AdminPageList.AdminDashboard;  }
    }
}
