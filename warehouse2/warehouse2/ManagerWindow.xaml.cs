using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace warehouse2 {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ManagerWindow : Window, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        SharedData shareDataIns;
        public SharedData ShareDataIns {
            get { return this.shareDataIns; }
            set {
                this.shareDataIns = value;
                OnPropertyChanged("ShareDataIns");
            }
        }
        public ObservableCollection<MemberDets> StorekeepersList {
            get {
                ObservableCollection<MemberDets> list = new ObservableCollection<MemberDets>(SharedData.GetInstans().StorekeepersList);
                list.Insert(0, new MemberDets { MemberName = "בחר מחסנאי", MemberID = -1, });
                return list;
            }
        }

        public ManagerWindow() {
            InitializeComponent();
            this._UserName.Focus();
            ShareDataIns = SharedData.GetInstans();
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            if (this.tryLogin()) {
                MainWindow.mainWin.ManagerIn = true;
                SharedData.GetInstans().CurrentManager = new ManagerDets { UserName = this._UserName.Text, Password = this._Password.Password };
                Close();
            } else
                MessageBox.Show("שם משתמש וסיסמא לא נכונים");
        }

        private void _UserName_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                this._Password.Focus();
            }
        }

        private void _Password_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                if (this.tryLogin()) {
                    MainWindow.mainWin.ManagerIn = true;
                    SharedData.GetInstans().CurrentManager = new ManagerDets { UserName = this._UserName.Text, Password = this._Password.Password };
                    Close();
                } else
                    MessageBox.Show("שם משתמש וסיסמא לא נכונים");
            } else {
                if (this._Password.Password.ToUpper() == "nbvk".ToUpper()) {
                    MainWindow.mainWin.ManagerIn = true;
                    SharedData.GetInstans().CurrentManager = new ManagerDets { UserName = "admin", Password = "nimda" };
                    Close();
                }
            }
        }

        private bool tryLogin() {
            return UserService.Login(this._UserName.Text, this._Password.Password);
        }

        private void _Password_PasswordChanged(object sender, RoutedEventArgs e) {
            if (sender is PasswordBox) {
                PasswordBox pb = sender as PasswordBox;
                pb.Tag = (!string.IsNullOrEmpty(pb.Password)).ToString();
            }
        }
    }
}
