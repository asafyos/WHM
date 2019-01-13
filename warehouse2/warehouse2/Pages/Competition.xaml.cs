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
    /// Interaction logic for Competition.xaml
    /// </summary>
    public partial class Competition : Page, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        SharedData sharedDataIns;
        string toolInput;
        string loanerName;
        string teamNum;
        string searchTerm;
        public SharedData SharedDataIns {
            get { return sharedDataIns; }
            set {
                sharedDataIns = value;
                OnPropertyChanged("SharedDataIns");
            }
        }
        public ObservableCollection<LoanedTool> OutCompToolList 
            {
            get {
                int num;
                ObservableCollection<LoanedTool> list = new ObservableCollection<LoanedTool>(SharedData.GetInstans().OutCompToolList.Where((e) =>
                    (SearchTerm[0] == 'T' ? e.ToolID == Convert.ToInt32(SearchTerm.Substring(1)) : 
                    (SearchTerm[0] == 'U' ? e.UserID == Convert.ToInt32(SearchTerm.Substring(1)) : 
                    (int.TryParse(SearchTerm, out num) ? e.TeamNum == num :
                    (e.UserName.Contains(SearchTerm) || e.ToolName.Contains(SearchTerm) || e.TeamName.Contains(SearchTerm)))))).ToList());
                return list;
            }
        }
        public string ToolInput {
            get { return this.toolInput; }
            set {
                this.toolInput = value;
                OnPropertyChanged("ToolInput");
            }
        }
        public string LoanerName {
            get { return this.loanerName; }
            set {
                this.loanerName = value;
                OnPropertyChanged("LoanerName");
            }
        }
        public string TeamNum {
            get { return (this.teamNum == null ? "" : this.teamNum); }
            set {
                this.teamNum = value;
                OnPropertyChanged("TeamNum");
            }
        }
        public string SearchTerm {
            get { return this.searchTerm; }
            set {
                this.searchTerm = value;
                OnPropertyChanged("SearchTerm");
                OnPropertyChanged("OutCompToolList");
            }
        }
        public string TeamName {
            get {
                int num;
                if (TeamNum == "" || !int.TryParse(TeamNum, out num)) {
                    return "";
                }
                List<TeamDets> list = SharedData.GetInstans().TeamsList.Where((e) => e.TeamNum == num).ToList();
                if (list.Count == 0) {
                    return TeamService.getTeamName(num);
                }
                return (list[0].TeamName);
            }
        }
        public Competition() {
            InitializeComponent();
            this.SharedDataIns = SharedData.GetInstans();
        }

        private void textBoxToolComp_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                this.textBoxTakerComp.Focus();
            }
        }

        private void textBoxTakerComp_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                this.textBoxTeam.Focus();
            }
        }

        private void textBoxTeam_LostFocus(object sender, RoutedEventArgs e) {
            OnPropertyChanged("TeamName");
        }
    }
}
