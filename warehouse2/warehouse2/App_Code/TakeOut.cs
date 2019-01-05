using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.OleDb;
using System.Data;
using System.Collections.ObjectModel;

namespace warehouse2 {
    class TakeOut {
        private static OleDbConnection myConn;

        static TakeOut() {
            myConn = new OleDbConnection(Connect.GetConnectionString());
        }

        public static ObservableCollection<LoanedTool> GetOutTools() {
            OleDbCommand cmd = new OleDbCommand("GetOutToolsNames", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "ToolsTakenTbl");
                ds.Tables["ToolsTakenTbl"].PrimaryKey = new DataColumn[] { ds.Tables["ToolsTakenTbl"].Columns["UserID"], ds.Tables["ToolsTakenTbl"].Columns["ToolID"] };
            } catch (Exception ex) { throw ex; }
            return ConvertToList(ds);
        }
        public static ObservableCollection<LoanedTool> ConvertToList(DataSet ds) {
            ObservableCollection<LoanedTool> list = new ObservableCollection<LoanedTool>();
            foreach (DataRow row in ds.Tables[0].Rows) {
                list.Add(new LoanedTool {
                    UserName = row["UserName"].ToString(),
                    UserID = row["UserID"].ToString(),
                    ToolName = row["KindName"].ToString() + " " + row["ToolName"].ToString() + " " + row["Numberring"].ToString(),
                    ToolID = row["ToolID"].ToString(),
                    TakeTime = Convert.ToDateTime(row["TakeDate"]),
                    GroupName = row["StatusName"].ToString(),
                    Storekeeper = row["Storekeeper"].ToString()
                });
            }
            return list;
        }
        public static void TakeOutTool(int UserID, int toolID, string storekeeper) {
            try {
                myConn.Open();
                OleDbCommand myComm = new OleDbCommand("TakeOutTools", myConn);
                myComm.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = myComm.Parameters.Add("@UserID", OleDbType.Integer);
                param.Direction = ParameterDirection.Input;
                param.Value = UserID;

                param = myComm.Parameters.Add("@ToolID", OleDbType.Integer);
                param.Direction = ParameterDirection.Input;
                param.Value = toolID;

                param = myComm.Parameters.Add("@Storekeeper", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = storekeeper;
                myComm.ExecuteNonQuery();

            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static void ReturnTool(int userID, int toolID, DateTime takeTime) {
            try {
                OleDbCommand cmd = new OleDbCommand("ReturnTool", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@UserID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = userID;
                param = cmd.Parameters.Add("@ToolID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = toolID;
                param = cmd.Parameters.Add("@TakeDate", OleDbType.Date);
                param.Direction = ParameterDirection.Input;
                param.Value = takeTime;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static DataSet GetOverTakeTool() {
            OleDbCommand cmd = new OleDbCommand("GetOverTakeTool", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "ToolsTakenTbl");
                ds.Tables["ToolsTakenTbl"].PrimaryKey = new DataColumn[] { ds.Tables["ToolsTakenTbl"].Columns["UserID"], ds.Tables["ToolsTakenTbl"].Columns["ToolID"] };
            } catch (Exception ex) { throw ex; }
            return ds;
        }
        public static DataSet GetOutToolForUser(int userID) {
            OleDbCommand cmd = new OleDbCommand("GetOutToolForUser", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                myConn.Open();
                OleDbParameter param;
                param = cmd.Parameters.Add("@UserID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = userID;
                adapter.Fill(ds, "ToolsTakenTbl");
                ds.Tables["ToolsTakenTbl"].PrimaryKey = new DataColumn[] { ds.Tables["ToolsTakenTbl"].Columns["UserID"], ds.Tables["ToolsTakenTbl"].Columns["ToolID"] };
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
            return ds;
        }
        public static void TakeOutTool(int UserID, ArrayList tools) {
            try {
                foreach (Object o in tools) {
                    int t;
                    if (o is int)
                        t = (int)o;
                    else
                        continue;
                    myConn.Open();
                    OleDbCommand cmd = new OleDbCommand("TakeOutTool", myConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    OleDbParameter param;
                    param = cmd.Parameters.Add("@UserID", OleDbType.Integer);
                    param.Direction = ParameterDirection.Input;
                    param.Value = UserID;

                    param = cmd.Parameters.Add("@ToolID", OleDbType.Integer);
                    param.Direction = ParameterDirection.Input;
                    param.Value = t;

                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static DataSet GetAllTakeOut() {
            OleDbCommand cmd = new OleDbCommand("GetAllTakeOut", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "ToolsTakenTbl");
                ds.Tables["ToolsTakenTbl"].PrimaryKey = new DataColumn[] { ds.Tables["ToolsTakenTbl"].Columns["UserID"], ds.Tables["ToolsTakenTbl"].Columns["ToolID"] };
            } catch (Exception ex) { throw ex; }
            return ds;
        }
    }
}
