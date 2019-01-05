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
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class Tasks : Page, INotifyPropertyChanged {

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
        public Tasks() {
            InitializeComponent();
            this.SharedDataIns = SharedData.GetInstans();
        }

        private void buttonAddTask_Click(object sender, RoutedEventArgs e) {
            if (TaskService.addTask(this.textBoxTask.Text)) {
                this.textBoxTask.Text = "";
                SharedDataIns.refreshData(TYPE.TASK);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (sender is Button) {
                Button btn = sender as Button;
                TaskService.doneTask(Convert.ToInt32(btn.Tag));
                SharedDataIns.refreshData(TYPE.TASK);
            }
        }
    }
}
