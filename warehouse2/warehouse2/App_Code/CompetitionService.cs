using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace warehouse2 {
    class CompetitionService {
        private static OleDbConnection myConn;
        static CompetitionService() {
            myConn = new OleDbConnection(Connect.GetConnectionString());
        }

        public static DataSet GetKinds() {
            OleDbCommand cmd = new OleDbCommand("GetCompetitionKinds", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "KindsTbl");
                ds.Tables["KindsTbl"].PrimaryKey = new DataColumn[] { ds.Tables["KindsTbl"].Columns["KindID"] };
            } catch (Exception ex) { throw ex; }
            DataRow row = ds.Tables[0].NewRow();
            row["KindID"] = 0;
            row["KindName"] = "הכל";
            ds.Tables[0].Rows.Add(row);
            return ds;
        }
        public static DataSet GetInTools(int? kind) {
            OleDbCommand cmd = new OleDbCommand("GetInToolsComp", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "ToolsTbl");
                ds.Tables["ToolsTbl"].PrimaryKey = new DataColumn[] { ds.Tables["ToolsTbl"].Columns["ToolID"] };
            } catch (Exception ex) { throw ex; }
            DataRow row = ds.Tables[0].NewRow();
            row["ToolID"] = 0;
            row["ToolName"] = "בחר כלי";
            ds.Tables[0].Rows.Add(row);
            return GetFixToolTable(ds, kind);
        }
        /// <summary>
        /// return an orgenised table of tools
        /// </summary>
        /// <param name="first">the dataset to fix</param>
        /// <returns></returns>
        private static DataSet GetFixToolTable(DataSet first, int? kind) {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("ToolsTbl");
            DataColumn dc = new DataColumn("ToolID", typeof(System.Int32));
            dc.Unique = false;
            dt.Columns.Add(dc);
            dc = new DataColumn("KindName", typeof(System.String));
            dc.Unique = false;
            dt.Columns.Add(dc);
            dc = new DataColumn("ToolName", typeof(System.String));
            dc.Unique = false;
            dt.Columns.Add(dc);
            dt.PrimaryKey = new DataColumn[] { dt.Columns["UserName"], dt.Columns["ToolName"], dt.Columns["TakeTime"] };
            DataRow dr;
            foreach (DataRow row in first.Tables[0].Rows) {
                if (kind == null || kind == Convert.ToInt32(row["KindID"])) {
                    dr = dt.NewRow();
                    dr["ToolID"] = row["ToolID"];
                    dr["ToolName"] = "" + (kind != null ? "" : row["KindName"] + " ") + row["ToolName"] + " " + row["Numberring"];
                    dt.Rows.Add(dr);
                }
            }
            ds.Tables.Add(dt);
            return ds;
        }
        public static DataSet GetOutTools(bool fix) {
            OleDbCommand cmd = new OleDbCommand("GetOutToolsNamesComp", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "ToolsTakenTbl");
                ds.Tables["ToolsTakenTbl"].PrimaryKey = new DataColumn[] { ds.Tables["ToolsTakenTbl"].Columns["UserID"], ds.Tables["ToolsTakenTbl"].Columns["ToolID"] };
            } catch (Exception ex) { throw ex; }
            if (fix)
                return GetFixedTable(ds);
            else
                return ds;
        }
        private static DataSet GetFixedTable(DataSet first) {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("TakeOutCompetition");
            DataColumn dc;
            dc = new DataColumn("ToolName", typeof(System.String));
            dc.Unique = false;
            dt.Columns.Add(dc);
            dc = new DataColumn("ToolID", typeof(System.Int32));
            dc.Unique = false;
            dt.Columns.Add(dc);
            dc = new DataColumn("UserName", typeof(System.String));
            dc.Unique = false;
            dt.Columns.Add(dc);
            dc = new DataColumn("TeamName", typeof(System.String));
            dc.Unique = false;
            dt.Columns.Add(dc);
            dc = new DataColumn("TakeTime", typeof(System.DateTime));
            dc.Unique = false;
            dt.Columns.Add(dc);
            dc = new DataColumn("Return", typeof(System.String));
            dc.Unique = false;
            dt.Columns.Add(dc);
            dt.PrimaryKey = new DataColumn[] { dt.Columns["ToolName"], dt.Columns["TakeTime"] };
            DataRow dr;
            foreach (DataRow row in first.Tables[0].Rows) {
                dr = dt.NewRow();
                dr["UserName"] = row["UserName"];
                dr["ToolName"] = "" + row["KindName"] + " " + row["ToolName"] + " " + row["Numberring"];
                dr["TeamName"] = "" + row["TeamNum"] + " - " + row["TeamName"];
                dr["ToolID"] = row["ToolID"];
                dr["TakeTime"] = row["TakeDate"];
                dr["Return"] = "החזר";
                dt.Rows.Add(dr);
            }
            ds.Tables.Add(dt);
            return ds;
        }
        public static DataSet GetAllTools() {
            OleDbCommand cmd = new OleDbCommand("GetAllToolsComp", myConn);
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
        public static void TakeOutTool(int toolID, string userName, string teamName, string teamNum) {
            if ((teamNum == null || teamNum == "מספר קבוצה") && (teamName == null || teamName == "")) {
                teamNum = "1943";
                teamName = "Neat Team";
            } else if (teamNum == null || teamNum == "מספר קבוצה") {
                teamNum = "0";
            } else if (teamName == null || teamName == "") {
                teamName = "";
            }
            try {
                myConn.Open();
                OleDbCommand myComm = new OleDbCommand("TakeOutCompetition", myConn);
                myComm.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;

                param = myComm.Parameters.Add("@ToolID", OleDbType.Integer);
                param.Direction = ParameterDirection.Input;
                param.Value = toolID;

                param = myComm.Parameters.Add("@TeamNum", OleDbType.Integer);
                param.Direction = ParameterDirection.Input;
                param.Value = Convert.ToInt32(teamNum);

                param = myComm.Parameters.Add("@TeamName", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = teamName;

                param = myComm.Parameters.Add("@UserName", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = userName;
                myComm.ExecuteNonQuery();

            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static void ReturnTool(int toolID, DateTime takeTime) {
            try {
                OleDbCommand cmd = new OleDbCommand("ReturnCompetitionTool", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
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
        public static void ResetComp() {
            try {
                OleDbCommand cmd = new OleDbCommand("ClearCompetition", myConn);
                OleDbCommand cmd2 = new OleDbCommand("DelCompetition", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd2.CommandType = CommandType.StoredProcedure;
                myConn.Open();
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static void AddTool(int toolID) {
            try {
                OleDbCommand cmd = new OleDbCommand("AddToolToCompetition", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@ToolID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = toolID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static void RemoveTool(int toolID) {
            try {
                OleDbCommand cmd = new OleDbCommand("RemoveToolFromCompetition", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@ToolID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = toolID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static bool CheckIfToolIn(int ToolID) {
            bool isIn = false;
            try {
                OleDbCommand cmd = new OleDbCommand("CheckIfToolInComp", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@ToolID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = ToolID;
                myConn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != null && Convert.ToInt32(obj) == ToolID)
                    isIn = true;
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
            return isIn;
        }
        public static bool CheckIfToolTaken(int ToolID) {
            // CheckIfTaken
            bool isTaken = false;
            try {
                OleDbCommand cmd = new OleDbCommand("CheckIfTaken", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@ToolID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = ToolID;
                myConn.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != null && Convert.ToInt32(obj) == ToolID)
                    isTaken = true;
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
            return isTaken;
        }
        public static void SetToolPlace(int toolID, string place) {
            try {
                OleDbCommand cmd = new OleDbCommand("SetToolPlace", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@Place", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = place;
                param = cmd.Parameters.Add("@ToolID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = toolID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
    }
}
