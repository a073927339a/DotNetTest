using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using Realtek.Commons;
using log4net;

namespace Realtek.IntraLogin.Commons
{
    public class DaoHelper
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<SqlCommand> _commandList = new List<SqlCommand>();
        private int _commandTimeout = 120;
        private string _commandText;
        private List<SqlParameter> _paramList;
        private string _dbKey;

        private DaoHelper() { }

        public static DaoHelper Create()
        {
            return new DaoHelper();
        }

        public static DaoHelper Create(string cmdText)
        {
            return Create().SetCommand(cmdText);
        }

        public DaoHelper SetCommand(string cmdText)
        {
            this._commandText = cmdText;
            this._paramList = new List<SqlParameter>();

            return this;
        }

        public DaoHelper SetConnection(string dbKey)
        {
            this._dbKey = dbKey;

            return this;
        }

        public DaoHelper Batch(string cmdText)
        {
            SqlCommand command = CreateCommand();
            if (command != null)
            {
                this._commandList.Add(command);
            }

            this.SetCommand(cmdText);

            return this;
        }

        public DaoHelper AddParameter(string name, object value)
        {
            SqlParameter parameter = new SqlParameter(name, value);
            if (value == null)
                parameter = new SqlParameter(name, DBNull.Value);
            this._paramList.Add(parameter);

            return this;
        }

        public DaoHelper AddParameter(string name, string value)
        {
            SqlParameter parameter = new SqlParameter(name, value);
            if (value == null)
                parameter = new SqlParameter(name, DBNull.Value);
            this._paramList.Add(parameter);

            return this;
        }
        public DaoHelper AddParameter(string name, decimal value)
        {
            SqlParameter parameter = new SqlParameter(name, value);
            this._paramList.Add(parameter);

            return this;
        }
        public DaoHelper AddParameter(string name, int value)
        {
            SqlParameter parameter = new SqlParameter(name, value);
            this._paramList.Add(parameter);

            return this;
        }

        public DaoHelper AddParameterOutput(string name, int size)
        {
            SqlParameter parameter = new SqlParameter(name, SqlDbType.VarChar, size);
            parameter.Direction = ParameterDirection.Output;
            this._paramList.Add(parameter);

            return this;
        }

        public DataSet ExecuteQuerySet()
        {
            DataSet ds = null;
            if (Init())
            {
                try
                {
                    using (SqlConnection conn = OpenConnection())
                    {
                        using (SqlCommand command = CreateCommand())
                        {
                            Prepare(conn, command);
                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            ds = new DataSet();
                            adapter.Fill(ds);
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.ToString(), ex);
                    throw ex;
                    ds = null;
                }
            }

            return ds;
        }

        public DataTable ExecuteQuery(int tableIndex = 0)
        {
            DataTable dt = null;
            DataSet ds = ExecuteQuerySet();
            if (ds != null && ds.Tables.Count > 0 && tableIndex < ds.Tables.Count)
            {
                dt = ds.Tables[tableIndex];
            }

            return dt;
        }

        public DataRow ExecuteQueryRow(int tableIndex = 0, int rowIndex = 0)
        {
            DataRow dr = null;
            DataTable dt = ExecuteQuery(tableIndex);
            if (dt != null && dt.Rows.Count > 0 && rowIndex < dt.Rows.Count)
            {
                dr = dt.Rows[rowIndex];
            }

            return dr;
        }

        public object ExecuteScalar()
        {
            object ret = null;
            try
            {
                if (Init())
                {
                    using (SqlConnection conn = OpenConnection())
                    {
                        using (SqlCommand command = CreateCommand())
                        {
                            Prepare(conn, command);
                            ret = command.ExecuteScalar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString(), ex);
                ret = null;
                throw ex;
            }

            return ret;
        }

        public DaoResult ExecuteUpdate()
        {
            DaoResult result = null;
            if (Init())
            {
                try
                {
                    using (SqlConnection conn = OpenConnection())
                    {
                        if (IsBatchMode())
                        {
                            EndBatch();
                            using (SqlTransaction transaction = conn.BeginTransaction())
                            {
                                foreach (SqlCommand command in _commandList)
                                {
                                    Prepare(conn, command, transaction);
                                    command.ExecuteNonQuery();
                                }
                                transaction.Commit();
                                result = new DaoResult() { IsSuccess = true, Message = "" };
                            }
                        }
                        else
                        {
                            using (SqlCommand command = CreateCommand())
                            {
                                Prepare(conn, command);
                                int updateCount = command.ExecuteNonQuery();
                                string msg = (updateCount == 0) ? "No row affected" : "";
                                result = new DaoResult() { IsSuccess = true, Message = msg };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.ToString(), ex);
                    result = new DaoResult() { IsSuccess = false, Message = ex.ToString() };
                    throw ex;
                }
            }

            return result;
        }

        private bool Init()
        {
            if (this._dbKey == null)
            {
                this._dbKey = AppConfig.DB_KEY;
            }

            return (this._commandText != null);
        }

        private void EndBatch()
        {
            this.Batch("");
        }

        private bool IsBatchMode()
        {
            return (this._commandList.Count > 0);
        }

        private SqlCommand CreateCommand()
        {
            SqlCommand command = null;
            if (!string.IsNullOrEmpty(this._commandText))
            {
                command = new SqlCommand(this._commandText);
                if (this._paramList.Count > 0)
                {
                    foreach (SqlParameter p in this._paramList)
                    {
                        command.Parameters.Add(p);
                    }
                }
            }

            return command;
        }

        private void Prepare(SqlConnection conn, SqlCommand command, SqlTransaction transaction = null)
        {
            command.CommandTimeout = this._commandTimeout;
            command.Connection = conn;
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            LogCommand(command);
        }

        private void LogCommand(SqlCommand sqlCmd)
        {
            StringBuilder cmdSB = new StringBuilder();
            cmdSB.Append("SQL COMMAND => " + sqlCmd.CommandText + "  ");
            List<string> values = new List<string>();
            foreach (DbParameter param in sqlCmd.Parameters)
            {
                //cmdSB.AppendFormat(" {0} = '{1}'; ", param.ParameterName, param.Value);
                if (param.Value != null)
                    values.Add("'" + param.Value + "'");
                else
                    values.Add("null");
            }
            cmdSB.Append(string.Join(", ", values));

            _logger.Debug(cmdSB.ToString());
        }

        private SqlConnection CreateConnection(string dbKey)
        {
            string connString = ConfigManager.GetConnectionString(dbKey);

            return new SqlConnection(connString);
        }

        private SqlConnection OpenConnection()
        {
            SqlConnection conn = CreateConnection(this._dbKey);
            conn.Open();

            return conn;
        }
    }

    public class DaoResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}