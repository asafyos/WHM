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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace warehouse2 {
    /// <summary>
    /// Interaction logic for Loans.xaml
    /// </summary>
    public partial class Loans : Page, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        string loanBarcode;
        string returnBarcode;
        string searchBarcode;
        SharedData sharedDataIns;

        public string LoanBarcode {
            get { return this.loanBarcode; }
            set {
                this.loanBarcode = value;
                OnPropertyChanged("LoanBarcode");
            }
        }
        public string ReturnBarcode {
            get { return this.returnBarcode; }
            set {
                this.returnBarcode = value;
                OnPropertyChanged("ReturnBarcode");
            }
        }
        public string SearchBarcode {
            get { return this.searchBarcode; }
            set {
                this.searchBarcode = value;
                OnPropertyChanged("SearchBarcode");
            }
        }
        public SharedData SharedDataIns { get { return sharedDataIns; } set {
                sharedDataIns = value;
                OnPropertyChanged("SharedDataIns");
            }
        }

        public Loans() {
            InitializeComponent();
            this.SharedDataIns = SharedData.GetInstans();
        }


        private void textBoxBarcode_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                if (LoanBarcode != null && LoanBarcode != "") {
                    if (LoanBarcode[0] == 'T') {

                    } else if (LoanBarcode[0] == 'U') {

                    } else {

                    }
                }
            }
        }

        private void textBoxReturn_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {

            }
        }

        private void textBoxSearch_KeyUp(object sender, KeyEventArgs e) {
            MainWindow.mainWin.ManagerIn = !MainWindow.mainWin.ManagerIn;
        }
    }
}
