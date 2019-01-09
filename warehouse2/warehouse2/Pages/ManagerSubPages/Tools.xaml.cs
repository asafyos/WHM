using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        KindDets selectedFilter;
        KindDets selectedAdding;
        KindDets selectedChanging;
        string toolName;
        string numberring;
        bool showBroke;
        public SharedData SharedDataIns {
            get { return sharedDataIns; }
            set {
                sharedDataIns = value;
                OnPropertyChanged("SharedDataIns");
            }
        }
        public ObservableCollection<ToolDets> ToolsList {
            get {
                ObservableCollection<ToolDets> list;
                list = new ObservableCollection<ToolDets>(SharedData.GetInstans().ToolsList.Where((e) => (SelectedFilter.KindID == -1 ? true : e.KindID == SelectedFilter.KindID) && (ShowBroke ? true : e.Enabled)));
                return list;
            }
        }
        public ObservableCollection<KindDets> KindsDDL {
            get {
                ObservableCollection<KindDets> list = new ObservableCollection<KindDets>(SharedData.GetInstans().KindsList);
                list.Insert(0, new KindDets { KindName = "בחר סוג", KindID = -1, Enabled = true });
                return list;
            }
        }
        public KindDets SelectedFilter {
            get { return (this.selectedFilter != null ? this.selectedFilter : new KindDets { KindName = "בחר סוג", KindID = -1, Enabled = true }); }
            set {
                this.selectedFilter = value;
                OnPropertyChanged("SelectedFilter");
                OnPropertyChanged("ToolsList");
            }
        }
        public KindDets SelectedAdding {
            get { return (this.selectedAdding != null ? this.selectedAdding : new KindDets { KindName = "בחר סוג", KindID = -1, Enabled = true }); }
            set {
                this.selectedAdding = value;
                OnPropertyChanged("SelectedAdding");
            }
        }
        public KindDets SelectedChanging {
            get { return (this.selectedChanging != null ? this.selectedChanging : new KindDets { KindName = "בחר סוג", KindID = -1, Enabled = true }); }
            set {
                this.selectedChanging = value;
                OnPropertyChanged("SelectedChanging");
            }
        }
        public string ToolName {
            get { return (this.toolName != null ? this.toolName : ""); }
            set {
                this.toolName = value;
                OnPropertyChanged("ToolName");
            }
        }
        public string Numberring {
            get { return (this.numberring != null ? this.numberring : ""); }
            set {
                this.numberring = value;
                OnPropertyChanged("Numberring");
            }
        }
        public bool ShowBroke {
            get { return this.showBroke; }
            set {
                this.showBroke = value;
                OnPropertyChanged("ShowBroke");
                OnPropertyChanged("ToolsList");
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
            OnPropertyChanged("ToolsList");
        }

        private void buttonfound_Click(object sender, RoutedEventArgs e) {
            foreach (ToolDets tool in this.dataGridTools.SelectedItems.Cast<ToolDets>().Where((t) => t.Enabled == false)) {
                ToolService.RestoreTool(tool.ToolID);
            }
            SharedDataIns.refreshData(TYPE.TOOL);
            OnPropertyChanged("ToolsList");
        }

        private void dataGridTools_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            OnPropertyChanged("NeedLost");
            OnPropertyChanged("NeedFound");
            OnPropertyChanged("NeedComp");
        }

        private void buttonAddComp_Click(object sender, RoutedEventArgs e) {

        }

        private void buttonChangeKind_Click(object sender, RoutedEventArgs e) {
            if (SelectedChanging.KindID != -1 && this.dataGridTools.SelectedItems.Count > 0) {
                int[] ids = new int[this.dataGridTools.SelectedItems.Count];
                int i = 0;
                foreach (ToolDets tool in this.dataGridTools.SelectedItems) {
                    ids[i] = tool.ToolID;
                    i++;
                }
                ToolService.changeKind(ids, SelectedChanging.KindID);
                SharedDataIns.refreshData(TYPE.TOOL);
                OnPropertyChanged("ToolsList");
                this.comboBoxChangeKind.SelectedIndex = 0;
            }
        }

        private void buttonAddTool_Click(object sender, RoutedEventArgs e) {
            if (ToolName != "" && SelectedAdding.KindID != -1) {
                ToolService.AddTool(ToolName, Numberring, SelectedAdding.KindID);
                ToolName = null;
                Numberring = null;
                this.comboBoxAddToolKind.SelectedIndex = 0;
                sharedDataIns.refreshData(TYPE.TOOL);
                OnPropertyChanged("ToolsList");
            }
        }

        private void buttonPrintToolBarcode_Click(object sender, RoutedEventArgs e) {
            openBarcode();
        }

        private void openBarcode() {
            List<string> list = new List<string>();
            foreach (ToolDets tool in this.dataGridTools.SelectedItems) {
                list.Add("T" + tool.ToolID.ToString("D10"));
            };
            BarcodeService.SaveBarcodesInFile(list.ToArray());
        }
    }
}
