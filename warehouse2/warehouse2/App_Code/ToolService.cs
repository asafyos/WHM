using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Collections.ObjectModel;

namespace warehouse2 {
    class ToolService {
        private static OleDbConnection myConn;
        static ToolService() {
            myConn = new OleDbConnection(Connect.GetConnectionString());
        }

        /// <summary>
        /// return all the tools in the database
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetInTools() {
            OleDbCommand cmd = new OleDbCommand("GetInTools", myConn);
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
            return getFixToolTable(ds, false);
        }

        /// <summary>
        /// return the tools for a given kind form the database
        /// </summary>
        /// <param name="kind">the kind to look for</param>
        /// <returns>DataSet</returns>
        public static DataSet GetInToolsForKind(int kind) {
            OleDbCommand cmd = new OleDbCommand("GetInToolsForKind", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                myConn.Open();
                OleDbParameter param;
                param = cmd.Parameters.Add("@StatusID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = kind;
                adapter.Fill(ds, "ToolsTbl");
                ds.Tables["ToolsTbl"].PrimaryKey = new DataColumn[] { ds.Tables["ToolsTbl"].Columns["ToolID"] };
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
            DataRow row = ds.Tables[0].NewRow();
            row["ToolID"] = 0;
            row["ToolName"] = "בחר כלי";
            ds.Tables[0].Rows.Add(row);
            return getFixToolTable(ds, true);
        }

        /// <summary>
        /// return an orgenised table of tools
        /// </summary>
        /// <param name="first">the dataset to fix</param>
        /// <returns></returns>
        private static DataSet getFixToolTable(DataSet first, bool byKind) {
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
                dr = dt.NewRow();
                dr["ToolID"] = row["ToolID"];
                dr["ToolName"] = "" + (byKind ? "" : row["KindName"] + " ") + row["ToolName"] + " " + row["Numberring"];
                dt.Rows.Add(dr);
            }
            ds.Tables.Add(dt);
            return ds;
        }

        /// <summary>
        /// Add a tool to the database
        /// </summary>
        /// <param name="ToolName">the name of the tool</param>
        /// <param name="Numberring">the numberring of the tool</param>
        /// <param name="KindID">the kind of the tool</param>
        /// <returns></returns>
        public static bool AddTool(string ToolName, string Numberring, int KindID) {
            bool success = false;
            try {
                OleDbCommand cmd = new OleDbCommand("InsertNewTool", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@ToolName", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = ToolName;

                param = cmd.Parameters.Add("@KindID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = KindID;

                param = cmd.Parameters.Add("@Numberring", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = Numberring;

                myConn.Open();
                if (cmd.ExecuteNonQuery() == 1)
                    success = true;
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
            return success;
        }

        public static ObservableCollection<ToolDets> GetAllTools() {
            OleDbCommand cmd = new OleDbCommand("GetAllToolsName2", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "ToolsTbl");
                ds.Tables["ToolsTbl"].PrimaryKey = new DataColumn[] { ds.Tables["ToolsTbl"].Columns["ToolID"] };
            } catch (Exception ex) { throw ex; }
            DataColumn dc = new DataColumn("Cancle", typeof(System.String));
            dc.Unique = false;
            ds.Tables[0].Columns.Add(dc);
            foreach (DataRow dr in ds.Tables[0].Rows) {
                dr["Cancle"] = "ביטול";
            }
            return ConvertToList(ds);
        }
        public static ObservableCollection<ToolDets> ConvertToList(DataSet ds) {
            ObservableCollection<ToolDets> list = new ObservableCollection<ToolDets>();
            foreach (DataRow row in ds.Tables[0].Rows) {
                list.Add(new ToolDets {
                    ToolName = row["ToolName"].ToString(),
                    ToolID = Convert.ToInt32(row["ToolID"]),
                    //Place = ,
                    Number = row["Numberring"].ToString(),
                    KindID = Convert.ToInt32(row["KindID"]),
                    KindName = row["KindName"].ToString(),
                    Enabled = Convert.ToBoolean(row["Enabled"]),
                    IsComp = row["check"].ToString() != ""
                });
            }
            return list;
        }

        public static DataSet GetCancleTools() {
            OleDbCommand cmd = new OleDbCommand("GetCancleTools", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "ToolsTbl");
                ds.Tables["ToolsTbl"].PrimaryKey = new DataColumn[] { ds.Tables["ToolsTbl"].Columns["ToolID"] };
            } catch (Exception ex) { throw ex; }
            DataColumn dc = new DataColumn("Restore", typeof(System.String));
            dc.Unique = false;
            ds.Tables[0].Columns.Add(dc);
            foreach (DataRow dr in ds.Tables[0].Rows) {
                dr["Restore"] = "שחזר";
            }
            return ds;
        }

        public static void RestoreTool(int toolID) {
            try {
                OleDbCommand cmd = new OleDbCommand("RestoreToolByID", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@ToolID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = toolID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }

        public static ObservableCollection<KindDets> GetAllKinds(bool addFirstRow) {
            OleDbCommand cmd = new OleDbCommand("GetAllKinds", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "KindsTbl");
                ds.Tables["KindsTbl"].PrimaryKey = new DataColumn[] { ds.Tables["KindsTbl"].Columns["KindID"] };
            } catch (Exception ex) { throw ex; }
            DataColumn dc = new DataColumn("Restore", typeof(System.String));
            dc.Unique = false;
            ds.Tables[0].Columns.Add(dc);
            foreach (DataRow dr in ds.Tables[0].Rows) {
                dr["Restore"] = (Convert.ToBoolean(dr["Enabled"].ToString()) ? "ביטול" : "שיחזור");
            }
            return ConvertToKindList(ds);
        }
        public static DataSet GetEnabledKinds(bool addFirstRow) {
            OleDbCommand cmd = new OleDbCommand("GetEnabledKinds", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "KindsTbl");
                ds.Tables["KindsTbl"].PrimaryKey = new DataColumn[] { ds.Tables["KindsTbl"].Columns["KindID"] };
            } catch (Exception ex) { throw ex; }
            if (addFirstRow) {
                DataRow row = ds.Tables[0].NewRow();
                row["KindID"] = 0;
                row["KindName"] = "הכל";
                ds.Tables[0].Rows.Add(row);
            }
            return ds;
        }

        public static bool AddKind(string kindName) {
            bool success = false;
            try {
                OleDbCommand cmd = new OleDbCommand("InsertNewKind", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@KindName", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = kindName;
                myConn.Open();
                if (cmd.ExecuteNonQuery() == 1)
                    success = true;
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
            return success;
        }

        /// <summary>
        /// check the tool as unEnabled in the database
        /// </summary>
        /// <param name="toolID">the tool to cancle</param>
        public static void CancleTool(int toolID) {
            try {
                OleDbCommand cmd = new OleDbCommand("CancleTool", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@ToolID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = toolID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }

        public static void CancleKind(int kindID) {
            try {
                OleDbCommand cmd = new OleDbCommand("CancleKind", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@KIndID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = kindID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static void RestoreKind(int kindID) {
            try {
                OleDbCommand cmd = new OleDbCommand("RestoreKindByID", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@KindID", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = kindID;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }

        public static ObservableCollection<KindDets> ConvertToKindList(DataSet ds) {
            ObservableCollection<KindDets> list = new ObservableCollection<KindDets>();
            foreach (DataRow row in ds.Tables[0].Rows) {
                list.Add(new KindDets {
                    KindID = Convert.ToInt32(row["KindID"]),
                    KindName = row["KindName"].ToString(),
                    Enabled = Convert.ToBoolean(row["Enabled"])
                });
            }
            return list;
        }

        public static void changeKind(int[] toolIds, int kindID) {
            myConn.Open();
            try {
                for (int i = 0; i < toolIds.Length; i++) {
                    OleDbCommand cmd = new OleDbCommand("UpdateToolKind", myConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    OleDbParameter param;
                    param = cmd.Parameters.Add("@KindID", OleDbType.BSTR);
                    param.Direction = ParameterDirection.Input;
                    param.Value = kindID;
                    param = cmd.Parameters.Add("@ToolID", OleDbType.BSTR);
                    param.Direction = ParameterDirection.Input;
                    param.Value = toolIds[i];
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }

        public static bool isToolIn(int toolID) {
            return false;
        }
    }
}
