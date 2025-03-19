using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace softcare_desktop
{
    public class DoctorViewmodel : ObservableObject
    {
        private DoctorPageList _doctorPageList;

        public DoctorPageList DoctorCurrentPage
        {
            get { return _doctorPageList; }
            set { SetProperty(ref _doctorPageList, value); }
        }

        public ICommand CMDDoctorChangePage => new RelayCommand<DoctorPageList>(DoctorChangePage);

        private void DoctorChangePage(DoctorPageList newPage)
        {
            DoctorCurrentPage = newPage;
        }

        public DoctorViewmodel() { DoctorCurrentPage = DoctorPageList.DoctorDashboard; }
    }
}
