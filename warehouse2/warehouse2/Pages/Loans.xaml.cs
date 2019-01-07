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
            get { return (this.loanBarcode == null ? "" : this.loanBarcode); }
            set {
                this.loanBarcode = value;
                OnPropertyChanged("LoanBarcode");
            }
        }
        public string ReturnBarcode {
            get { return (this.returnBarcode == null ? "" : this.returnBarcode); }
            set {
                this.returnBarcode = value;
                OnPropertyChanged("ReturnBarcode");
            }
        }
        public string SearchBarcode {
            get { return (this.searchBarcode == null ? "" : this.searchBarcode); }
            set {
                this.searchBarcode = value;
                OnPropertyChanged("SearchBarcode");
            }
        }
        public SharedData SharedDataIns {
            get { return sharedDataIns; }
            set {
                sharedDataIns = value;
                OnPropertyChanged("SharedDataIns");
            }
        }
        public bool NeedReturn {
            get { return this.dataGridLoaned.SelectedItems.Count > 0; }
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
            if (e.Key == Key.Enter && ReturnBarcode != "") {
                if (ReturnBarcode[0] == 'T') {
                    ReturnBarcode = ReturnBarcode.Remove(0, 1);
                    int toolID = Convert.ToInt32(ReturnBarcode);
                    List<LoanedTool> list = this.SharedDataIns.OutToolList.Where((t) => t.ToolID == toolID).ToList();
                    if (list.Count == 1) {
                        LoanedTool tool = list[0];
                        TakeOut.ReturnTool(tool.UserID, tool.ToolID, tool.TakeTime);
                        this.SharedDataIns.refreshData(TYPE.LOAN);
                        OnPropertyChanged("OutToolList");
                    } else {
                        UserInputWindow input = new UserInputWindow();
                        input.DisplayText = "סרוק משאיל";
                        input.ShowDialog();
                        list = list.Where((t) => t.UserID == input.Input).ToList();
                        if (list.Count == 1) {
                            LoanedTool tool = list[0];
                            TakeOut.ReturnTool(tool.UserID, tool.ToolID, tool.TakeTime);
                            this.SharedDataIns.refreshData(TYPE.LOAN);
                            OnPropertyChanged("OutToolList");
                        } else {
                            // TODO: MultiItem
                        }
                    }
                } else if (ReturnBarcode[0] == 'U') {
                    ReturnBarcode = ReturnBarcode.Remove(0, 1);
                    int userID = Convert.ToInt32(ReturnBarcode);
                    List<LoanedTool> list = this.SharedDataIns.OutToolList.Where((t) => t.ToolID == userID).ToList();
                    if (list.Count == 1) {
                        LoanedTool tool = list[0];
                        TakeOut.ReturnTool(tool.UserID, tool.ToolID, tool.TakeTime);
                        this.SharedDataIns.refreshData(TYPE.LOAN);
                        OnPropertyChanged("OutToolList");
                    } else {
                        UserInputWindow input = new UserInputWindow();
                        input.DisplayText = "סרוק כלי";
                        input.ShowDialog();
                        list = list.Where((t) => t.ToolID == input.Input).ToList();
                        if (list.Count == 1) {
                            LoanedTool tool = list[0];
                            TakeOut.ReturnTool(tool.UserID, tool.ToolID, tool.TakeTime);
                            this.SharedDataIns.refreshData(TYPE.LOAN);
                            OnPropertyChanged("OutToolList");
                        } else {
                            // TODO: MultiItem
                        }
                    }
                } else {
                    // TODO: FreeText
                }
                ReturnBarcode = null;
                this.SharedDataIns.refreshData(TYPE.LOAN);
                OnPropertyChanged("OutToolList");
            }
        }

        private void textBoxSearch_KeyUp(object sender, KeyEventArgs e) {
            MainWindow.mainWin.ManagerIn = !MainWindow.mainWin.ManagerIn;
        }

        private void GridButton_Click(object sender, RoutedEventArgs e) {
            if (sender is Button) {
                Button btn = sender as Button;
                if (btn.Tag is LoanedTool) {
                    LoanedTool tool = btn.Tag as LoanedTool;
                    TakeOut.ReturnTool(tool.UserID, tool.ToolID, tool.TakeTime);
                    this.SharedDataIns.refreshData(TYPE.LOAN);
                    OnPropertyChanged("OutToolList");
                }
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e) {
            foreach (LoanedTool tool in this.dataGridLoaned.SelectedItems) {
                TakeOut.ReturnTool(tool.UserID, tool.ToolID, tool.TakeTime);
            }
            this.SharedDataIns.refreshData(TYPE.LOAN);
            OnPropertyChanged("OutToolList");
        }

        private void dataGridLoaned_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            OnPropertyChanged("NeedReturn");
        }
    }
}
