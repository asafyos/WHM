using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Collections.ObjectModel;

namespace warehouse2 {
    class UserService {
        static OleDbConnection myConn;
        static UserService() {
            myConn = new OleDbConnection(Connect.GetConnectionString());
        }

        /// <summary>
        /// return all the members in the database
        /// </summary>
        /// <returns>DataSet</returns>
        public static ObservableCollection<MemberDets> GetAllUsers(bool addFirstRow) {
            OleDbCommand cmd = new OleDbCommand("GetAllUsers", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "UsersTbl");
                ds.Tables["UsersTbl"].PrimaryKey = new DataColumn[] { ds.Tables["UsersTbl"].Columns["UserID"] };
            } catch (Exception ex) { throw ex; }
            if (addFirstRow) {
                DataRow row = ds.Tables[0].NewRow();
                row["UserID"] = 0;
                row["StatusID"] = 0;
                row["UserName"] = "בחר חבר קבוצה";
                ds.Tables[0].Rows.Add(row);
            }
            DataColumn dc = new DataColumn("Cancle", typeof(System.String));
            dc.Unique = false;
            ds.Tables[0].Columns.Add(dc);
            dc = new DataColumn("Update", typeof(System.String));
            dc.Unique = false;
            ds.Tables[0].Columns.Add(dc);
            foreach (DataRow dr in ds.Tables[0].Rows) {
                dr["Cancle"] = "ביטול";
                dr["Update"] = "שנה";
            }
            return ConvertToList(ds);
        }

        public static ObservableCollection<MemberDets> ConvertToList(DataSet ds) {
            ObservableCollection<MemberDets> list = new ObservableCollection<MemberDets>();
            foreach (DataRow row in ds.Tables[0].Rows) {
                list.Add(new MemberDets {
                    MemberID = Convert.ToInt32(row["UserID"]),
                    MemberName = row["UserName"].ToString(),
                    GroupID = Convert.ToInt32(row["StatusID"]),
                    GroupName = row["StatusName"].ToString(),
                    Enabled = Convert.ToBoolean(row["Enabled"]),
                    JoinDate = Convert.ToDateTime(row["AddingDate"])
                });
            }
            return list;
        }

