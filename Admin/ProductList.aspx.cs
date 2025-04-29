using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyCuaHangLiderCloset.Admin
{
    public partial class ProductList : System.Web.UI.Page
    {


        private readonly DataAccess _da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCumbTitle"] = "Danh sách sản phẩm";
                Session["breadCumbPage"] = "Quản lý";
                LoadCategories();
                LoadProducts();
            }
        }

        void LoadCategories()
        {
            try
            {
                string query = "SELECT MaHangMuc, TenHangMuc FROM tblHANGMUC WHERE isActive = 1 ORDER BY TenHangMuc";
                DataTable dt = _da.ExecuteQuery(query);
                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "TenHangMuc";
                ddlCategory.DataValueField = "MaHangMuc";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
            catch (Exception ex)
            {
                Response.Write($"<div class='alert alert-danger'>Lỗi khi tải danh mục: {ex.Message}</div>");
            }
        }

        void LoadProducts()
        {
            try
            {
                string query = @"
                    SELECT sp.MaSP, sp.TenSP, sp.ImageUrl, sp.DonGia, sp.isActive, 
                           hm.TenHangMuc, hmc.TenHMC
                    FROM tblSANPHAM sp
                    LEFT JOIN tblHANGMUCCON hmc ON sp.MaHMC = hmc.MaHMC
                    LEFT JOIN tblHANGMUC hm ON hmc.MaHangMuc = hm.MaHangMuc
                    WHERE 1=1";

                if (ddlCategory.SelectedValue != "0")
                {
                    query += " AND hm.MaHangMuc = @MaHangMuc";
                }

                int status = Convert.ToInt32(ddlStatus.SelectedValue);
                if (status != 2)
                {
                    query += " AND sp.isActive = @isActive";
                }

                query += " ORDER BY sp.NgayCapNhat DESC";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaHangMuc", ddlCategory.SelectedValue != "0" ? Convert.ToInt32(ddlCategory.SelectedValue) : (object)DBNull.Value),
                    new SqlParameter("@isActive", status != 2 ? status : (object)DBNull.Value)
                };

                DataTable dt = _da.ExecuteQuery(query, parameters);
                rProductList.DataSource = dt;
                rProductList.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write($"<div class='alert alert-danger'>Lỗi khi tải sản phẩm: {ex.Message}</div>");
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
                    SELECT sp.MaSP, sp.TenSP, sp.ImageUrl, sp.DonGia, sp.isActive, 
                           hm.TenHangMuc, hmc.TenHMC
                    FROM tblSANPHAM sp
                    LEFT JOIN tblHANGMUCCON hmc ON sp.MaHMC = hmc.MaHMC
                    LEFT JOIN tblHANGMUC hm ON hmc.MaHangMuc = hm.MaHangMuc
                    WHERE 1=1";

                if (ddlCategory.SelectedValue != "0")
                {
                    query += " AND hm.MaHangMuc = @MaHangMuc";
                }

                int status = Convert.ToInt32(ddlStatus.SelectedValue);
                if (status != 2)
                {
                    query += " AND sp.isActive = @isActive";
                }

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaHangMuc", ddlCategory.SelectedValue != "0" ? Convert.ToInt32(ddlCategory.SelectedValue) : (object)DBNull.Value),
                    new SqlParameter("@isActive", status != 2 ? status : (object)DBNull.Value)
                };

                DataTable dt = _da.ExecuteQuery(query, parameters);

                // Xuất CSV
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ID,Tên sản phẩm,Giá,Danh mục,Danh mục con,Trạng thái");
                foreach (DataRow row in dt.Rows)
                {
                    string statusText = Convert.ToBoolean(row["isActive"]) ? "Hoạt động" : "Không hoạt động";
                    string tenHangMuc = row["TenHangMuc"] != DBNull.Value ? row["TenHangMuc"].ToString() : "Không có";
                    string tenHMC = row["TenHMC"] != DBNull.Value ? row["TenHMC"].ToString() : "Không có";
                    sb.AppendLine($"{row["MaSP"]},\"{row["TenSP"]}\",\"{row["DonGia"]:N0}\",\"{tenHangMuc}\",\"{tenHMC}\",\"{statusText}\"");
                }

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("Content-Disposition", "attachment;filename=ProductList.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write($"<div class='alert alert-danger'>Lỗi khi xuất CSV: {ex.Message}</div>");
            }
        }
    }
}
