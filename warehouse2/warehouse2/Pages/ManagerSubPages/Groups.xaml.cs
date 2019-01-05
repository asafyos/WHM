using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace warehouse2 {
    /// <summary>
    /// Interaction logic for Groups.xaml
    /// </summary>
    public partial class Groups : Page, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        SharedData sharedDataIns;
        string groupName;
        public SharedData SharedDataIns {
            get { return sharedDataIns; }
            set {
                sharedDataIns = value;
                OnPropertyChanged("SharedDataIns");
            }
        }
        public string GroupName {
            get { return this.groupName; }
            set {
                this.groupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        public Groups() {
            InitializeComponent();
            this.SharedDataIns = SharedData.GetInstans();
        }

        private void Enabled_Button_Click(object sender, RoutedEventArgs e) {
            if (sender is Button) {
                Button btn = sender as Button;
                if (btn.Content.ToString() == "פעיל") {
                    UserService.cancleStatus(Convert.ToInt32(btn.Tag));
                } else {
                    UserService.restoreStatus(Convert.ToInt32(btn.Tag));
                }
                SharedDataIns.refreshData(TYPE.TEAM);
            }
        }

        private void buttonAddCrew_Click(object sender, RoutedEventArgs e) {
            if (GroupName != null && GroupName != "") {
                UserService.AddStatus(GroupName);
                this.SharedDataIns.refreshData(TYPE.TEAM);
                GroupName = "";
            }
        }
    }
}
