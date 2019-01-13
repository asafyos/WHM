using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Collections.ObjectModel;
using System.Threading;

namespace warehouse2 {
    class TeamService {
        private static OleDbConnection myConn;

        private const string FRC_URL = "http://www.thebluealliance.com/api/v3/teams/";
        private const string FINISH = "/simple";
        private const string END_OF_OUTPUT = "[END_OF_OUTPUT]";
        private const int PAGE_SIZE = 500;
        public static Thread MainThread { get; set; }

        static TeamService() {
            myConn = new OleDbConnection(Connect.GetConnectionStringTeams());
        }

        public static string getTeamName(int teamNumber) {
            int page = teamNumber / PAGE_SIZE;
            try {
                WebRequest req = WebRequest.Create(FRC_URL + page + FINISH);
                req.Headers.Add("X-TBA-Auth-Key", "IYzOdICVebpdvXKhrxwcpFVip66RadQzqPidfFimwwjHTnZehOLaTKb3UIaQVTxu");
                Stream stream = req.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string output = "";
                while (!reader.EndOfStream) {
                    output += reader.ReadLine();
                }
                if (output != "[]") {
                    dynamic dyObj = JsonConvert.DeserializeObject(output);
                    foreach (var data in dyObj) {
                        if (data.team_number == teamNumber) {
                            return data.nickname;
                        }
                    }
                } else {
                    return END_OF_OUTPUT;
                }
            } catch {
                return getTeamNameOffline(teamNumber);
            }
            return null;
        }
        private static bool addTeamMassive(int page) {
            try {
                WebRequest req = WebRequest.Create(FRC_URL + page + FINISH);
                req.Headers.Add("X-TBA-Auth-Key", "IYzOdICVebpdvXKhrxwcpFVip66RadQzqPidfFimwwjHTnZehOLaTKb3UIaQVTxu");
                Stream stream = req.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string output = "";
                while (!reader.EndOfStream) {
                    output += reader.ReadLine();
                }
                if (output != "[]") {
                    dynamic dyObj = JsonConvert.DeserializeObject(output);
                    foreach (var data in dyObj) {
                        addTeam(Convert.ToInt32(data.team_number), Convert.ToString(data.nickname));
                    }
                } else {
                    return false;
                }
            } catch (Exception ex) { Console.WriteLine(ex.Message); }
            return true;
        }
        private static string getTeamNameOffline(int teamNumber) {
            // TASK: Add offline teams
            object obj;
            try {
                OleDbCommand cmd = new OleDbCommand("GetTeamName", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@TeamNum", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = teamNumber;
                myConn.Open();
                obj = cmd.ExecuteScalar();
            } catch (Exception ex) {
                Console.WriteLine(ex.StackTrace);
                return null;
            } finally { myConn.Close(); }
            if (obj != null)
                return obj.ToString();
            else
                return null;
        }
        public static void updateDB() {
            if (getTeamName(1) != null) {
                delDB();
                for (int page = 0; true; page++) {
                    if (!addTeamMassive(page))
                        break;
                }
                MainThread.Join();
                SharedData.GetInstans().refreshData(TYPE.FIRST);
                MainWindow.mainWin.message("הורדת נתונים הושלמה");
            }
        }
        private static void delDB() {
            try {
                OleDbCommand cmd = new OleDbCommand("delDB", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) {
                throw ex;
            } finally { myConn.Close(); }
        }
        private static void addTeam(int teamNum, string teamName) {
            try {
                OleDbCommand cmd = new OleDbCommand("AddTeam", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                OleDbParameter param;
                param = cmd.Parameters.Add("@TeamNum", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = teamNum;
                param = cmd.Parameters.Add("@TeamName", OleDbType.BSTR);
                param.Direction = ParameterDirection.Input;
                param.Value = teamName;
                myConn.Open();
                cmd.ExecuteNonQuery();
            } catch (Exception ex) { throw ex; } finally { myConn.Close(); }
        }
        public static ObservableCollection<TeamDets> GetAllTeams() {
            ObservableCollection<TeamDets> list = new ObservableCollection<TeamDets>();
            OleDbCommand cmd = new OleDbCommand("GetAllTeams", myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try {
                adapter.Fill(ds, "TeamsTbl");
                ds.Tables["TeamsTbl"].PrimaryKey = new DataColumn[] { ds.Tables["TeamsTbl"].Columns["TeamNum"] };
            } catch (Exception ex) { throw ex; }
            foreach (DataRow row in ds.Tables[0].Rows) {
                list.Add(new TeamDets {
                    TeamName = row["TeamName"].ToString(),
                    TeamNum = Convert.ToInt32(row["TeamNum"])
                });
            }
            return list;
        }
    }
}
