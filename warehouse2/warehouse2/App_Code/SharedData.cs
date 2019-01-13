using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehouse2 {
    public enum TYPE {
        ALL,
        LOAN,
        TASK,
        COMP,
        TEAM,
        MMBR,
        KIND,
        TOOL,
        MNGR,
        FIRST
    }


    public class SharedData : INotifyPropertyChanged {
        const int TeamStart = 2005;
        const int buildrsID = 2;

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        // Data
        private static SharedData _this;
        ObservableCollection<LoanedTool> outToolList;
        ObservableCollection<TaskDets> tasksList;
        ObservableCollection<LoanedTool> outCompToolList;
        ObservableCollection<ToolDets> compToolsList;
        ObservableCollection<GroupDets> groupsList;
        ObservableCollection<MemberDets> membersList;
        ObservableCollection<KindDets> kindsList;
        ObservableCollection<ToolDets> toolsList;
        ObservableCollection<ManagerDets> managersList;
        ObservableCollection<TeamDets> teamList;
        ManagerDets currentManager;
        MemberDets currentStorekeeper;
        ObservableCollection<DateTime> yDates;

        // Properties
        public ObservableCollection<LoanedTool> OutToolList {
            get { return outToolList; }
            set {
                this.outToolList = value;
                OnPropertyChanged("OutToolList");
            }
        }
        public ObservableCollection<TaskDets> TasksList {
            get { return tasksList; }
            set {
                this.tasksList = value;
                OnPropertyChanged("TasksList");
            }
        }
        public ObservableCollection<LoanedTool> OutCompToolList {
            get { return outCompToolList; }
            set {
                this.outCompToolList = value;
                OnPropertyChanged("OutCompToolList");
            }
        }
        public ObservableCollection<ToolDets> CompToolsList {
            get { return compToolsList; }
            set {
                this.compToolsList = value;
                OnPropertyChanged("CompToolsList");
            }
        }
        public ObservableCollection<GroupDets> GroupsList {
            get { return groupsList; }
            set {
                this.groupsList = value;
                OnPropertyChanged("GroupsList");
                OnPropertyChanged("GroupsDDL");
            }
        }
        public ObservableCollection<MemberDets> MembersList {
            get { return membersList; }
            set {
                this.membersList = value;
                OnPropertyChanged("MembersList");
                OnPropertyChanged("StorekeepersList");
            }
        }
        public ObservableCollection<KindDets> KindsList {
            get { return kindsList; }
            set {
                this.kindsList = value;
                OnPropertyChanged("KindsList");
                OnPropertyChanged("KindsDDL");
            }
        }
        public ObservableCollection<ToolDets> ToolsList {
            get { return toolsList; }
            set {
                this.toolsList = value;
                OnPropertyChanged("ToolsList");
            }
        }
        public ObservableCollection<ManagerDets> ManagersList {
            get {
                return new ObservableCollection<ManagerDets>(managersList.Where((e) => e.UserName != "admin" &&
                                                                                       e.Password != "nimda" &&
                                                                                       e.UserName != CurrentManager.UserName &&
                                                                                       e.Password != CurrentManager.Password));
            }
            set {
                this.managersList = value;
                OnPropertyChanged("ManagersList");
            }
        }
        public ObservableCollection<MemberDets> StorekeepersList {
            get { return new ObservableCollection<MemberDets>(membersList.Where((e) => e.GroupID == buildrsID)); }
        }
        public ObservableCollection<TeamDets> TeamsList {
            get { return this.teamList; }
            set { this.teamList = value; }
        }
        public ManagerDets CurrentManager {
            get {
                if (this.currentManager == null) {
                    return new ManagerDets();
                } else {
                    return this.currentManager;
                }
            }
            set {
                this.currentManager = value;
                OnPropertyChanged("CurrentManager");
            }
        }
        public MemberDets CurrentStorekeeper {
            get { return (this.currentStorekeeper == null ? new MemberDets { MemberName = "בחר מחסנאי" } : this.currentStorekeeper); }
            set {
                this.currentStorekeeper = value;
                OnPropertyChanged("CurrentStorekeeper");
            }
        }
        public ObservableCollection<DateTime> YDates {
            get { return this.yDates; }
            set {
                this.yDates = value;
                OnPropertyChanged("YDates");
            }
        }

        // Ctor
        private SharedData() {
            refreshData(TYPE.ALL);
            CurrentManager = null;
            CurrentStorekeeper = null;
        }
        public static SharedData GetInstans() {
            if (_this == null) {
                _this = new SharedData();
            }
            return _this;
        }

        // Methods
        public void refreshData(TYPE type) {
            if (type == TYPE.ALL || type == TYPE.LOAN) {
                OutToolList = TakeOut.GetOutTools();
            }
            if (type == TYPE.ALL || type == TYPE.TASK) {
                TasksList = TaskService.getTasks();
            }
            if (type == TYPE.ALL || type == TYPE.COMP) {
                //OutCompToolList
                //CompToolsList
            }
            if (type == TYPE.ALL || type == TYPE.TEAM) {
                GroupsList = UserService.GetAllStatus(0);
            }
            if (type == TYPE.ALL || type == TYPE.MMBR) {
                MembersList = UserService.GetAllUsers(false);
            }
            if (type == TYPE.ALL || type == TYPE.KIND) {
                KindsList = ToolService.GetAllKinds(false);
            }
            if (type == TYPE.ALL || type == TYPE.TOOL) {
                ToolsList = ToolService.GetAllTools();
            }
            if (type == TYPE.ALL || type == TYPE.MNGR) {
                ManagersList = UserService.GetAllMenegers();
            }
            if (type == TYPE.ALL || type == TYPE.FIRST) {
                //TeamsList = TeamService.GetAllTeams();
            }
            if (type == TYPE.ALL) {
                setDates();
            }
        }

        private void setDates() {
            this.YDates = new ObservableCollection<DateTime>();
            int currDate = DateTime.Now.Year;
            while (currDate >= TeamStart) {
                this.YDates.Add(new DateTime(currDate, 9, 1));
                currDate--;
            }
        }
    }
}
