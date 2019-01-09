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
using System.Windows.Threading;
using System.Collections;
using System.Data;
using System.Collections.ObjectModel;

namespace warehouse2 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public static MainWindow mainWin;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private DispatcherTimer checkReturnTimer;
        private DispatcherTimer checkReturnTimerComp;
        private bool managerIn;
        SharedData sharedDataIns;
        MemberDets CurrentStorekeeper {
            get { return SharedData.GetInstans().CurrentStorekeeper; }
        }

        public bool ManagerIn {
            get { return this.managerIn; }
            set {
                this.managerIn = value;
                this.OnPropertyChanged("ManagerIn");
            }
        }
        public SharedData SharedDataIns {
            get { return sharedDataIns; }
            set {
                sharedDataIns = value;
                OnPropertyChanged("SharedDataIns");
            }
        }

        public MainWindow() {
            if (mainWin == null) {
                mainWin = this;
            }
            try {
                InitializeComponent();
            } catch (Exception ex) {
                MessageBox.Show("1. " + ex.Message);
            }
            try {
                this.SharedDataIns = SharedData.GetInstans();
                checkReturnTimer = new DispatcherTimer();
                checkReturnTimer.Tick += CheckReturnTimer_Tick;
                checkReturnTimer.Interval = new TimeSpan(0, 5, 0);
                checkReturnTimer.Start();
                checkReturnTimerComp = new DispatcherTimer();
                checkReturnTimerComp.Tick += CheckReturnTimerComp_Tick;
                checkReturnTimerComp.Interval = new TimeSpan(0, 5, 0);
                checkReturnTimerComp.Start();
                //BarcodeService BS = new BarcodeService();
                //BS.SaveBarcodesInFile(new string[] { "U001", "U002", "U003", "T001", "T002", "T003", "U004", "T004", "U005", "T005", "U006", "T006" });
#if COMP
            this.CompFrame.Content = new SoonPage();
#endif

            } catch (Exception ex) {
                MessageBox.Show("2. " + ex.Message);
            }
        }

        private void CheckReturnTimerComp_Tick(object sender, EventArgs e) {
            string st = "הכלים הללו מעל שעה בחוץ:\n";
            ObservableCollection<LoanedTool> list = SharedDataIns.OutToolList;
            if (list.Count > 0) {
                bool ok = false;
                foreach (LoanedTool tool in list) {
                    if (tool.TakeTime.AddHours(1) < DateTime.Now) {
                        st += tool.ToolName + " (" + tool.UserName + ")\n"; // dr["KindName"] + " " + dr["ToolName"] + " " + dr["Numberring"] + " (" + dr["UserName"] + ")\n";
                        ok = true;
                    }
                }
                if (ok)
                    MessageBox.Show(st);
            }
        }

        private void CheckReturnTimer_Tick(object sender, EventArgs e) {

        }

        private void MenuItem_LogIn_Click(object sender, RoutedEventArgs e) {
            ManagerWindow win = new ManagerWindow();
            win.ShowDialog();
        }

        private void MenuItem_LogOut_Click(object sender, RoutedEventArgs e) {
            ManagerIn = false;
        }

        private void MenuItem_Refresh_Click(object sender, RoutedEventArgs e) {
            SharedDataIns.refreshData(TYPE.ALL);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            OnPropertyChanged("CurrentStorekeeper");
        }
    }
}
