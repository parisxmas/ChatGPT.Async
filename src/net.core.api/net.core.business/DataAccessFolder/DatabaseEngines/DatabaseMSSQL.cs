using LanguageWidget.Business.DataAccessFolder.DatabaseEngines.Base;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net.core.business.Base;
using net.core.data.Base;
using net.core.data.DataAccessFolder;

namespace net.core.business.DataAccessFolder.DatabaseEngines
{
    public class DatabaseMSSQL : BaseDatabase, IDatabase
    {
        private string SqlMessage = null;

        public DatabaseMSSQL(IConfiguration configuration, ILogger<LoggerClass> logger, string Env) : base(configuration, logger, DatabaseEngine.MSSQL, Env) { }

        protected override DbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        #region - Run Query -
        public string RunQuery(string Query, List<SQLParameterItem> Params = null)
        {
            string Message = null;
            try
            {
                using (SqlConnection SqlConn = (SqlConnection)GetConnection())
                {
                    using (SqlCommand Cmd = new SqlCommand(Query, SqlConn))
                    {
                        if (Params != null)
                        {
                            for (int i = 0; i < Params.Count; i++)
                            {
                                Cmd.Parameters.Add(new SqlParameter(Params[i].Name, Params[i].Value == null ? DBNull.Value : Params[i].Value));
                            }
                        }

                        if (SqlConn.State != ConnectionState.Open) SqlConn.Open();
                        Cmd.CommandType = CommandType.Text;
                        Cmd.ExecuteNonQuery();
                        Cmd.Dispose();
                    }
                    //Close();
                }
            }
            catch (Exception Err)
            {
                Message = Err.Message;
            }
            return Message;
        }

        #endregion

        #region - Exist Record Control -

        public bool ExistRecord(string Query, List<SQLParameterItem> Params = null)
        {
            bool Status = false;

            DataTable DT = Run(Query, Params).Table;
            if (DT.Rows.Count > 0) Status = true;

            return Status;
        }

        #endregion

        #region - Run Table

        public SQLResult Run(string Query, List<SQLParameterItem> Params = null)
        {
            SQLResult result = new SQLResult();
            result.Table = PrepareDataTable(Query, Params);
            result.SqlQuery = Query;
            result.Message = SqlMessage;
            return result;
        }

        protected override DataTable PrepareDataTable(string Query, List<SQLParameterItem> Params)
        {
            DataTable DT = new DataTable();
            SqlMessage = null; // Clear

            try
            {
                using (SqlConnection SqlConn = (SqlConnection)GetConnection())
                {
                    using (SqlDataAdapter DA = new SqlDataAdapter(Query, SqlConn))
                    {
                        if (Query.ToLower().Substring(0, 3) == "sp_") DA.SelectCommand.CommandType = CommandType.StoredProcedure;
                        if (Params != null)
                        {
                            for (int i = 0; i < Params.Count; i++)
                            {
                                if (Query.ToLower().Substring(0, 3) != "sp_")
                                {
                                    if (Params[i].Value == null) Query = Query.Replace("@" + Params[i].Name, "NULL");
                                    else
                                    {
                                        if (Function.IsInteger(Params[i].Value)) Query = Query.Replace("@" + Params[i].Name, Params[i].Value.ToString());
                                        else Query = Query.Replace("@" + Params[i].Name, Params[i].Value == null ? "NULL" : "'" + Params[i].Value.ToString() + "'");
                                    }
                                }
                                else
                                {
                                    Query += " @" + Params[i].Name + "=" + (Params[i].Value == null ? "NULL" : "'" + Params[i].Value.ToString() + "'") + ",";
                                }
                                DA.SelectCommand.Parameters.Add(Params[i].Name, (SqlDbType)Enum.Parse(typeof(SqlDbType), Params[i].Type.ToString())).Value = Params[i].Value == null ? DBNull.Value : Params[i].Value;
                            }

                            if (Query.ToLower().Substring(0, 3) == "sp_" && Query.IndexOf("@") != -1)
                            {
                                Query = Query.Substring(0, Query.Length - 1);
                            }
                        }

                        SqlLog(Query);

                        if (SqlConn.State != ConnectionState.Open) SqlConn.Open();
                        DA.Fill(DT);
                    }
                }
            }
            catch (Exception Err)
            {
                SqlMessage = Err.Message;
            }

            return DT;
        }
        #endregion
    }
}
