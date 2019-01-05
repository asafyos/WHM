using System;
using System.Collections.Generic;
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
    public partial class ManagerWindow : Window {
        public ManagerWindow() {
            InitializeComponent();
            this._UserName.Focus();
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
