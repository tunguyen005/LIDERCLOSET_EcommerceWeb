using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace QuanLyCuaHangLiderCloset
{
        public class DataAccess
        {
            private string _connectionString = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    cmd.CommandType = query.StartsWith("Category_Crud") || query.StartsWith("SubCategory_Crud") || query.StartsWith("Product_Crud") ? CommandType.StoredProcedure : CommandType.Text;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    cmd.CommandType = query.StartsWith("Category_Crud") || query.StartsWith("SubCategory_Crud") || query.StartsWith("Product_Crud") ? CommandType.StoredProcedure : CommandType.Text;
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    
    public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thực thi ExecuteScalar: " + ex.Message);
            }
        }
    }
}
