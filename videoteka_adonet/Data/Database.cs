using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace videoteka_adonet.Data
{
    // Minimal ADO.NET helper for simple student-like project
    public static class Database
    {
        private static string ConnectionString
        {
            get
            {
                // Try to read from App.config using ConfigurationManager
                try
                {
                    var cs = ConfigurationManager.ConnectionStrings["VideotekaDB"]?.ConnectionString;
                    if (!string.IsNullOrWhiteSpace(cs))
                        return cs;
                }
                catch
                {
                    // ignore and fallback
                }

                // Fallback connection string for LocalDB — used when App.config is not found/copied
                return "Server=(localdb)\\MSSQLLocalDB;Database=VideotekaDB;Trusted_Connection=True;MultipleActiveResultSets=True;";
            }
        }

        public static DataTable GetDataTable(string sql, params SqlParameter[] parameters)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                conn.Open();
                da.Fill(dt);
            }
            return dt;
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}
