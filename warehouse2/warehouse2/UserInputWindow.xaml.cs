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
using System.Windows.Shapes;

namespace warehouse2 {
    /// <summary>
    /// Interaction logic for UserInputWindow.xaml
    /// </summary>
    public partial class UserInputWindow : Window, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        string displayText;
        string userChoise;
        public string DisplayText {
            get { return this.displayText; }
            set {
                this.displayText = value;
                OnPropertyChanged("DisplayText");
            }
        }
        public string UserChoise {
            get { return this.userChoise; }
            set {
                this.userChoise = value;
                OnPropertyChanged("UserChoise");
            }
        }
        public int Input {
            get;
            set;
        }
        public UserInputWindow() {
            InitializeComponent();
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                if (UserChoise[0] == 'T' || UserChoise[0] == 'U') {
                    Input = Convert.ToInt32(UserChoise.Remove(0, 1));
                } else 
                    Input = Convert.ToInt32(UserChoise);
                this.Close();
            }
        }
    }
}
