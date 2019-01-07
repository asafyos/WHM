using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.ComponentModel;

namespace warehouse2 {
    public class NotifObject : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    public class LoanedTool : NotifObject {
        private string userName;
        private int userID;
        private string toolName;
        private int toolID;
        private DateTime takeTime;
        private string groupName;
        private string storekeeper;
        private string teamName;
        private int teamNum;

        public string UserName {
            get { return userName; }
            set {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
        public int UserID {
            get { return userID; }
            set {
                userID = value;
                OnPropertyChanged("UserID");
            }
        }
        public string ToolName {
            get { return toolName; }
            set {
                toolName = value;
                OnPropertyChanged("ToolName");
            }
        }
        public int ToolID {
            get { return toolID; }
            set {
                toolID = value;
                OnPropertyChanged("ToolID");
            }
        }
        public DateTime TakeTime {
            get { return takeTime; }
            set {
                takeTime = value;
                OnPropertyChanged("TakeTime");
            }
        }
        public string GroupName {
            get { return groupName; }
            set {
                groupName = value;
                OnPropertyChanged("StatusName");
            }
        }
        public string Storekeeper {
            get { return storekeeper; }
            set {
                storekeeper = value;
                OnPropertyChanged("Storekeeper");
            }
        }
        public string TeamName {
            get { return teamName; }
            set {
                teamName = value;
                OnPropertyChanged("TeamName");
            }
        }
        public int TeamNum {
            get { return teamNum; }
            set {
                teamNum = value;
                OnPropertyChanged("TeamNum");
            }
        }
    }
    public class TaskDets : NotifObject {
        private int taskID;
        private string taskText;
        public int TaskID {
            get { return taskID; }
            set {
                taskID = value;
                OnPropertyChanged("TaskID");
            }
        }
        public string TaskText {
            get { return taskText; }
            set {
                taskText = value;
                OnPropertyChanged("TaskText");
            }
        }

    }
    public class GroupDets : NotifObject {
        private int groupID;
        private string groupName;
        private bool enabled;
        public int GroupID {
            get { return groupID; }
            set {
                groupID = value;
                OnPropertyChanged("GroupID");
            }
        }
        public string GroupName {
            get { return groupName; }
            set {
                groupName = value;
                OnPropertyChanged("GroupName");
            }
        }
        public bool Enabled {
            get { return enabled; }
            set {
                enabled = value;
                OnPropertyChanged("Enabled");
            }
        }
    }
    public class MemberDets : NotifObject {
        private int memberID;
        private string memberName;
        private int groupID;
        private string groupName;
        private DateTime joinDate;
        private bool enabled;
        public int MemberID {
            get { return memberID; }
            set {
                memberID = Convert.ToInt32(value);
                OnPropertyChanged("MemberID");
            }
        }
        public string MemberName {
            get { return memberName; }
            set {
                memberName = value;
                OnPropertyChanged("MemberName");
            }
        }
        public int GroupID {
            get { return groupID; }
            set {
                groupID = value;
                OnPropertyChanged("GroupID");
            }
        }
        public string GroupName {
            get { return groupName; }
            set {
                groupName = value;
                OnPropertyChanged("GroupName");
            }
        }
        public DateTime JoinDate {
            get { return joinDate; }
            set {
                joinDate = value;
                OnPropertyChanged("JoinDate");
            }
        }
        public bool Enabled {
            get { return enabled; }
            set {
                enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

    }
    public class KindDets : NotifObject {
        private int kindID;
        private string kindName;
        private bool enabled;
        public int KindID {
            get { return kindID; }
            set {
                kindID = value;
                OnPropertyChanged("KindID");
            }
        }
        public string KindName {
            get { return kindName; }
            set {
                kindName = value;
                OnPropertyChanged("KindName");
            }
        }
        public bool Enabled {
            get { return enabled; }
            set {
                enabled = value;
                OnPropertyChanged("Enabled");
            }
        }
    }
    public class ToolDets : NotifObject {
        private int toolID;
        private string toolName;
        private int kindID;
        private string kindName;
        private string number;
        private bool enabled;
        private bool isComp;
        private string place;

        public int ToolID {
            get { return toolID; }
            set {
                toolID = value;
                OnPropertyChanged("ToolID");
            }
        }
        public string ToolName {
            get { return toolName; }
            set {
                toolName = value;
                OnPropertyChanged("ToolName");
            }
        }
        public int KindID {
            get { return kindID; }
            set {
                kindID = value;
                OnPropertyChanged("KindID");
            }
        }
        public string KindName {
            get { return kindName; }
            set {
                kindName = value;
                OnPropertyChanged("KindName");
            }
        }
        public string Number {
            get { return number; }
            set {
                number = value;
                OnPropertyChanged("Number");
            }
        }
        public bool Enabled {
            get { return enabled; }
            set {
                enabled = value;
                OnPropertyChanged("Enabled");
            }
        }
        public bool IsComp {
            get { return isComp; }
            set {
                isComp = value;
                OnPropertyChanged("IsComp");
            }
        }
        public string Place {
            get { return place; }
            set {
                place = value;
                OnPropertyChanged("Place");
            }
        }
    }
    public class ManagerDets : NotifObject {
        private string userName;
        private string password;
        public string UserName {
            get { return userName; }
            set {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
        public string Password {
            get { return password; }
            set {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        
        public override bool Equals(object obj) {

            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            return (userName == ((ManagerDets)obj).userName && password == ((ManagerDets)obj).password);
        }
        
        public override int GetHashCode() {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
        }
    }
}