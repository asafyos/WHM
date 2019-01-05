using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Collections.ObjectModel;

namespace warehouse2 {
    class TaskService {
        static OleDbConnection myConn;
        static TaskService() {
            myConn = new OleDbConnection(Connect.GetConnectionString());
        }

        public static ObservableCollection<TaskDets> getTasks() {
            OleDbCommand cmd = new OleDbCommand("GetTasks", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "UsersTbl");
                ds.Tables["UsersTbl"].PrimaryKey = new DataColumn[] { ds.Tables["UsersTbl"].Columns["UserID"] };
            } catch (Exception ex) { throw ex; }

            DataColumn dc = new DataColumn("Done", typeof(System.String));
            dc.Unique = false;
            ds.Tables[0].Columns.Add(dc);
            foreach (DataRow dr in ds.Tables[0].Rows) {
                dr["Done"] = "הסתיים";
            }
            return ConvertToList(ds);
        }
        public static ObservableCollection<TaskDets> ConvertToList(DataSet ds) {
            ObservableCollection<TaskDets> list = new ObservableCollection<TaskDets>();
            foreach (DataRow row in ds.Tables[0].Rows) {
                list.Add(new TaskDets {
                    TaskID = Convert.ToInt32(row["taskID"].ToString()),
                    TaskText = row["taskText"].ToString()
                });
            }
            return list;
        }
        public static bool addTask(string taskText) {
            bool success = false;
            try {
                OleDbCommand cmd = new OleDbCommand("InsertNewTask", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@TaskText", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = taskText;

                myConn.Open();
                if (cmd.ExecuteNonQuery() == 1) {
                    success = true;
                }
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
            return success;
        }

        public static void doneTask(int taskID) {
            try {
                OleDbCommand cmd = new OleDbCommand("DoneTask", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@TaskID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = taskID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
    }
}
