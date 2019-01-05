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
    /// Interaction logic for Manager.xaml
    /// </summary>
    public partial class Manager : Page, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        SharedData sharedDataIns;
        public SharedData SharedDataIns {
            get { return sharedDataIns; }
            set {
                sharedDataIns = value;
                OnPropertyChanged("SharedDataIns");
            }
        }

        private ManagerDets current;
        private string userName;
        private string password;

        public ManagerDets Current {
            get { return this.current; }
            set {
                this.current = value;
                OnPropertyChanged("Current");
            }
        }
        public string UserName {
            get { return userName; }
            set {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
        public string Password {
            get { return password; }
            set {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public Manager() {
            InitializeComponent();
            this.SharedDataIns = SharedData.GetInstans();
        }

        private void buttonAddManager_Click(object sender, RoutedEventArgs e) {
            if (UserName != null && Password != null &&
                UserName != "בחר משתמש" && Password != "0") {
                if (!UserService.AddMeneger(UserName, Password)) {
                    Password = "";
                    MessageBox.Show("שם משתמש או סיסמא קיימים");
                } else {
                    UserName = "";
                    Password = "";
                    this.passwordBoxPassword.Password = Password;
                    this.SharedDataIns.refreshData(TYPE.MNGR);
                }
            } else {
                MessageBox.Show("מלא את כל הפרטים");
            }
        }

        private void buttonRemoveManager_Click(object sender, RoutedEventArgs e) {
            if (Current.Password != "0") {
                UserService.DeleteMeneger(Current.UserName, Current.Password);
                this.SharedDataIns.refreshData(TYPE.MNGR);
            }
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e) {
            if (sender is PasswordBox) {
                PasswordBox pb = sender as PasswordBox;
                pb.Tag = (!string.IsNullOrEmpty(pb.Password)).ToString();
                Password = pb.Password;
            }
        }
    }
}
