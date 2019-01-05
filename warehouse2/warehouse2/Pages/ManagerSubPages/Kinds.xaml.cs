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
    /// Interaction logic for Kinds.xaml
    /// </summary>
    public partial class Kinds : Page, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        SharedData sharedDataIns;
        string kindName;
        public SharedData SharedDataIns {
            get { return sharedDataIns; }
            set {
                sharedDataIns = value;
                OnPropertyChanged("SharedDataIns");
            }
        }
        public string KindName {
            get { return this.kindName; }
            set {
                this.kindName = value;
                OnPropertyChanged("KindName");
            }
        }

        public Kinds() {
            InitializeComponent();
            this.SharedDataIns = SharedData.GetInstans();
        }

        private void Enabled_Button_Click(object sender, RoutedEventArgs e) {
            if (sender is Button) {
                Button btn = sender as Button;
                if (btn.Content.ToString() == "קיים") {
                    ToolService.CancleKind(Convert.ToInt32(btn.Tag));
                } else {
                    ToolService.RestoreKind(Convert.ToInt32(btn.Tag));
                }
                SharedDataIns.refreshData(TYPE.KIND);
            }
        }

        private void buttonAddKind_Click(object sender, RoutedEventArgs e) {
            if (KindName != null && KindName != "") {
                ToolService.AddKind(KindName);
                SharedDataIns.refreshData(TYPE.KIND);
                KindName = "";
            }
        }
    }
}
