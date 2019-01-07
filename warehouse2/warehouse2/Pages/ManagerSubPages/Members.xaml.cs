﻿using System;
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
    /// Interaction logic for Members.xaml
    /// </summary>
    public partial class Members : Page, INotifyPropertyChanged {

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

        private GroupDets selectdFilter;
        private GroupDets selectedChange;
        private GroupDets selectedAdding;
        private DateTime selectedYear;
        private string memberName;
        private bool showMentor;
        public ObservableCollection<MemberDets> MembersList {
            get { return new ObservableCollection<MemberDets>(SharedData.GetInstans().MembersList.Where((e) => (SelectedFilter.GroupID != -1 ? e.GroupID == SelectedFilter.GroupID : (ShowMentor ? true : e.GroupID != 1)))); }
        }
        public ObservableCollection<GroupDets> GroupsDDL {
            get {
                ObservableCollection<GroupDets> list = new ObservableCollection<GroupDets>(SharedData.GetInstans().GroupsList);
                list.Insert(0, new GroupDets { GroupName = "בחר צוות", GroupID = -1, Enabled = true });
                return list;
            }
        }
        public GroupDets SelectedFilter {
            get { return (this.selectdFilter != null ? this.selectdFilter : new GroupDets { GroupName = "בחר צוות", GroupID = -1, Enabled = true }); }
            set {
                this.selectdFilter = value;
                OnPropertyChanged("SelectedFilter");
                OnPropertyChanged("MembersList");
                OnPropertyChanged("DispCheck");
            }
        }
        public GroupDets SelectedChange {
            get { return (this.selectedChange != null ? this.selectedChange : new GroupDets { GroupName = "בחר צוות", GroupID = -1, Enabled = true }); }
            set {
                this.selectedChange = value;
                OnPropertyChanged("SelectedChange");
            }
        }
        public GroupDets SelectedAdding {
            get { return (this.selectedAdding != null ? this.selectedAdding : new GroupDets { GroupName = "בחר צוות", GroupID = -1, Enabled = true }); }
            set {
                this.selectedAdding = value;
                OnPropertyChanged("SelectedAdding");
            }
        }
        public DateTime SelectedYear {
            get { return this.selectedYear; }
            set {
                this.selectedYear = value;
                OnPropertyChanged("SelectedYear");
            }
        }
        public string MemberName {
            get { return (this.memberName == null ? "" : this.memberName); }
            set {
                this.memberName = value;
                OnPropertyChanged("MemberName");
            }
        }
        public bool ShowMentor {
            get { return (DispCheck && this.showMentor); }
            set {
                this.showMentor = value;
                OnPropertyChanged("ShowMentor");
                OnPropertyChanged("MembersList");
            }
        }
        public bool DispCheck {
            get { return this.SelectedFilter.GroupID == -1; }
        }

        public Members() {
            InitializeComponent();
            this.SharedDataIns = SharedData.GetInstans();
        }

        private void buttonQuit_Click(object sender, RoutedEventArgs e) {
            
        }

        private void buttonReturn_Click(object sender, RoutedEventArgs e) {

        }

        private void buttonChangeCrew_Click(object sender, RoutedEventArgs e) {

        }

        private void buttonPrintMemberBarcode_Click(object sender, RoutedEventArgs e) {

        }

        private void buttonAddMember_Click(object sender, RoutedEventArgs e) {
            if (MemberName != "" && SelectedAdding.GroupID != -1 && SelectedYear.Year != 1) {
                UserService.AddUser(MemberName, SelectedAdding.GroupID, SelectedYear);
                this.SharedDataIns.refreshData(TYPE.MMBR);
                MemberName = null;
                SelectedAdding = null;
                this.comboBoxAddMemberCrew.SelectedIndex = 0;
                SelectedYear = new DateTime(1, 1, 1);
                this.comboBoxAddMemberYear.SelectedIndex = 0;
            }
        }
    }
}
