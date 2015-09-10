using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

public class SqlDB:IDisposable
    {
        private bool isDisposed = false;

        private string _connectionString;
        public SqlDB(string connString)
        {
            _connectionString = connString;
        }
        public SqlDB(string server, string database, string user, string password) 
        {
        }
        public void Dispose()
        {
            Dispose(true);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            isDisposed = true;
        }
        public void addDTtoDS(ref DataSet ds, string sql, string tablename)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter cmd = new SqlDataAdapter(sql, conn);
            cmd.Fill(ds, tablename);
            conn.Dispose();
            cmd.Dispose();
        }
        public DataTable getDataTable(string sql)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter cmd = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            cmd.Fill(ds, "tmp");
            conn.Dispose();
            cmd.Dispose();
            return ds.Tables["tmp"];
        }
        public void addDTtoDSWithCmd(ref DataSet ds, ref SqlCommand cmd, string tablename)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter custDA = new SqlDataAdapter();
            custDA.SelectCommand = cmd;
            cmd.Connection = conn;
            custDA.Fill(ds, tablename);
            conn.Close();
            custDA.Dispose();
        }

        public DataTable getDataTableWithCmd(ref SqlCommand cmd)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter custDA = new SqlDataAdapter();
            custDA.SelectCommand = cmd;
            cmd.Connection = conn;
            DataSet ds = new DataSet();
            custDA.Fill(ds, "tmp");
            conn.Close();
            custDA.Dispose();
            return ds.Tables["tmp"];
        }

        public DataSet getDataSetCmd(ref SqlCommand cmd)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter custDA = new SqlDataAdapter();
            custDA.SelectCommand = cmd;
            cmd.Connection = conn;
            DataSet ds = new DataSet();
            custDA.Fill(ds);
            conn.Close();
            custDA.Dispose();
            return ds;
        }
        public void execSql(string sql)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
        }

        public void execSqlWithCmd(ref SqlCommand cmd)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        public object getSingalValue(string sql)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            object o = cmd.ExecuteScalar();
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
            return o;
        }

        public object getSignalValueCmd(ref SqlCommand cmd)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            cmd.Connection = conn;
            conn.Open();
            object o = cmd.ExecuteScalar();
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
            return o;
        } 
    }