        /// <summary>
        /// return the members for a given status form the database
        /// </summary>
        /// <param name="status">the status to look for</param>
        /// <returns>DataSet</returns>
        public static DataSet GetUsersForStatus(int status) {
            OleDbCommand cmd = new OleDbCommand("GetUsersForStatus", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                myConn.Open();
                OleDbParameter param;
                param = cmd.Parameters.Add("@StatusID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = status;
                adapter.Fill(ds, "UsersTbl");
                ds.Tables["UsersTbl"].PrimaryKey = new DataColumn[] { ds.Tables["UsersTbl"].Columns["UserID"] };
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
            DataRow row = ds.Tables[0].NewRow();
            row["UserID"] = -1;
            row["UserName"] = "בחר חבר קבוצה";
            ds.Tables[0].Rows.Add(row);
            return ds;
        }

        /// <summary>
        /// Add a member to the database
        /// </summary>
        /// <param name="userName">the name of the member</param>
        /// <param name="status">the status of the member: 1 for elder/mentor, 2-building, 3-programing, 4-sketching, 5-chairmans, 6-media</param>
        /// <param name="date">the yaer of register. 1/9/201Y, when Y is the start of the study year in the Yod 10th class</param>
        public static bool AddUser(string userName, int status, DateTime date) {
            bool success = false;
            DateTime time = new DateTime(date.Year, 9, 1);
            if (date.Month < 9) {
                time.AddYears(-1);
            }
            try {
                OleDbCommand cmd = new OleDbCommand("InsertNewUser", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@UserName", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = userName;

                param = cmd.Parameters.Add("@StatusID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = status;

                param = cmd.Parameters.Add("@Date", OleDbType.Date);
                param.Direction = ParameterDirection.Input;
                param.Value = time;

                myConn.Open();
                if (cmd.ExecuteNonQuery() == 1) {
                    success = true;
                }
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
            return success;
        }

        /// <summary>
        /// check the member as unEnabled in the database
        /// </summary>
        /// <param name="userID">the member to cancle</param>
        public static void CancleUser(int userID) {
            try {
                OleDbCommand cmd = new OleDbCommand("CancleUser", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@UserID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = userID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }

        /// <summary>
        /// check and change the members that are elders for the team
        /// </summary>
        /// <param name="now">this year. change to Elder whoever this year is 20/6/201X, when X is the number of register year + 3</param>
        public static void checkIsElder() {
            changeToElder(getToElderID());
        }
        private static object[] getToElderID() {//GetUserIDNDate
            DataSet ds = new DataSet();
            try {
                OleDbCommand cmd = new OleDbCommand("GetUserIDNDate", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds, "UsersTbl");
                ds.Tables["UsersTbl"].PrimaryKey = new DataColumn[] { ds.Tables["UsersTbl"].Columns["UserID"] };
            } catch (Exception ex) { throw ex; }
            System.Collections.ArrayList ids = new System.Collections.ArrayList();
            foreach (DataRow dr in ds.Tables[0].Rows) {
                DateTime date = (DateTime)dr["AddingDate"];
                date = date.AddYears(3);
                if (date < DateTime.Now)
                    ids.Add((int)dr["UserID"]);
            }
            return ids.ToArray();
        }
        private static void changeToElder(object[] ids) {
            foreach (object user in ids) {
                int userID;
                if (user is int) {
                    userID = Convert.ToInt32(user);
                    try {
                        myConn.Open();
                        OleDbCommand myComm = new OleDbCommand("MakeUserElder", myConn);
                        myComm.CommandType = CommandType.StoredProcedure;
                        OleDbParameter param;
                        param = myComm.Parameters.Add("UserID", OleDbType.Integer);
                        param.Direction = ParameterDirection.Input;
                        param.Value = user;
                        myComm.ExecuteNonQuery();
                    } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
                }
            }
        }

        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addFirstRow">0 - Get Status, else - Get All statuses, 1 - show all, 2 - choose team, 3 - with mentor</param>
        /// <returns></returns>
        public static ObservableCollection<GroupDets> GetAllStatus(int addFirstRow) {
            DataSet ds = new DataSet();
            OleDbCommand cmd = new OleDbCommand((addFirstRow != 0 ? "GetStatus" : "GetAllStatus"), myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            try {
                adapter.Fill(ds, "StatusTbl");
                ds.Tables["StatusTbl"].PrimaryKey = new DataColumn[] { ds.Tables["StatusTbl"].Columns["StatusID"] };
            } catch (Exception ex) { throw ex; }
            if (addFirstRow != 0) {
                DataRow row;
                if (addFirstRow == 3) {
                    row = ds.Tables[0].NewRow();
                    row["StatusID"] = -1;
                    row["StatusName"] = "חברי קבוצה";
                    row["Enabled"] = "True";
                    ds.Tables[0].Rows.Add(row);
                }
                row = ds.Tables[0].NewRow();
                row["StatusID"] = 0;
                row["StatusName"] = (addFirstRow == 1 || addFirstRow == 3 ? "הכל" : "בחר צוות");
                row["Enabled"] = "True";
                ds.Tables[0].Rows.Add(row);

            }
            DataColumn dc = new DataColumn("Restore", typeof(System.String));
            dc.Unique = false;
            ds.Tables[0].Columns.Add(dc);
            foreach (DataRow dr in ds.Tables[0].Rows) {
                dr["Restore"] = (Convert.ToBoolean(dr["Enabled"].ToString()) ? "ביטול" : "שיחזור");
            }

            return ConvertToGroupsList(ds);
        }

        public static ObservableCollection<GroupDets> ConvertToGroupsList(DataSet ds) {
            ObservableCollection<GroupDets> list = new ObservableCollection<GroupDets>();
            foreach (DataRow row in ds.Tables[0].Rows) {
                if (Convert.ToInt32(row["StatusID"]) > 0)
                    list.Add(new GroupDets {
                        GroupID = Convert.ToInt32(row["StatusID"]),
                        GroupName = row["StatusName"].ToString(),
                        Enabled = Convert.ToBoolean(row["Enabled"])
                    });
            }
            return list;
        }

        public static void RestoreUser(int userID) {
            try {
                OleDbCommand cmd = new OleDbCommand("RestoreUserByID", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@UserID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = userID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }

        public static void AddStatus(string statusName) {
            try {
                OleDbCommand cmd = new OleDbCommand("InsertNewStatus", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@StatusName", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = statusName;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }

        public static void restoreStatus(int statusID) {
            try {
                OleDbCommand cmd = new OleDbCommand("RestoreStatusByID", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@StatusID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = statusID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static void cancleStatus(int statusID) {
            try {
                OleDbCommand cmd = new OleDbCommand("CancleStatus", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@StatusID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = statusID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static DataSet GetNotUsers() {
            OleDbCommand cmd = new OleDbCommand("GetNotUsers", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "UsersTbl");
                ds.Tables["UsersTbl"].PrimaryKey = new DataColumn[] { ds.Tables["UsersTbl"].Columns["UserID"] };
            } catch (Exception ex) { throw ex; }
            DataColumn dc = new DataColumn("Restore", typeof(System.String));
            dc.Unique = false;
            ds.Tables[0].Columns.Add(dc);
            foreach (DataRow dr in ds.Tables[0].Rows) {
                dr["Restore"] = "שחזר";
            }
            return ds;
        }

        //
        public static bool Login(string userName, string password) {
            bool loged = false;
            try {
                OleDbCommand cmd = new OleDbCommand((userName != "" ? "Login" : "LoginByID"), myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                if (userName != "") {
                    param = cmd.Parameters.Add("@UserName", OleDbType.BSTR);
                    param.Direction = ParameterDirection.Input;
                    param.Value = userName;
                }
                param = cmd.Parameters.Add("@Password", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = password;
                myConn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != null && obj.ToString() == password)
                    loged = true;
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
            return loged;
        }
        public static bool AddMeneger(string userName, string password) {
            if (Login("", password)) {
                return false;
            }
            try {
                OleDbCommand cmd = new OleDbCommand("AddMeneger", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@UserName", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = userName;
                param = cmd.Parameters.Add("@Password", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = password;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
            return true;
        }
        public static void DeleteMeneger(string userName, string password) {
            try {
                OleDbCommand cmd = new OleDbCommand("DeleteMeneger", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@UserName", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = userName;
                param = cmd.Parameters.Add("@Password", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = password;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static ObservableCollection<ManagerDets> GetAllMenegers() {
            DataSet ds = new DataSet();
            OleDbCommand cmd = new OleDbCommand("GetAllMenegers", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            try {
                adapter.Fill(ds, "MenegersTbl");
                ds.Tables["MenegersTbl"].PrimaryKey = new DataColumn[] { ds.Tables["MenegersTbl"].Columns["Password"], ds.Tables["MenegersTbl"].Columns["UserName"] };
            } catch (Exception ex) { throw ex; }
            DataRow row = ds.Tables[0].NewRow();
            row["Password"] = "0";
            row["UserName"] = "בחר משתמש";
            ds.Tables[0].Rows.InsertAt(row, 0);
            return ConvertToManagerList(ds);
        }

        public static ObservableCollection<ManagerDets> ConvertToManagerList(DataSet ds) {
            ObservableCollection<ManagerDets> list = new ObservableCollection<ManagerDets>();
            foreach (DataRow row in ds.Tables[0].Rows) {
                list.Add(new ManagerDets {
                    UserName = row["UserName"].ToString(),
                    Password = row["Password"].ToString()
                });
            }
            return list;
        }

        public static void UpdateUser(int userID, int StatusID) {
            try {
                OleDbCommand cmd = new OleDbCommand("UpdateCrew", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@StatusID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = StatusID;
                param = cmd.Parameters.Add("@UserID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = userID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
    }
}
