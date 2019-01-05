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
    /// Interaction logic for Tools.xaml
    /// </summary>
    public partial class Tools : Page, INotifyPropertyChanged {

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

        public bool NeedLost {
            get { return ((this.dataGridTools.SelectedItems.Cast<ToolDets>().Where((e) => e.Enabled == true)).ToList().Count > 0); }
        }
        public bool NeedFound {
            get { return ((this.dataGridTools.SelectedItems.Cast<ToolDets>().Where((e) => e.Enabled == false)).ToList().Count > 0); }
        }
        public bool NeedComp {
            get { return ((this.dataGridTools.SelectedItems.Cast<ToolDets>().Where((e) => e.Enabled == true && e.IsComp == false)).ToList().Count > 0); }
        }
        public Tools() {
            InitializeComponent();
            this.SharedDataIns = SharedData.GetInstans();
        }

        private void buttonLost_Click(object sender, RoutedEventArgs e) {
            foreach (ToolDets tool in this.dataGridTools.SelectedItems.Cast<ToolDets>().Where((t) => t.Enabled == true)) {
                ToolService.CancleTool(tool.ToolID);
            }
            SharedDataIns.refreshData(TYPE.TOOL);
        }

        private void buttonfound_Click(object sender, RoutedEventArgs e) {
            foreach (ToolDets tool in this.dataGridTools.SelectedItems.Cast<ToolDets>().Where((t) => t.Enabled == false)) {
                ToolService.RestoreTool(tool.ToolID);
            }
            SharedDataIns.refreshData(TYPE.TOOL);
        }

        private void dataGridTools_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            OnPropertyChanged("NeedLost");
            OnPropertyChanged("NeedFound");
            OnPropertyChanged("NeedComp");
        }

        private void buttonAddComp_Click(object sender, RoutedEventArgs e) {

        }

        private void buttonChangeKind_Click(object sender, RoutedEventArgs e) {

        }

        private void buttonAddTool_Click(object sender, RoutedEventArgs e) {

        }

        private void comboBoxToolsFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }
    }
}
